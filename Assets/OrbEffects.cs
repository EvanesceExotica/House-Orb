using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using MirzaBeig.ParticleSystems;
public class OrbEffects : MonoBehaviour
{

    [Header("Assign in Inspector")]

    AudioSource source;

    public AudioClip failClip;

    public AudioClip succeedClip;
    public ParticleSystems baseParticleSystem;
    public ParticleSystems FizzingParticleSystems; //ultima looping

    public ParticleSystems parryParticleSystem; //ultima one s hot

    public ParticleSystems closeToHiddenSystem; //

    public ParticleSystems burningSystem;//solar ; 

    public ParticleSystems inRoomWithHiddenSconce; // this one should trigger if you've seen a hint in a memory?

    public ParticleSystems hoveringOverMemory;

    public ParticleSystems nearHiddenSconce;

    public ParticleSystems hintParticleSystems;

    public ParticleSystems autoParryReadySystem;

    public ParticleSystems returnToSconceReadySystem;

    public ParticleSystems autoReflectChargeParticleSystem;

    public ParticleSystems prevSconeTeleportParticleSystem;

    public ParticleSystems refreshGivenParticleSystem;

    public ParticleSystems buffSpinParticleSystem;

    ParticleSystems mainCurrentPlayingSystem;

    public ParticleSystems failureSystem;
    Light ourLight;
    float defaultIntensity;
    float defaultColor;
    void Awake()
    {
        source = GetComponent<AudioSource>();
        ourLight = GetComponentInChildren<Light>();
        HiddenSconce.SconceRevealed += ReturnToStandardParticleEffect;
        ReturnPlayerToLastSconce.ArrivedAtLastSconceWithPlayer += ResetSystems;
        PromptPlayerHit.AutoRepelUsed += ReturnToStandardParticleEffect;

        Memory.AutoReflectGiven += StartAutoReflectChargeParticleSystem;
        Memory.RefreshGiven += StartRefreshParticleSystem;
        Memory.PrevSconceTeleportGiven += StartPrevSconceTeleportParticleSystem;
        Memory.HintGiven += StartHintParticleSystem;

        PromptPlayerHit.PlayerParried += Parry;
        PromptPlayerHit.AutoRepelUsed += Parry;

        FatherOrb.Fizzing += StartFizz;
        FatherOrb.RedHot += IncreaseFizzTempo;
        //FatherOrb.Overheated += StopFizz;
        FatherOrb.OrbRefreshed += StopFizz;
        FatherOrb.Dropped += StopFizz;

        PromptPlayerHit.PlayerFailed += PlayFailureEffect;
    }

    void StopAllButFizz()
    {

    }

    void ResetSystems(MonoBehaviour mono)
    {
        ReturnToStandardParticleEffect();
    }

    void ReturnToStandardParticleEffect()
    {
        if (mainCurrentPlayingSystem != null)
        {
            mainCurrentPlayingSystem.Stop();
            baseParticleSystem.Play();
            mainCurrentPlayingSystem = baseParticleSystem;
        }
    }


    void Shake()
    {
        transform.DOShakePosition(1.0f, 0.5f, 3, 90, false, true);
    }

    void PlayFailureEffect()
    {
        //TODO: We want maybe a red vingette effect 
        Shake();
        if (failureSystem != null)
        {
            failureSystem.Play();
        }
        source.PlayOneShot(failClip);

    }
    void GeneralBuff()
    {
        buffSpinParticleSystem.Play();
    }

    void StartHintParticleSystem(HiddenSconce irrelevant)
    {
        baseParticleSystem.Stop();
        hintParticleSystems.Play();
        mainCurrentPlayingSystem = hintParticleSystems;
    }

    void PlayInRoomWithSconceParticleSystem()
    {

    }

    void StartAutoReflectChargeParticleSystem()
    {
        if (autoReflectChargeParticleSystem != null)
        {
            baseParticleSystem.Stop();
            autoReflectChargeParticleSystem.Play();
            mainCurrentPlayingSystem = autoReflectChargeParticleSystem;
        }
    }

    void StartRefreshParticleSystem()
    {
        refreshGivenParticleSystem.Play();
        //TODO: Maybe all of the memories should refresh time
        //TODO: This one maybe can just be demonstrated by the turn.
    }

    void StartPrevSconceTeleportParticleSystem()
    {
        baseParticleSystem.Stop();
        prevSconeTeleportParticleSystem.Play();
        mainCurrentPlayingSystem = prevSconeTeleportParticleSystem;
    }
    void StartFizz()
    {
        FizzingParticleSystems.Play();

    }

    void StopFizz(UnityEngine.Object ourObject)
    {
        FizzingParticleSystems.Stop();
    }

    void IncreaseFizzTempo()
    {
        FizzingParticleSystems.SetPlaybackSpeed(2.0f);
    }
    void ChangeLightIntensity(float intensity, float duration)
    {
        ourLight.DOIntensity(intensity, duration);
    }

    void ChangeLightColor(Color color, float duration)
    {
        ourLight.DOColor(color, duration);
    }

    void Parry()
    {
        parryParticleSystem.Play();
    }

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            StartFizz();
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            IncreaseFizzTempo();
        }

    }
}
