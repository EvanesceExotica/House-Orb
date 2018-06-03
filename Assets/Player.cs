using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using MirzaBeig.ParticleSystems;
public class Player : MonoBehaviour {

	public ParticleSystems blinded; //serum2	
	public PlayerRender playerRenderer;
	public PlayerMovement movement;
	// Use this for initialization
	public static event Action PickUpOrb;

	public static event Action Burning;

	public static event Action Blinded;

	public static event Action NoLongerBurning;

	public static event Action Hiding;

	public static event Action NoLongerHiding;

	void PickingUpOrb(){
		if(PickUpOrb != null){
			PickUpOrb();
		}
	}
	public enum PlayerState{
		NotCarryingOrb,
		CarryingOrb,
		Hiding,
		Burned
	}

	public PlayerState playerState;
	
	void SetCarryingOrb(){
		playerState = PlayerState.CarryingOrb;
	}

	void SetCarryingOrb(GameObject source){
		playerState = PlayerState.CarryingOrb;
	}

	void SetNotCarryingOrb(GameObject source){
		playerState = PlayerState.NotCarryingOrb;
	}

	void SetNotCarryingOrb(){
		playerState = PlayerState.NotCarryingOrb;
	}
	void SetBurned(){

		playerState = PlayerState.Burned;
		if(Burning != null){
			Burning();
		}
		StartCoroutine(EffectCountDown(4.0f, NegativeEffects.Burn));
	}

	void SetBlinded(){
		if(Blinded != null){
			Blinded();
		}
		StartCoroutine(EffectCountDown(8.0f, NegativeEffects.Blind));
	}
	void SetHiding(GameObject source){
		playerState = PlayerState.Hiding;

	}

	public float burnDuration;
	public float burnStartTime;
	public IEnumerator EffectCountDown(float duration, NegativeEffects typeOfEffect){
		float startTime = Time.time;

		if(typeOfEffect == NegativeEffects.Burn){
			ApplyBurnEffects();
		}
		else if (typeOfEffect == NegativeEffects.Blind){
			ApplyBlindEffects();
		}

		while(Time.time < startTime + duration){

			yield return null;
		}

		if(typeOfEffect == NegativeEffects.Burn){
			ReverseBurnEffects();
		}
		else if(typeOfEffect == NegativeEffects.Blind){
			ReverseBlindEffects();
		}
	}

	public enum NegativeEffects{
		Burn,
		Blind
	}

	void ApplyBurnEffects(){
		Debug.Log("BURNED");
		///one effect of the burn is to half the speed
		movement.maxSpeed *= 0.5f;
	}

	void ReverseBurnEffects(){
		Debug.Log("No longer burned");
		movement.maxSpeed /= 0.5f;
	}

	void ApplyBlindEffects(){
		Debug.Log("BLINDED");
		movement.maxSpeed *= 0.5f;
	}

	void ReverseBlindEffects(){
		Debug.Log("No longer blinded");
		movement.maxSpeed /= 0.5f;
	}

	void SetNotBurned(){
		playerState = PlayerState.NotCarryingOrb;
		if(NoLongerBurning != null){
			NoLongerBurning();
		}

	}
	void Awake(){
		burnDuration = 5.0f;
		playerState = PlayerState.NotCarryingOrb;
		FatherOrb.PickedUp += this.SetCarryingOrb;
		FatherOrb.Dropped += this.SetNotCarryingOrb;
		playerRenderer = GetComponent<PlayerRender>();
		movement = GetComponent<PlayerMovement>();
		OrbController.ChannelingOrb += SetNotCarryingOrb;
		OrbController.StoppedChannelingOrb += SetCarryingOrb;
		HidingSpace.PlayerHiding += SetHiding;
		//TODO -- separate the hiding and the orb-carrying. I want the light to cause the player to be unhidden.
		HidingSpace.PlayerNoLongerHiding += SetNotCarryingOrb;
		PromptPlayerHit.PlayerFailed += SetBlinded;
	}

}
