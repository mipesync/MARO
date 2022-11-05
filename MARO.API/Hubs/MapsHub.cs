using Microsoft.AspNetCore.SignalR;

namespace MARO.API.Hubs
{
    public class MapsHub : Hub
    {
        public async Task Enter(Guid userId, string fullName, Guid groupId)
        {
            if (userId == Guid.Empty)
            {
                await Clients.Caller.SendAsync("Notify", "Для входа в чат введите логин");
            }
            else
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, groupId.ToString());
                await Clients.Group(groupId.ToString()).SendAsync("Notify", $"{fullName} присоединился к группе");
            }
        }

        public async Task Send(List<Route> route)
        {

        }
    }
}
