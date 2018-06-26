using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using DG.Tweening;
using MirzaBeig.ParticleSystems;
using Com.LuisPedroFonseca.ProCamera2D;
public class Room : MonoBehaviour
{

    AudioSource ourSource;

    AudioClip boom;

    [Header("Assigned In Inspector")]
    public ParticleSystems scentParticles;

    [Header("Other Attributes")]
    public List<Light> roomLights = new List<Light>();

    public Transform entranceA;

    public Transform entranceB;

    public event Action<Room> PlayerEnteredRoom;

    public event Action<Room> EnemyEnteredAdjacentRoom;
    public event Action<Room> EnemyExitedAdjacentRoom;

    public enum RoomType{
        Bedroom,
        Kitchen,
        EdgeRoomLeft,

        EdgeRoomRight,
        TallRoomLeft,
        TallRoomRight

    }

    public RoomType ourRoomType;
    public void EnemyExitedAdjacentRoomWrapper(Room room){
        if(playerLocation == PlayerStatus.InRoom){
            if(EnemyExitedAdjacentRoom != null){
                EnemyExitedAdjacentRoom(room);
            }
        }
    }

    public void EnemyEnteredAdjacentRoomWrapper(Room room)
    {
        //this event can trigger a dialogue for if the player
        //we only want to trigger this event if the player is in the room
        //we should have screenshake and audio cues when the enemy enters the room too
        if (playerLocation == PlayerStatus.InRoom)
        {
            if (EnemyEnteredAdjacentRoom != null)
            {
                EnemyEnteredAdjacentRoom(room);
            }
        }
    }

    void PlayBoom(){
        //TODO: Add something to calculate the distance from player
        ourSource.PlayOneShot(boom);
    }

    public static event Action<bool> RoomWithPlayerHit;

    void RoomWithPlayerHitWrapper(bool orbHeld)
    {
        //this is for if the room with the player is hit by the starscream, NOT the enemy itself. The enemy entering the room of a non-hiding player will result in immediate game-over
        if (RoomWithPlayerHit != null)
        {
            RoomWithPlayerHit(orbHeld);
        }
    }
    bool hasSconce;

    Sconce sconce;

    public List<Room> adjacentRooms = new List<Room>();
    List<HidingSpace> hidingSpaces;

    HidingSpace hidingSpace;
    Player player;

    ProCamera2D ourProCamera;

    bool CheckIfOrbInPlayerOrSconce()
    {
        bool orbInSconeOrCarried = false;
        if (sconce != null)
        {
            if (sconce.fillStatus == Sconce.Status.HoldingOrb || GameHandler.player.playerState == Player.PlayerState.CarryingOrb)
            {
                //both won't be true, but one must be true
                orbInSconeOrCarried = true;
            }
        }
        else if (sconce == null)
        {
            if (GameHandler.player.playerState == Player.PlayerState.CarryingOrb)
            {
                orbInSconeOrCarried = true;
            }
        }
        return orbInSconeOrCarried;
    }
    public enum PlayerStatus
    {
        InRoom,
        OutOfRoom
    }


    public PlayerStatus playerLocation;

    public void HandleScreamWrapper()
    {
        PlayShakeAnimation();
        StartCoroutine(FullFlash());
        if (playerLocation == PlayerStatus.InRoom)
        {
            bool orbHeld = CheckIfOrbInPlayerOrSconce();
            RoomWithPlayerHitWrapper(orbHeld);

            Debug.Log("FLASH HIT PLAYER ROOM");
            //a light must be in a sconce or in the player's hand  to jar the monster's senses
            //the player can move the thing around to try and find the sconces.
            //once all the sconces are found, the door to the next level opens
        }
    }

    public void HandleRumbleWrapper()
    {
        PlayShakeAnimation();
        if (playerLocation == PlayerStatus.InRoom)
        {
            Debug.Log("");
        }
    }
    void PlayShakeAnimation()
    {
        transform.DOShakePosition(1.0f, 0.5f, 1, 45, false, true);
        
        //  transform.DOShakePosition(1.0f, 0.5f, 1, 1, true, true);
        //todo: Have a shake animation played when the enemy's scouting call passes through this room
    }

    void PlayPunchAnimation()
    {

        transform.DOPunchPosition(transform.up, 0.5f, 0, 0.5f, true);
    }

    void Awake()
    {
        entranceA = transform.Find("EntranceA");
        //entrance A will always be on the left
        entranceB = transform.Find("EntranceB");
        //entrance B will always be on the right
        ourProCamera = Camera.main.GetComponent<ProCamera2D>();
        playerLocation = PlayerStatus.OutOfRoom;
        enemyLocation = EnemyStatus.OutOfRoom;
        roomLights = gameObject.GetComponentsInChildren<Light>().ToList();
        sconce = GetComponentInChildren<Sconce>(true);
        scentParticles = GetComponentInChildren<ParticleSystems>();
        foreach (Room adjacentRoom in adjacentRooms)
        {
            adjacentRoom.EnemyEntered += EnemyEnteredAdjacentRoomWrapper;
            adjacentRoom.EnemyExited += EnemyExitedAdjacentRoomWrapper;
        }
    }
    public enum EnemyStatus
    {
        InRoom,

        OutOfRoom
    }

    public void PlayScentParticleSystem()
    {
        scentParticles.transform.DOLocalMoveY(-2, 4.0f, false);
        scentParticles.Play();
    }

    public void StopScentParticleSystem()
    {
        scentParticles.transform.DOLocalMoveY(-6, 4.0f, false);
        //TODO: Attach this and the above method to the player entering/leaving the room aciton
        scentParticles.Stop();
    }

    public EnemyStatus enemyLocation;
    public void PlayerIntoRoom(Room room)
    {
        ourProCamera.AddCameraTarget(room.gameObject.transform);
        if (PlayerEnteredRoom != null)
        {
            PlayerEnteredRoom(room);
        }
        playerLocation = PlayerStatus.InRoom;
    }
    public event Action<Room> PlayerLeftRoom;

    public void PlayerExited(Room room)
    {
        ourProCamera.RemoveCameraTarget(room.gameObject.transform);
        PlayScentParticleSystem();
        if (PlayerLeftRoom != null)
        {
            PlayerLeftRoom(room);
        }
        playerLocation = PlayerStatus.OutOfRoom;
    }

    public event Action<Room> EnemyEntered;

    public event Action<Room> EnemyExited;
    public void EnemyEnteredRoom(Room room)
    {
        PlayShakeAnimation();
        if (EnemyEntered != null)
        {
            EnemyEntered(room);
        }
        enemyLocation = EnemyStatus.InRoom;
    }

    public void EnemyExitedRoom(Room room)
    {
        if (EnemyExited != null)
        {
            EnemyExited(room);
        }
        enemyLocation = EnemyStatus.OutOfRoom;

    }
    void LightsOn()
    {
        foreach (Light light in roomLights)
        {
            StartCoroutine(FlashLightOn(light));
        }
    }

    void LightsOff()
    {

        foreach (Light light in roomLights)
        {
            StartCoroutine(FlashLightOff(light));
        }
    }


    public IEnumerator FullFlash()
    {
        LightsOn();
        yield return new WaitForSeconds(1.0f);
        LightsOff();
    }

    float maxIntensity = 1.68f;
    float minIntensity = 0.75f;

    public IEnumerator FlashLightOn(Light light)
    {
        float flashTime = UnityEngine.Random.Range(0.25f, 1.0f);
        float flashMaxIntensity = UnityEngine.Random.Range(minIntensity, maxIntensity);
        float startTime = Time.time;
        float lerpTime = 0;
        while (Time.time < startTime + flashTime)
        {
            lerpTime += (Time.deltaTime / flashTime);
            light.intensity = Mathf.Lerp(0, flashMaxIntensity, lerpTime);
            yield return null;
        }
    }

    public IEnumerator FlashLightOff(Light light)
    {
        float flashTime = UnityEngine.Random.Range(0.25f, 1.0f);
        float flashMaxIntensity = UnityEngine.Random.Range(minIntensity, maxIntensity);
        float startTime = Time.time;
        float lerpTime = 0;
        while (Time.time < startTime + flashTime)
        {
            lerpTime += (Time.deltaTime / flashTime);
            light.intensity = Mathf.Lerp(light.intensity, 0, lerpTime);
            yield return null;
        }
        yield return null;
    }
    // Use this for initialization

}
