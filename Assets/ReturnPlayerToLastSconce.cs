using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Com.LuisPedroFonseca.ProCamera2D;
public class ReturnPlayerToLastSconce : MonoBehaviour {

	public static event Action ReturningToLastSconceWithPlayer;

	public static event Action ArrivedAtLastSconceWithPlayer;

	bool canReturn;
	ProCamera2D ourCamera;
	void CarryPlayerAlong(){

	}
	void Awake(){
		 ourCamera = Camera.main.GetComponent<ProCamera2D>();
	}

	void ReturningToLastSconceWithPlayerWrapper(){
		ourCamera.RemoveCameraTarget(GameHandler.playerGO.transform);
		ourCamera.AddCameraTarget(GameHandler.fatherOrbGO.transform);

	}

	void ArrivedAtLastSconce(){
		ourCamera.RemoveCameraTarget(GameHandler.fatherOrbGO.transform);
		ourCamera.AddCameraTarget(GameHandler.playerGO.transform);
	}

	void SetCanReturn(){
		canReturn =true;
	}

	void SetCANTReturn(){
		canReturn =false;
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(canReturn && Input.GetKeyDown(KeyCode.X)){
			//TODO: Connect everythign
			GameHandler.fatherOrb.ReturnToLastSconceWrapper() ;
		}
		
	}
}
