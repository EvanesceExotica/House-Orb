﻿using System.Collections;
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

    public static event Action<MonoBehaviour> WaitingForScreamPrompt;

    void WaitingForScreamPromptWrapper()
    {
        if (WaitingForScreamPrompt != null)
        {
            WaitingForScreamPrompt(this);
        }
    }
    public static event Action<MonoBehaviour> ScreamPromptPassed;

    void ScreamPromptPassedWrapper()
    {

        if (ScreamPromptPassed != null)
        {
            ScreamPromptPassed(this);
        }

    }
    Image promptTimeImage;
    TextMeshProUGUI textComponent;
    CanvasGroup ourCanvasGroup;

    public float hitDurationWindow = 1.0f;
    float startTime;

    void PlayerHitByStarScream()
    {

    }
    bool canAutoRepel;
    void AutoRepelActivated()
    {
        Debug.Log("We were set to auto-repel");
        canAutoRepel = true;
    }

    void AutoRepelUsedWrapper()
    {

        canAutoRepel = false;
        if (AutoRepelUsed != null)
        {
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
        AutoRepel.AutoRepelTriggered += AutoRepelActivated;

        topCanvasGroup.alpha = 0;
        leftCanvasGroup.alpha = 0;
        downCanvasGroup.alpha = 0;
        rightCanvasGroup.alpha = 0;

        topImage = topCanvasGroup.GetComponentInChildren<Image>();
        leftImage = leftCanvasGroup.GetComponentInChildren<Image>();
        downImage = downCanvasGroup.GetComponentInChildren<Image>();
        rightImage = rightCanvasGroup.GetComponentInChildren<Image>();

        StarScream.ScreamHitRoomAdjacent += FocusOnOrb;

    }

    [Header("Top")]
    public ParticleSystems topSystem;

    public CanvasGroup topCanvasGroup;

    public Image topImage;

    [Header("Left")]
    public ParticleSystems leftSystem;

    public CanvasGroup leftCanvasGroup;

    public Image leftImage;
    [Header("Down")]
    public ParticleSystems downSystem;

    public CanvasGroup downCanvasGroup;

    public Image downImage;

    [Header("Right")]
    public ParticleSystems rightSystem;


    public CanvasGroup rightCanvasGroup;
    public Image rightImage;

    void Update()
    {

    }

    void PromptPlayerHitWrapper(bool orbHeld)
    {
        if (orbHeld)
        {
            StartCoroutine(PromptPlayerHitCoroutine());
        }
        else
        {
            PlayerMissedOrFailed();
        }

    }

    void FadeInPrompt()
    {
        Debug.Log("Fade in prompted");
        ourCanvasGroup.DOFade(1, 0.5f);
    }

    void FadeOutPrompt()
    {

        ourCanvasGroup.DOFade(0, 0.5f);
    }

    void PlayerMissedOrFailed()
    {
        if (PlayerFailed != null)
        {
            PlayerFailed();
        }
        Debug.Log("Player MISSED OR FAILED");
    }

    void PlayerParriedScream()
    {
        Debug.Log("SCREAM PARRIED");
        if (PlayerParried != null)
        {
            PlayerParried();
        }

    }

    void FocusOnOrb()
    {
        i += 1;
        Debug.Log("This was triggered " + i + " times");
        AddOrbAsTarget();
        ZoomIn();
    }

    void FocusOnPlayer()
    {
        RemoveOrbAsTarget();
        ZoomOut();
    }
    void AddOrbAsTarget()
    {
        GameHandler.proCamera.RemoveCameraTarget(GameHandler.roomManager.GetPlayerCurrentRoom().gameObject.transform);
        GameHandler.proCamera.AddCameraTarget(GameHandler.fatherOrbGO.transform, 1, 1, 0.5f, Vector2.zero);
    }

    void RemoveOrbAsTarget()
    {
        GameHandler.proCamera.RemoveCameraTarget(GameHandler.fatherOrbGO.transform);
        GameHandler.proCamera.AddCameraTarget(GameHandler.roomManager.GetPlayerCurrentRoom().gameObject.transform);
    }

    void ZoomIn()
    {
        GameHandler.proCamera.Zoom(-1, 0.5f, Com.LuisPedroFonseca.ProCamera2D.EaseType.EaseInOut);
    }

    void ZoomOut()
    {

        GameHandler.proCamera.Zoom(1, 0.5f, Com.LuisPedroFonseca.ProCamera2D.EaseType.EaseInOut);
    }

    enum Sides
    {
        Up,
        Down,
        Left,
        Right
    }

    KeyCode ourKeyCode;

    ParticleSystems chosenSystem;
    KeyCode PickOrbSide()
    {
        KeyCode potentialKeyCode = KeyCode.E;
        int random = UnityEngine.Random.Range(0, 4);
        if (random == 0)
        {
            //UP
            potentialKeyCode = KeyCode.I;
            chosenSystem = topSystem;
            ourCanvasGroup = topCanvasGroup;
            promptTimeImage = topImage;
            PlaySystem();
        }
        else if (random == 1)
        {
            potentialKeyCode = KeyCode.J;
            ourCanvasGroup = leftCanvasGroup;
            promptTimeImage = leftImage;
            chosenSystem = leftSystem;
            PlaySystem();

        }
        else if (random == 2)
        {
            potentialKeyCode = KeyCode.K;
            ourCanvasGroup = downCanvasGroup;
            promptTimeImage = downImage;
            chosenSystem = downSystem;
            PlaySystem();

        }
        else if (random == 3)
        {
            potentialKeyCode = KeyCode.L;
            ourCanvasGroup = rightCanvasGroup;
            promptTimeImage = rightImage;
            chosenSystem = rightSystem;
            PlaySystem();

        }
        Debug.Log("Chose potential key " + potentialKeyCode.ToString());
        return potentialKeyCode;
    }

    void PlaySystem()
    {
        chosenSystem.SetPlaybackSpeed(2.0f);
        chosenSystem.Play();
    }

    void StopSystem()
    {
        chosenSystem.Stop();
    }


    bool waitingForPrompt;
    KeyCode lastHitKey;
    void OnGui()
    {
        if (waitingForPrompt)
        {
            if (Input.anyKeyDown)
            {
                lastHitKey = Event.current.keyCode;
            }
        }
    }
    bool hitSuccess = false;

    public IEnumerator PromptPlayerHitCoroutine()
    {
        if (canAutoRepel)
        {
            PlayerParriedScream();
            AutoRepelUsedWrapper();
            yield break;
        }
        float startTime = Time.time;

        //grab the side of the orb -- up down left or right
        KeyCode ourKeyCode = PickOrbSide();
        FadeInPrompt();
        waitingForPrompt = true;
        WaitingForScreamPromptWrapper();
        while (Time.time < startTime + hitDurationWindow)
        {
            promptTimeImage.fillAmount -= Time.deltaTime / hitDurationWindow;
            //TODO: this key will change and be random
            if (Input.anyKeyDown)
            {
                if (Input.GetKeyDown(ourKeyCode))
                {
                    textComponent.color = Color.green;
                    hitSuccess = true;
                    break;
                }
                if (lastHitKey != ourKeyCode)
                {
                    promptTimeImage.color = Color.red;
                    break;
                }
                // if (!Input.GetKeyDown(ourKeyCode))
                // {
                //     textComponent.color = Color.red;
                //     break;
                // }
            }
            yield return null;
        }

        FocusOnPlayer();
        waitingForPrompt = false;
        StopSystem();
        //TODO: play some cowering animation by the player here
        ScreamPromptPassedWrapper();
        FadeOutPrompt();
        if (hitSuccess)
        {
            //TODO: Add some particle effect that shows a sheild or something exploding forth from the parried side
            PlayerParriedScream();
            Debug.Log("Stunned enemy!");
            //TODO: Insert good stuff, stunning the enemy here
        }
        else
        {
            PlayerMissedOrFailed();
            promptTimeImage.color = Color.red;
            //textComponent.color = Color.red;
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
    int i = 0;
    void Start()
    {

    }

    // Update is called once per frame

}
