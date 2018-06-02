using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using MirzaBeig.ParticleSystems;
public class Sconce : PooledObject, iInteractable
{

    public ParticleSystems orbOccupiedParticles;
    public ParticleSystems revealedParticles;
    public event Action<Sconce> Revealed;

    public void RevealedWrapper(Sconce sconce)
    {
        PlayRevealedParticles();
        if (Revealed != null)
        {
            Revealed(sconce);
        }
    }

    public event Action<Sconce> Extinguished;

    public void ExtinguishedWrapper(Sconce sconce)
    {
        StopOccupiedParticles();
        fillStatus = Status.Empty;
        if (Extinguished != null)
        {
            Extinguished(sconce);
        }
    }

    // Use this for initialization
    public static event Action<Sconce> OrbHeld;


    public void OrbPlacedInUs(Sconce sconce)
    {
        Debug.Log(sconce + " is holding orb now");
        PlayOccupiedParticles();
        fillStatus = Status.HoldingOrb;
        if (OrbHeld != null)
        {
            OrbHeld(sconce);
        }
    }
    public static event Action<Sconce> OrbRemoved;

    public void OrbRemovedFromUs(Sconce sconce)
    {
        fillStatus = Status.FreshlyLit;
        StartCoroutine(CountdownToEmpty());
        if (OrbRemoved != null)
        {
            OrbRemoved(sconce);
        }
    }

    float countdownStartTime;
    float countdownDuration = 10.0f;
    public IEnumerator CountdownToEmpty()
    {

        countdownStartTime = Time.time;
        while (Time.time < countdownStartTime + countdownDuration)
        {
            if (fillStatus == Status.HoldingOrb)
            {
                break;
            }
            yield return null;
        }
        Extinguished(this);
    }
    public enum Status
    {
        Hidden,
        Empty,

        FreshlyLit,
        HoldingOrb,
    }

    public Status fillStatus;



    public void OnInteractWithMe(Player player)
    {
        Debug.Log(gameObject.name + " is being interacted with");
        if (player.playerState == Player.PlayerState.CarryingOrb)
        {
            Debug.Log("Orb placed in " + gameObject.name);
            OrbPlacedInUs(this);
        }
        else if (fillStatus == Status.HoldingOrb && (player.playerState != Player.PlayerState.Hiding || player.playerState != Player.PlayerState.Burned || player.playerState != Player.PlayerState.CarryingOrb))
        {
            //if the player is able to receive the orb, isn't carrying it, isn't burned or hiding, the orb will be removed upon interaction
            OrbRemovedFromUs(this);
        }
    }

    public void OnHoverMe(Player player)
    {
        if (player.playerState == Player.PlayerState.CarryingOrb)
        {
            Debug.Log("Press E to place orb in sconce");
        }
        else if (fillStatus == Status.HoldingOrb && player.playerState == Player.PlayerState.NotCarryingOrb)
        {
            Debug.Log("Press E to take orb from sconce");
        }
    }

    public void OnStopHoverMe(Player player)
    {
        //todo: Remove Prompt
    }

    void Awake()
    {
        //TODO: Fix this so that it's an assignment later
        orbOccupiedParticles = transform.GetChild(1).GetComponent<ParticleSystems>();
        revealedParticles = transform.GetChild(0).GetComponent<ParticleSystems>();
        RevealedWrapper(this);
        if (fillStatus != Status.HoldingOrb)
        {
            StopOccupiedParticles();
        }
        //fillStatus = Status.Empty;
    }

    void PlayOccupiedParticles()
    {
        Debug.Log("Occupied particles are played");
        orbOccupiedParticles.Play();
    }

    void StopOccupiedParticles()
    {
        orbOccupiedParticles.Stop();
    }

    void PlayRevealedParticles()
    {
        revealedParticles.Play();

    }

    void StopRevealedParticles()
    {
        revealedParticles.Stop();
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            PlayOccupiedParticles();
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            StopOccupiedParticles();
        }

    }
}
