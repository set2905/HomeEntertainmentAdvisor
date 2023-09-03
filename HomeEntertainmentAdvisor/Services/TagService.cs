using HomeEntertainmentAdvisor.Domain.Repo.Interfaces;
using HomeEntertainmentAdvisor.Models;
using HomeEntertainmentAdvisor.Services.Interfaces;

namespace HomeEntertainmentAdvisor.Services
{
    public class TagService : ITagService
    {
        private readonly ITagRepo tagRepo;
        private readonly IReviewsRepo reviewsRepo;
        private readonly IReviewTagRelationsRepo reviewsTagRelationsRepo;

        public TagService(ITagRepo tagRepo, IReviewsRepo reviewsRepo, IReviewTagRelationsRepo reviewsTagRelationsRepo)
        {
            this.tagRepo=tagRepo;
            this.reviewsRepo=reviewsRepo;
            this.reviewsTagRelationsRepo=reviewsTagRelationsRepo;
        }
        public async Task<List<Tag>> GetReviewTags(Guid reviewId)
        {
            return await reviewsTagRelationsRepo.GetTagsByReviewId(reviewId);
        }
        public async Task<IEnumerable<string>> SearchByName(string query)
        {
            return (await tagRepo.SearchByName(query)).Select(x => x.Name);
        }
        public async Task<bool> OverwriteTags(Guid reviewId, IEnumerable<string> tags)
        {
            await reviewsTagRelationsRepo.RemoveByReviewId(reviewId);
            return await AddTags(reviewId, tags);

        }
        public async Task<bool> AddTags(Guid reviewId, IEnumerable<string> tags)
        {
            if (await reviewsRepo.GetById(reviewId)==null) return false;
            foreach (string tagName in tags)
            {
                Tag? found = await tagRepo.GetByName(tagName);
                Guid tagId = found!=null ? found.Id : default;
                if (found == null || tagId==default)
                    tagId = await tagRepo.Save(new() { Name=tagName });
                if (await reviewsTagRelationsRepo.GetById((reviewId, tagId))==null)
                    await reviewsTagRelationsRepo.Save(new() { ReviewId=reviewId, TagId=tagId });
            }
            return true;

        }
    }
}
