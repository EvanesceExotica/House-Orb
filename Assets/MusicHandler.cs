﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MusicHandler : MonoBehaviour
{

    public float fadeDuration = 1.0f;
    public AudioSource musicSource;
    public AudioClip normalMusic;

    public AudioClip memoryMusic;

    public AudioClip chaseMusic;

	public AudioClip channelingMusic;
    // Use this for initialization
    void Awake()
    {
        musicSource = GetComponent<AudioSource>();
        //SetNormalMood();
		ChangeClip(Mood.Normal);
    }

    // void SetNormalMood(){
    // 	ourMood = Mood.Normal;
    // 	musicSource.volume = 0;
    // 	musicSource.clip = normalMusic;
    // 	FadeInMusic();
    // }

    void ChangeClip(Mood moodToChangeTo)
    {
        FadeOutMusic();
        ourMood = moodToChangeTo;
        if (moodToChangeTo == Mood.Chased)
        {
            musicSource.clip = chaseMusic;
        }
        else if (moodToChangeTo == Mood.Normal)
        {
			musicSource.clip = normalMusic; 
        }
        else if (moodToChangeTo == Mood.Channeling)
        {
			musicSource.clip = channelingMusic;
        }
        FadeInMusic();
    }

    public enum Mood
    {

        Normal,
        Chased,
        Memory,

        Channeling,
        AllSconcesLit
    }

    public Mood ourMood;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void FadeOutMusic()
    {

        musicSource.DOFade(0f, fadeDuration);
    }

    void FadeInMusic()
    {
        musicSource.DOFade(1.0f, fadeDuration);

    }
}
