using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.LuisPedroFonseca.ProCamera2D;

public class CameraShake2 : MonoBehaviour {

	private ProCamera2DShake shaker;
	// Use this for initialization
	void Start () {
		shaker = GetComponent<ProCamera2DShake>();
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space)){
			shaker.Shake(1.0f, new Vector2(-2, -2), 10, 0.1f, -1, default(Vector2), 0.1f, false);
		}
		
	}
}
