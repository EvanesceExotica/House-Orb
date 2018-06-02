using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour {


	public static Player player;
	public static GameObject playerGO;

	public static FatherOrb fatherOrb;
	public static GameObject fatherOrbGO;

	public static Monster monster;
	public static GameObject monsterGO;

	void Awake(){
		playerGO = GameObject.Find("Player");
		player = playerGO.GetComponent<Player>();
		fatherOrbGO = GameObject.Find("FatherOrb");
		fatherOrb = fatherOrbGO.GetComponent<FatherOrb>();
		monsterGO = GameObject.Find("Monster");
		monster = monsterGO.GetComponent<Monster>();
		Monster.MonsterReachedPlayer += GameOver;
	}
	// Use this for initialization

	void GameOver(){

	}

}
