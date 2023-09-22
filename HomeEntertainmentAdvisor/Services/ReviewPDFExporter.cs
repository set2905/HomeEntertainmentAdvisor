using MarkdownSharp;
using BlazorDownloadFile;
using HomeEntertainmentAdvisor.Domain.Repo.Interfaces;
using HomeEntertainmentAdvisor.Models;
using HomeEntertainmentAdvisor.Services.Interfaces;
using System.Text;
using Microsoft.AspNetCore.Html;
using System.Drawing.Printing;
using SelectPdf;

namespace HomeEntertainmentAdvisor.Services
{
    public class ReviewPDFExporter : IReviewExporter
    {
        private readonly IReviewImagesRepo imagesRepo;
        private readonly IBlazorDownloadFileService blazorDownloadFileService;

        public ReviewPDFExporter(IReviewImagesRepo imagesRepo, IBlazorDownloadFileService blazorDownloadFileService)
        {
            this.imagesRepo=imagesRepo;
            this.blazorDownloadFileService=blazorDownloadFileService;
        }
        /// <summary>
        /// Exports review as pdf and downloads th file
        /// </summary>
        /// <param name="review"></param>
        /// <returns></returns>
        public async Task ExportReview(Review review)
        {
            StringBuilder markdownBuilder = new($"### {review.Name}\n");
            markdownBuilder.AppendLine($"{review.Content}");
            IEnumerable<ReviewImage> images = (await imagesRepo.GetImagesForReview(review.Id));
            foreach (ReviewImage img in images)
            {
                markdownBuilder.AppendLine($"![{img.FileName}]({img.Url})");
            }
            byte[] fileContent = GetFileContentFromMD(markdownBuilder.ToString());
            await blazorDownloadFileService.DownloadFile($"{review.Name}.pdf",
                                                        fileContent,
                                                         32768,
                                                         "application/octet-stream",
                                                         null);
        }
        private byte[] GetFileContentFromMD(string markdown)
        {
            Markdown markdownSharp = new();
            string html = markdownSharp.Transform(markdown);
            HtmlToPdf converter = new HtmlToPdf();

            // set converter options
            converter.Options.PdfPageSize = PdfPageSize.A4;
            converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;


            // create a new pdf document converting an url
            PdfDocument doc = converter.ConvertHtmlString(html);
            return doc.Save();
        }
    }
}
