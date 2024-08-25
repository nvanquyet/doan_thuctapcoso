using Mirror;
using System;
using System.Collections.Generic;
using UnityEngine;
namespace Server
{
    public class RoomManager : MonoBehaviour
    {
        public static RoomManager Instance;

        public int maxPlayersPerRoom = 4;
        private List<Room> rooms = new List<Room>();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public Room CreateRoom()
        {
            Room newRoom = new Room(maxPlayersPerRoom);
            rooms.Add(newRoom);
            return newRoom;
        }

        public bool JoinRoom(NetworkConnection conn, Room room)
        {
            if (room.AddPlayer(conn))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void LeaveRoom(NetworkConnection conn, Room room)
        {
            room.RemovePlayer(conn);
        }


        public Room FindRoom()
        {
            return rooms.Find(r => !r.IsFull());
        }
    }

}