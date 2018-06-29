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
		duration = 1.0f;
		strength = new Vector2(2, 2);
		vibrato = 10;
		randomness = 0.1f;

		initialAngle = -1;
		rotation = default(Vector3);
		smoothness = 0.1f;
		
	}
	void Awake(){
		Monster.ReadyToScream += StartShakeWithDelay;
		StarScream.ScreamHitPlayerCurrentRoom += ScreamShake;
		PromptPlayerHit.PlayerParried += ScreamShake;
	}


	void StartShakeWithDelay(){
		StartCoroutine(ShakeWithDelay());
	}
	public IEnumerator ShakeWithDelay(){
		yield return new WaitForSeconds(1.0f);
		ScreamShake();
	}

	void ScreamShake(int irrelevant){

			shaker.Shake(duration, strength, vibrato, randomness, initialAngle, rotation, smoothness, false);
	}

	void ScreamShake() {
			shaker.Shake(duration, strength, vibrato, randomness, initialAngle, rotation, smoothness, false);
	}

	void Update(){
		if(Input.GetKeyDown(KeyCode.Space)){
			ScreamShake();
		}
	}
}
