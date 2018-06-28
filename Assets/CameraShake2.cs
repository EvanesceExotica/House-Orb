using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.LuisPedroFonseca.ProCamera2D;

public class CameraShake2 : MonoBehaviour {

	private ProCamera2DShake shaker;

	public float duration;
	public Vector2 strength;
	public int vibrato;
	public float randomness;

	public float initialAngle; 

	public Vector3 rotation;

	public float smoothness;

	// Use this for initialization
	void Start () {
		shaker = GetComponent<ProCamera2DShake>();
		duration = 0.5f;
		strength = new Vector2(2, 2);
		vibrato = 10;
		randomness = 0.1f;

		initialAngle = -1;
		rotation = default(Vector3);
		smoothness = 0.1f;
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space)){
			shaker.Shake(duration, strength, vibrato, randomness, initialAngle, rotation, smoothness, false);
			//shaker.Shake(1.0f, new Vector2(-2, -2), 10, 1.0f, -1, default(Vector2), 0.5f, false);
		}
		
	}
}
