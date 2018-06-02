using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRender : MonoBehaviour {

	//SpriteRenderer spriteRenderer;
	MeshRenderer meshRenderer;

	void Awake(){
		meshRenderer = GetComponent<MeshRenderer>();
		//spriteRenderer = GetComponent<SpriteRenderer>();
		HidingSpace.PlayerHiding += TurnOffPlayerRenderer;
		HidingSpace.PlayerNoLongerHiding += TurnOnPlayerRenderer;
	}
	public void TurnOffPlayerRenderer(GameObject go){
		meshRenderer.enabled = false;
	}

	public void TurnOnPlayerRenderer(GameObject go){
		meshRenderer.enabled = true;
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
