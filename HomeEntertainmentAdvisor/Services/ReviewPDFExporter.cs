using BlazorDownloadFile;
using HomeEntertainmentAdvisor.Domain.Repo.Interfaces;
using HomeEntertainmentAdvisor.Models;
using HomeEntertainmentAdvisor.Services.Interfaces;
using System.Text;

namespace HomeEntertainmentAdvisor.Services
{
    public class ReviewPDFExporter : IReviewExporter
    {
        private readonly IReviewImagesRepo imagesRepo;
        private readonly IBlazorDownloadFileService blazorDownloadFileService;

        public ReviewPDFExporter(IReviewImagesRepo imagesRepo, IBlazorDownloadFileService blazorDownloadFileService)
        {
            if (IronPdf.Installation.AutomaticallyDownloadNativeBinaries!=true)
                IronPdf.Installation.AutomaticallyDownloadNativeBinaries=true;
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
            ChromePdfRenderer renderer = new ChromePdfRenderer();
            StringBuilder markdownBuilder = new($"### {review.Name}\n");
            markdownBuilder.AppendLine($"{review.Content}");
            IEnumerable<ReviewImage> images = (await imagesRepo.GetImagesForReview(review.Id));
            foreach (ReviewImage img in images)
            {
                markdownBuilder.AppendLine($"![{img.FileName}]({img.Url})");
            }
            PdfDocument contentPDF = renderer.RenderMarkdownStringAsPdf(markdownBuilder.ToString());
            await blazorDownloadFileService.DownloadFile($"{review.Name}.pdf",
                                                         contentPDF.BinaryData,
                                                         32768,
                                                         "application/octet-stream",
                                                         null);
        }
    }
}
