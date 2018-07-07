﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
public class Monster : MonoBehaviour
{


    AudioSource ourSource;

    public AudioClip nearScream;

    public AudioClip scoutingScream;

    public AudioClip scareChord;

    int starScreamDirection;

    public GameObject ourLightObject;
    public static event Action MonsterReachedPlayer;
    public static event Action ReadyToScream;

    void ScreamReady()
    {
        PlayScoutingScream();
        if (ReadyToScream != null)
        {
            ReadyToScream();
        }
    }
    RoomManager roomManager;
    // Use this for initialization
    public int playerStartingRoomIndex;


    public int minRoomsAwayFromPlayer = 3;
    List<int> acceptableRoomIndexes;
    public List<Room> acceptableRoomsToDrawFrom;
    void Awake()
    {
        ourSource = GetComponent<AudioSource>();
        roomManager = GameObject.Find("Managers").GetComponent<RoomManager>();
        HidingSpace.PlayerHiding += PlayerHid;
        PromptPlayerHit.PlayerFailed += HuntPlayerWrapper;
        StarScream.ScreamHitPlayerCurrentRoom += SetStarScreamDirection;
        FatherOrb.OrbScream += HurryToRoomOfScreamWrapper;
    }

    void PlayScoutingScream()
    {
        ourSource.PlayOneShot(scoutingScream);
    }

    void PlayLocatedPlayerChord()
    {
        ourSource.PlayOneShot(scareChord);
    }

    void Start()
    {
        StartCoroutine(MonsterInitializationCoroutine());
    }

    IEnumerator MonsterInitializationCoroutine()
    {
        yield return new WaitForSeconds(1);
        ChooseRandomRoom();
    }


    void ChooseRandomRoom()
    {
        playerStartingRoomIndex = roomManager.GetPlayerCurrentRoomIndex();
        acceptableRoomsToDrawFrom = GetRoomAtLeastMinRoomsAway(minRoomsAwayFromPlayer);
        //todo: remove -- for testing purposes
        // foreach (Room room in acceptableRoomsToDrawFrom)
        // {
        //     room.GetComponent<SpriteRenderer>().enabled = false;
        // }
        //remember Random.Range's max is inclusive- that's why you're using Count instead of Count - 1
        int randomIndex = UnityEngine.Random.Range(0, acceptableRoomsToDrawFrom.Count);
        transform.position = acceptableRoomsToDrawFrom[randomIndex].gameObject.transform.position;
        Room acceptableRoom = acceptableRoomsToDrawFrom[randomIndex];
        StartCoroutine(MoveBetweenRooms(roomManager.roomList.IndexOf(acceptableRoom)));
    }

    //int numberOfRoomsToStayAway = 3;
    public List<Room> compilation = new List<Room>();
    public List<Room> GetRoomAtLeastMinRoomsAway(int numberOfRoomsToStayAway)
    {
        //this should probably be in the RoomManager class

        //make a copy of the roomlist -- we're going to delete from them

        List<Room> acceptableRooms = roomManager.roomList.ToList();
        List<Room> compiledRooms = roomManager.roomList.ToList();
        if (playerStartingRoomIndex + numberOfRoomsToStayAway > roomManager.roomList.Count - 1)
        {
            List<Room> addedRooms = compiledRooms.ToList();
            addedRooms.Remove(roomManager.GetPlayerCurrentRoom());
            compiledRooms.AddRange(addedRooms);

        }
        else if (playerStartingRoomIndex - numberOfRoomsToStayAway < 0)
        {
            List<Room> addedRooms = compiledRooms.ToList();
            addedRooms.Remove(roomManager.GetPlayerCurrentRoom());
            compiledRooms.InsertRange(0, addedRooms.ToList());

        }

        compilation = compiledRooms;
        int playerRoomIndexInCompiledRooms = compiledRooms.IndexOf(roomManager.GetPlayerCurrentRoom());
        for (int i = 0; i < compiledRooms.Count - 1; i++)
        {
            if (i <= playerRoomIndexInCompiledRooms + numberOfRoomsToStayAway && i > playerRoomIndexInCompiledRooms)
            {
                acceptableRooms.Remove(compiledRooms[i]);
            }
            if (i >= playerRoomIndexInCompiledRooms - numberOfRoomsToStayAway && i < playerRoomIndexInCompiledRooms)
            {
                acceptableRooms.Remove(compiledRooms[i]);
            }
        }
        acceptableRooms.Remove(roomManager.GetPlayerCurrentRoom());

        return acceptableRooms;

    }

    public float timeSpentInRooms = 10.0f;
    public int roomsBeforeSwitch = 6;
    public int roomsWeveMovedThroughBeforeSwitch = 0;

    public int roomsWeveMovedThroughBeforeFrustrationScream = 10;

    bool orbScreamed;
    public IEnumerator MoveBetweenRooms(int startingRoomIndex)
    {
        bool readyToScream = false;
        int positiveOrNegative = UnityEngine.Random.Range(0, 2) * 2 - 1;
        int currentRoomIndex = startingRoomIndex;
        //TODO: change this value to "timeSpentInRooms"
        yield return new WaitForSeconds(5.0f);
        while (true)
        {
            if (orbScreamed)
            {
                //if the orb was channeled for too long, it'll scream, and you scream back
                Debug.Log("Orb screamed, so we're going to scream back");
                orbScreamed = false;
                readyToScream = true;
                break;
            }
            if (roomManager.roomsPlayerEntered.Contains(roomManager.roomList[currentRoomIndex]))
            {
                foreach (Room room in roomManager.roomsPlayerEntered)
                {
                    Debug.Log("We entered this room " + room);
                }
                //if we enter a room the player has been in recently
                readyToScream = true;
                break;
            }

            if (roomsWeveMovedThroughBeforeSwitch == roomsBeforeSwitch)
            {
                //this will randomly see if the monster continues in the same direction or switches depending on how many rooms we want it  to progress in a single direction before going back the other way
                positiveOrNegative *= UnityEngine.Random.Range(0, 2) * 2 - 1;
                roomsBeforeSwitch = 0;
            }

            currentRoomIndex = roomManager.GetEnemyCurrentRoomIndex() + positiveOrNegative;

            if (currentRoomIndex == -1)
            {
                //if it equals the first index
                currentRoomIndex = roomManager.roomList.Count - 1;
            }
            else if (currentRoomIndex == roomManager.roomList.Count)
            {
                //if it equals the last index
                currentRoomIndex = 0;
            }
            transform.position = roomManager.roomList[currentRoomIndex].gameObject.transform.position;
            roomsWeveMovedThroughBeforeSwitch++;
            //TODO: Change this value to "TimeSpentInRooms
            yield return new WaitForSeconds(5.0f);
        }
        if (readyToScream)
        {
            ScreamReady();
        }
    }

    void SetStarScreamDirection(int direction)
    {
        //this method determines what direction the monster should hunt the player in based on the direction the star scream travelled
        starScreamDirection = direction;
    }
    bool playerHiding = false;
    void PlayerHid(UnityEngine.Object ourObject)
    {
        playerHiding = false;
    }

    void PlayerCameOutOfHiding()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            ScreamReady();
        }
    }

    bool playerSucessfullyHiding;
    bool playerChangedRoom;
    void RecalculatePlayerRoom()
    {
        playerChangedRoom = true;
    }

    void DetermineClosestDistance()
    {

    }

    void MonsterReachedPlayerWrapper()
    {
        if (MonsterReachedPlayer != null)
        {
            MonsterReachedPlayer();
        }

    }

    void SearchPlayer()
    {

    }

    void HuntPlayerWrapper(/*int direction*/)
    {
        PlayLocatedPlayerChord();
        StartCoroutine(HuntPlayer(/*direction*/));
    }

    public IEnumerator WaitForHidingPlayer()
    {
        bool detectedPlayer = false;
        float startTime = Time.time;
        while (Time.time < startTime + hidingWaitDuration)
        {
            if (!playerHiding)
            {
                detectedPlayer = true;
                break;
            }
            yield return null;
        }
        if (detectedPlayer)
        {
            MonsterReachedPlayerWrapper();
            //TODO: Maybe have player manage their hyperventilation?
        }
        else
        {
            //go back to moving between rooms like normal
            StartCoroutine(MoveBetweenRooms(GameHandler.roomManager.GetEnemyCurrentRoomIndex()));
        }
    }
    float hidingWaitDuration = 10;

    public void HurryToRoomOfScreamWrapper(){
        Debug.Log("Heard scream, hurrying over");
        //this will cause the enemy to hurry to the room where it heard the father orb scream
        StartCoroutine(HurryToRoomOfScream());
    }
    public IEnumerator HurryToRoomOfScream()
    {
        int screamSourceIndex = roomManager.GetPlayerCurrentRoomIndex();
        int currentRoomIndex = roomManager.GetEnemyCurrentRoomIndex();
        while (currentRoomIndex != screamSourceIndex)
        {
            //GO to the room the player is currently in until the player stops hiding and you eat them or the timer runs out
            currentRoomIndex = roomManager.GetEnemyCurrentRoomIndex() + starScreamDirection;

            if (currentRoomIndex == -1)
            {
                //if it equals the first index
                currentRoomIndex = roomManager.roomList.Count - 1;
            }
            else if (currentRoomIndex == roomManager.roomList.Count)
            {
                //if it equals the last index
                currentRoomIndex = 0;
            }
            transform.position = roomManager.roomList[currentRoomIndex].gameObject.transform.position;
            yield return new WaitForSeconds(2.0f);
        }
        if (screamSourceIndex == roomManager.GetEnemyCurrentRoomIndex())
        {
            if (playerHiding)
            {
                StartCoroutine(WaitForHidingPlayer());
            }

            else
            {
                MonsterReachedPlayerWrapper();
            }
        }
    }
    public IEnumerator HuntPlayer(/*int direction*/)
    {
        int currentRoomIndex = roomManager.GetEnemyCurrentRoomIndex();
        int playerRoomIndex = roomManager.GetPlayerCurrentRoomIndex();
        while (currentRoomIndex != playerRoomIndex)
        {
            //GO to the room the player is currently in until the player stops hiding and you eat them or the timer runs out
            playerRoomIndex = roomManager.GetPlayerCurrentRoomIndex();
            currentRoomIndex = roomManager.GetEnemyCurrentRoomIndex() + starScreamDirection;

            if (currentRoomIndex == -1)
            {
                //if it equals the first index
                currentRoomIndex = roomManager.roomList.Count - 1;
            }
            else if (currentRoomIndex == roomManager.roomList.Count)
            {
                //if it equals the last index
                currentRoomIndex = 0;
            }
            transform.position = roomManager.roomList[currentRoomIndex].gameObject.transform.position;
            yield return new WaitForSeconds(2.0f);
        }
        float startTime = Time.time;
        if (playerHiding)
        {
            StartCoroutine(WaitForHidingPlayer());
        }
        else
        {
            MonsterReachedPlayerWrapper();
        }
    }
    // Update is called once per frame

}
