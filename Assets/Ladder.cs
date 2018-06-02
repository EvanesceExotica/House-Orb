using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour, iInteractable {

	Transform PointA;
	Transform PointB;

	public Transform destinationPoint;

	void Awake(){
		PointA = transform.Find("PointA");
		PointB = transform.Find("PointB");
	}

	public void OnHoverMe(Player player){
		Debug.Log("Press [E] to take ladder down");
	}	

	public void OnInteractWithMe(Player player){
		
		GameHandler.playerGO.transform.position = destinationPoint.position;

	}

	public void OnStopHoverMe(Player player){

	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
