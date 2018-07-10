using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.LuisPedroFonseca.ProCamera2D;
public class GameHandler : MonoBehaviour
{

    public static ScreamFollowObject screamFollowObject;
    public static RoomManager roomManager;
    public static LayerMask defaultPlayerLayer;
    public static Player player;
    public static GameObject playerGO;

    public static FatherOrb fatherOrb;
    public static GameObject fatherOrbGO;

    public static Monster monster;
    public static GameObject monsterGO;

    public static ProCamera2D proCamera;

    public static Camera mainCamera;

    public static Transform fatherOrbHoldTransform;

	public static OrbController orbController;

    public static OrbEffects orbEffects;
    public static Transform bubbleLineStartTransform;

    public static AudioSource screamSoundObjectSource; 
    void Awake()
    {
        screamSoundObjectSource = GameObject.Find("ScreamSound").GetComponent<AudioSource>();
        screamFollowObject = GameObject.Find("ScreamFollowObject").GetComponent<ScreamFollowObject>();
        fatherOrbHoldTransform = GameObject.Find("FatherOrbPos").transform;
        bubbleLineStartTransform = GameObject.Find("LineStartPosition").transform;
        proCamera = Camera.main.GetComponent<ProCamera2D>();
        mainCamera = Camera.main;
        roomManager = GameObject.Find("Managers").GetComponent<RoomManager>();
        playerGO = GameObject.Find("Player");
        player = playerGO.GetComponent<Player>();
        defaultPlayerLayer = playerGO.layer;
        fatherOrbGO = GameObject.Find("FatherOrb");
        fatherOrb = fatherOrbGO.GetComponent<FatherOrb>();
        monsterGO = GameObject.Find("Monster");
		orbController = fatherOrbGO.GetComponent<OrbController>();
        orbEffects = fatherOrbGO.GetComponent<OrbEffects>();
        if (monsterGO != null)
        {
            monster = monsterGO.GetComponent<Monster>();
        }
        Monster.MonsterReachedPlayer += GameOver;
    }
    // Use this for initialization

    void GameOver()
    {
		Debug.Log("GAme over :O");
    }

}
