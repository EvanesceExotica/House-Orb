﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using MirzaBeig.ParticleSystems;
public class OrbEffects : MonoBehaviour {

[Header("Assign in Inspector")]
	public ParticleSystems FizzingParticleSystems; //ultima looping

	public ParticleSystems parryParticleSystem; //ultima one s hot

	public ParticleSystems closeToHiddenSystem; //

	public ParticleSystems burningSystem ;//solar ; 

	Light ourLight;
	float defaultIntensity;
	float defaultColor;
	void Awake(){
		ourLight = GetComponentInChildren<Light>();
		PromptPlayerHit.PlayerParried += Parry;
	}

	void ChangeLightIntensity(float intensity, float duration){
		ourLight.DOIntensity(intensity, duration);
	}

	void ChangeLightColor(Color color, float duration){
		ourLight.DOColor(color, duration);
	}

	void Parry(){
		parryParticleSystem.Play();
	}

	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.U)){
			parryParticleSystem.Play();
		}
		
	}
}
