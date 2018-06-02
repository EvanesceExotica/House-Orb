using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class RoomManager : MonoBehaviour
{

    public Queue<Room> roomsPlayerEntered = new Queue<Room>();
    public List<Room> roomsEnteredList = new List<Room>();
    public List<Room> roomList = new List<Room>();

    int maxNumberOfRoomsScentLingersIn;
    [SerializeField] Room playerCurrentRoom;
    [SerializeField] Room enemyCurrentRoom;

    public int numberOfRooms;

    public void SetPlayerCurrentRoom(Room room)
    {
        playerCurrentRoom = room;
    }

    public int GetPlayerCurrentRoomIndex()
    {
        return roomList.IndexOf(playerCurrentRoom);
    }
    public Room GetPlayerCurrentRoom()
    {
        return playerCurrentRoom;
    }

    public void SetEnemyCurrentRoom(Room room)
    {

        enemyCurrentRoom = room;
    }

    void AddRoomsEntered(Room room)
    {
        if (roomsPlayerEntered.Count == 4)
        {
            roomsEnteredList.Remove(roomsPlayerEntered.First());
            roomsEnteredList.Add(room);
            Room scentlessRoom = roomsPlayerEntered.Dequeue();
            ScentDispersed(scentlessRoom);
            roomsPlayerEntered.Enqueue(room);
        }
        else
        {
            if (roomsPlayerEntered.Contains(room))
            {
                roomsEnteredList.Remove(roomsPlayerEntered.First());
                roomsEnteredList.Add(room);
                Room scentlessRoom = roomsPlayerEntered.Dequeue();
                ScentDispersed(scentlessRoom);
                roomsPlayerEntered.Enqueue(room);
            }
            else
            {
                roomsPlayerEntered.Enqueue(room);
                roomsEnteredList.Add(room);
            }
        }



    }

    void ScentDispersed(Room room)
    {
        room.StopScentParticleSystem();
    }

    public int GetEnemyCurrentRoomIndex()
    {

        return roomList.IndexOf(enemyCurrentRoom);
    }
    //TODO: You have to make sure this list grabs the objects in order of hwo they are in the scene -- maybe have a room instantiator
    void Awake()
    {
        roomList = transform.GetComponentsInChildren<Room>().ToList();
        //	levelRooms = FindObjectsOfType<Room>().ToList();
        foreach (Room room in roomList)
        {
            room.PlayerEnteredRoom += SetPlayerCurrentRoom;
            room.PlayerEnteredRoom += AddRoomsEntered;
            room.EnemyEntered += SetEnemyCurrentRoom;
        }
        numberOfRooms = roomList.Count;
    }
    // Use this for initialization
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            Debug.Log(GetPlayerCurrentRoomIndex());
        }
    }
}
