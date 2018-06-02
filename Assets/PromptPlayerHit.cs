using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;
using System;
public class PromptPlayerHit : MonoBehaviour
{
    public static event Action PlayerParried;

    
    public static event Action PlayerFailed;


    Image promptTimeImage;
   TextMeshProUGUI textComponent; 
        CanvasGroup ourCanvasGroup;

    public float hitDurationWindow = 1.0f;
    float startTime;

    void PlayerHitByStarScream()
    {

    }
    void Awake()
    {
        promptTimeImage = GetComponentInChildren<Image>();
        ourCanvasGroup = GetComponent<CanvasGroup>();
        textComponent = GetComponentInChildren<TextMeshProUGUI>();
        Room.RoomWithPlayerHit += this.PromptPlayerHitWrapper;

    }

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

    bool hitSuccess = false;

    public IEnumerator PromptPlayerHitCoroutine()
    {
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
