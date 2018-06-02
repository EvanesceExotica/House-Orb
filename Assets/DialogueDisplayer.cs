using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueDisplayer : MonoBehaviour {

	// Use this for initialization

	void ReactToMemoryHover(){

	}
	void Awake(){
		Memory.HoveringOverMemoryObject += ReactToMemoryHover;
	}
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
