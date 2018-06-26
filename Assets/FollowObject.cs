using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MirzaBeig.ParticleSystems;
public class FollowObject : MonoBehaviour
{
	ParticleSystems ourSystem;
	Transform objectWereFollowing;

    void Awake(){
        ScreamFollowObject.ScreamObjectMoving += SetObjectWereFollowing;
    }  
	void PlayOurSystem(){
        ourSystem.Play();
	}

	void StopOurSystem(){
        ourSystem.Stop();
	}
	
    public float speed = 8.0f;
    public float distanceFromCamera = 5.0f;

    public bool ignoreTimeScale;
	void SetObjectWereFollowing(Transform target){

		objectWereFollowing = target;

        if(!ourSystem.IsPlaying()){
            PlayOurSystem();
        }
	}
    void Update()
    {
        float deltaTime = !ignoreTimeScale ? Time.deltaTime : Time.unscaledDeltaTime;
        Vector3 position = Vector3.Lerp(transform.position, objectWereFollowing.position, 1.0f - Mathf.Exp(-speed * deltaTime));

        transform.position = position;
    }

}
