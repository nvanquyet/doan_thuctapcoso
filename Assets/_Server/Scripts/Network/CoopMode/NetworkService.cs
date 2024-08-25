using Mirror;
using UnityEngine;
namespace Server
{

    public class NetworkService : NetworkManager
    {
        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            Room roomToJoin = RoomManager.Instance.FindRoom();

            if (roomToJoin == null)
            {
                roomToJoin = RoomManager.Instance.CreateRoom();
            }

            RoomManager.Instance.JoinRoom(conn, roomToJoin);

            Debug.Log($"Player {conn.connectionId} joined room.");
        }

    }
}
}
