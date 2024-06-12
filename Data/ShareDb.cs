using managementapp.Data.Models;
using System.Collections.Concurrent;

namespace managementapp.Data;

public class ShareDb
{
    private readonly ConcurrentDictionary<string, UserConnection> _connections = new ();

    public ConcurrentDictionary<string, UserConnection> connections => _connections;
}
