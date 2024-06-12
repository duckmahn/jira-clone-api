using managementapp.Data;
using managementapp.Data.Models;
using Microsoft.AspNetCore.SignalR;

namespace managementapp.Hubs;

public class SignalHub : Hub
{
    private readonly ShareDb _shareDb;
    public SignalHub(ShareDb shareDb)
    {
        _shareDb = shareDb;
    }
    public async Task JoinCHat(UserConnection connection)
    {
        await Clients.All.SendAsync("ReceiveMessage","JoinChat", connection.Username);
    }
    public async Task LeaveChat(UserConnection connection)
    {
        await Clients.All.SendAsync("ReceiveMessage", "LeaveChat", connection.Username);
    }
    public async Task JoinSpecificChatRoom(UserConnection connection)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, connection.Chatroom);

        _shareDb.connections[Context.ConnectionId] = connection;
        await Clients.Group(connection.Chatroom).SendAsync("ReceiveMessage", "JoinChat", connection);
    }

    public async Task SendMessage(string userName, string message)
    {
        //if(_shareDb.connections.TryGetValue(userName, out var userConnection))
        //{
        //    await Clients.All.SendAsync("ReceiveMessage", "SendMessage", userName, message);

        //}
        await Clients.All.SendAsync("ReceiveMessage", "SendMessage", userName, message);    

    }
    
}
