using MarkdownSharp;
using HomeEntertainmentAdvisor.Domain.Repo.Interfaces;
using HomeEntertainmentAdvisor.Models;
using HomeEntertainmentAdvisor.Services.Interfaces;
using System.Text;
using HiQPdf;
using Microsoft.JSInterop;
using System.IO;

namespace HomeEntertainmentAdvisor.Services
{
    public class ReviewPDFExporter : IReviewExporter
    {
        private readonly IReviewImagesRepo imagesRepo;
        private readonly IJSRuntime JS;

        public ReviewPDFExporter(IReviewImagesRepo imagesRepo, IJSRuntime JS)
        {
            this.imagesRepo=imagesRepo;
            this.JS=JS;
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
            var fileStream = new MemoryStream(fileContent);
            using var streamRef = new DotNetStreamReference(stream: fileStream);
            await JS.InvokeVoidAsync("downloadFileFromStream", $"{review.Name}.pdf", streamRef);
        }
        private byte[] GetFileContentFromMD(string markdown)
        {
            Markdown markdownSharp = new();
            string html = markdownSharp.Transform(markdown);
            HtmlToPdf htmlToPdfConverter = new HtmlToPdf();
            htmlToPdfConverter.Document.ResizePageWidth=true;
            return htmlToPdfConverter.ConvertHtmlToMemory(html, "");
        }
    }
}
