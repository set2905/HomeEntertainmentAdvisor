using HomeEntertainmentAdvisor.Models;

namespace HomeEntertainmentAdvisor.Services.Interfaces
{
    public interface IAccountService
    {
        Task<List<string>> DeleteUsers(IEnumerable<string> userIds);
        Task<List<User>> GetUsers(int skip = 0, int take = 10);
        Task SetBlock(IEnumerable<string> userIds, bool blockValue);
    }
}