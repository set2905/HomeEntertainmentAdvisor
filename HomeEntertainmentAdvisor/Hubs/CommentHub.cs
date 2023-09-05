using HomeEntertainmentAdvisor.Models;
using Microsoft.AspNetCore.SignalR;

namespace HomeEntertainmentAdvisor.Hubs
{

    public class CommentHub : Hub
    {
        public async Task SendComment(Comment comment)
        {
            await Clients.All.SendAsync("RecieveComment", comment);
        }
    }
}
