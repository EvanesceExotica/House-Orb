using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class HiddenSconce : MonoBehaviour
{
    public static event Action<FatherOrb.HeldStatuses> SconceRevealed;
    public event Action<Sconce, HiddenSconce> RevealedSconce;

    public void RevealedSconceWrapper(Sconce sconce, HiddenSconce hiddenSconce){
        if(RevealedSconce != null){
            RevealedSconce(sconce, hiddenSconce);
        }
    }
    public void SconceWasRevealed(FatherOrb.HeldStatuses orbStatus)
    {
        if (SconceRevealed != null)
        {
            SconceRevealed(orbStatus);
        }
    }

    float proximityRange = 0.5f;


    public Sconce sconceToReveal;

    void Awake()
    {
//        sconceToRevealGO = transform.GetChild(0).gameObject;// GetComponentInChildren<Sconce>().gameObject;
//        sconceToReveal = sconceToRevealGO.GetComponent<Sconce>();
 //       sconceToReveal.fillStatus = Sconce.Status.Hidden;
    }
    // Use this for initialization
    void Start()
    {
        proximityRange = 0.5f;

    }

    // Update is called once per frame
    void Update()
    {

    }

    bool orbOverlappingUs = false;
    void OnTriggerEnter2D(Collider2D hit)
    {
        if (hit.gameObject == GameHandler.fatherOrbGO)
        {
            if (GameHandler.fatherOrb.heldStatus == FatherOrb.HeldStatuses.Channeled)
            {
                orbOverlappingUs = true;
                StartCoroutine(BeginDeterminingProximity());
            }
        }
    }

    void OnTriggerExit2D(Collider2D hit)
    {
        if (hit.gameObject == GameHandler.fatherOrbGO)
        {
            orbOverlappingUs = false;
        }

    }

    IEnumerator BeginDeterminingProximity()
    {
        //we only want to begin calculating the distance once the transforms are somewhat overlapped
        while (orbOverlappingUs)
        {

            if (Vector2.Distance(GameHandler.fatherOrbGO.transform.position, transform.position) <= proximityRange)
            {
                //TODO: Code in the sconce appearing
                //FOUND IT
                RevealSconce();
                break;
            }
            yield return null;
        }
    }

    void RevealSconce()
    {
        //play some animation	
        Sconce revealedSconce = sconceToReveal.GetPooledInstance<Sconce>();
        revealedSconce.transform.position  = transform.position;
        //sconceToRevealGO.SetActive(true);
        //sconceToRevealGO.transform.parent = null;
        SconceWasRevealed(FatherOrb.HeldStatuses.InSconce);
        RevealedSconceWrapper(revealedSconce, this);
        revealedSconce.OrbPlacedInUs(revealedSconce);
        gameObject.SetActive(false);


    }
}
