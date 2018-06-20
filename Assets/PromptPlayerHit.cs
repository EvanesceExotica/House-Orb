using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;
using System;
using MirzaBeig.ParticleSystems;
public class PromptPlayerHit : MonoBehaviour
{
    public static event Action PlayerParried;

   public static event Action AutoRepelUsed; 
    public static event Action PlayerFailed;


    Image promptTimeImage;
   TextMeshProUGUI textComponent; 
        CanvasGroup ourCanvasGroup;

    public float hitDurationWindow = 1.0f;
    float startTime;

    void PlayerHitByStarScream()
    {

    }
    bool canAutoRepel;
    void AutoRepelActivated(){
        canAutoRepel = true;
    }

    void AutoRepelUsedWrapper(){

        canAutoRepel = false;
        if(AutoRepelUsed != null){
            AutoRepelUsed();
        }
    }
    RoomManager roomManager;
    void Awake()
    {
        promptTimeImage = GetComponentInChildren<Image>();
        ourCanvasGroup = GetComponent<CanvasGroup>();
        textComponent = GetComponentInChildren<TextMeshProUGUI>();
        Room.RoomWithPlayerHit += this.PromptPlayerHitWrapper;
        AutoRepel.AutoRepelTriggered+= AutoRepelActivated;

    }

    public ParticleSystems topSystem;
    public ParticleSystems leftSystem;
    public ParticleSystems downSystem;
    public ParticleSystems rightSystem;

    void Update(){

    }

    void PromptPlayerHitWrapper(bool orbHeld)
    {
        if (orbHeld)
        {
            FadeInPrompt();
            StartCoroutine(PromptPlayerHitCoroutine());
        }
		else{
			PlayerMissedOrFailed();
		}

    }

    void FadeInPrompt(){
        Debug.Log("Fade in prompted");
        ourCanvasGroup.DOFade(1, 0.5f);
    }

    void FadeOutPrompt(){

        ourCanvasGroup.DOFade(0, 0.5f);
    }

    void PlayerMissedOrFailed()
    {
        if(PlayerFailed != null){
            PlayerFailed();
        }
		Debug.Log("Player MISSED OR FAILED");
    }

	void PlayerParriedScream(){
		Debug.Log("SCREAM PARRIED");
        if(PlayerParried != null){
            PlayerParried();
        }

	}
    void AddOrbAsTarget(){
        GameHandler.proCamera.RemoveCameraTarget(GameHandler.roomManager.GetPlayerCurrentRoom().gameObject.transform);
        GameHandler.proCamera.AddCameraTarget(GameHandler.fatherOrbGO.transform);
    }

    void RemoveOrbAsTarget(){
        GameHandler.proCamera.RemoveCameraTarget(GameHandler.fatherOrbGO.transform);
        GameHandler.proCamera.AddCameraTarget(GameHandler.roomManager.GetPlayerCurrentRoom().gameObject.transform);
    }

    void ZoomIn(){
        //GameHandler.proCamera.Zoom(10, 0.5, );
    }

    void ZoomOut(){

    }

    enum Sides{
        Up,
        Down,
        Left,
        Right
    }

    KeyCode ourKeyCode;

   KeyCode PickOrbSide(){
       KeyCode potentialKeyCode = KeyCode.E;
       int random = UnityEngine.Random.Range(0, 4);
       if(random == 0){
           //UP
          potentialKeyCode = KeyCode.I; 
          PlaySystem(topSystem);
       }
       else if(random == 1){
           potentialKeyCode = KeyCode.J;
           PlaySystem(leftSystem);

       }
       else if(random == 2){
           potentialKeyCode = KeyCode.K;
           PlaySystem(downSystem);

       }
       else if(random == 3){
           potentialKeyCode = KeyCode.L;
           PlaySystem(rightSystem);

       }
       return potentialKeyCode;
    }

    void PlaySystem(ParticleSystems chosenSystem){
        chosenSystem.Play();
    }

    void StopSystem(ParticleSystems chosenSystem){
        chosenSystem.Stop();
    }


    bool hitSuccess = false;

    public IEnumerator PromptPlayerHitCoroutine()
    {
        if(canAutoRepel){
            PlayerParriedScream();
            yield break;
        }
        float startTime = Time.time;
		Debug.Log("Hit E");
        //Make the keys random?
        while (Time.time < startTime + hitDurationWindow)
        {
             promptTimeImage.fillAmount -= Time.deltaTime / hitDurationWindow;
            //TODO: this key will change and be random
            if (Input.GetKeyDown(KeyCode.E))
            {
                textComponent.color = Color.green;
                hitSuccess = true;
                break;
            }
            yield return null;
        }
        FadeOutPrompt();
        if (hitSuccess)
        {
			PlayerParriedScream();
            Debug.Log("Stunned enemy!");
            //TODO: Insert good stuff, stunning the enemy here
        }
        else
        {
            PlayerMissedOrFailed();
            textComponent.color = Color.red;
            Debug.Log("we're blinded oh no");
            //TODO: Insert bad stuff, player blinded here.
        }
    }
    public IEnumerator PromptPlayerJumpCoroutine()
    {
        float startTime = Time.time;
        //Make the keys random?
        while (Time.time < startTime + hitDurationWindow)
        {
            if (Input.GetButtonDown("Jump"))
            {
                hitSuccess = true;
                yield break;
            }
            yield return null;
        }
        if (hitSuccess)
        {
            //PlayerParried();
            Debug.Log("Player jumped and didn't drop orb!");
        }

        else
        {
			//PlayerMissedOrFailed();
            Debug.Log("Player is stunned and if was carrying orb, dropped it ");
        }
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
   
}
