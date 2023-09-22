using MarkdownSharp;
using BlazorDownloadFile;
using HomeEntertainmentAdvisor.Domain.Repo.Interfaces;
using HomeEntertainmentAdvisor.Models;
using HomeEntertainmentAdvisor.Services.Interfaces;
using System.Text;
using System.Xml;
using Syncfusion.Pdf;
using Syncfusion.HtmlConverter;

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
            HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter();
            htmlConverter.ConverterSettings.SplitImages = true;
            htmlConverter.ConverterSettings.SplitTextLines = true;
            PdfDocument document = htmlConverter.Convert(html,"");
            using (MemoryStream ms = new MemoryStream())
            {
                document.Save(ms);
                return ms.ToArray();
            }

        }
    }
}
