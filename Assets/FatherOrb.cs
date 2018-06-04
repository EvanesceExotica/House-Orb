using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class FatherOrb : MonoBehaviour//, iInteractable
{

    GameObject player;
    [SerializeField] Sconce previousSconce;
    [SerializeField] Sconce currentSconce;
    public static event Action PickedUp;

    public static event Action Dropped;

    public static event Action ReturnedToPreviousScone;

    void ReturnedToPreviousSconeWrapper(){
        
    }

    void PlayerDroppedOrb()
    {
        heldStatus = HeldStatuses.Travelling;
        if (Dropped != null)
        {
            Dropped();
        }
    }
    public static event Action Fizzing;

    void OrbFizzing()
    {
        if (Fizzing != null)
        {
            Fizzing();
        }
    }
    public static event Action RedHot;

    void OrbRedHot()
    {
        if (RedHot != null)
        {
            RedHot();
        }
    }
    public static event Action Overheated;


    void OrbOverheated()
    {
        PlayerDroppedOrb();
        StartCoroutine(ReturnToLastSconce());
        if (Overheated != null)
        {
            Overheated();
        }
        instabilityStatus = InstabilityStatus.NotPickedUp;
    }



    public void OnInteractWithMe(Player player)
    {
        if (heldStatus == HeldStatuses.InSconce && (player.playerState != Player.PlayerState.Burned || player.playerState != Player.PlayerState.Hiding))
        {
            // PickedUpByPlayer();
        }
    }
    public void OnHoverMe(Player player)
    {
        //TODO: Add a prompt
        if (heldStatus == HeldStatuses.InSconce && player.playerState == Player.PlayerState.NotCarryingOrb)
        {
            // Debug.Log("Press E to pick up orb");
        }
    }
    public void OnStopHoverMe(Player player)
    {
        //TODO: Remove prompt
    }
    void Awake()
    {
        player = GameObject.FindWithTag("Player");
        durationHeld = 22.0f;
        durationBeforeFizzing = 10.0f;
        durationBeforeRedHot = 17.0f;
        durationAtBurn = 22.0f;
        Sconce.OrbHeld += this.EnteredSconce;
        Sconce.OrbRemoved += this.PickedUpByPlayer;
        if (transform.parent != null)
        {
            Sconce sconce = transform.parent.GetComponent<Sconce>();
            sconce.OrbPlacedInUs(sconce);
        }
        //SetInSconce(transform.parent.gameObject);
    }

    void SetInSconce(Sconce sconce)
    {
        //sconce.GetComponent<Sconce>().OrbPlacedInUs();
        heldStatus = HeldStatuses.InSconce;
        currentSconce = sconce;
        inSconce = true;
        transform.parent = currentSconce.transform;
        instabilityStatus = InstabilityStatus.NotPickedUp;
    }
    public float durationHeld;
    public float heldStartTime;
    public float elapsedTime;
    public enum HeldStatuses
    {
        InSconce,
        Travelling,
        Channeled,
        Carried
    }
    public HeldStatuses heldStatus;

    float durationBeforeFizzing;
    float durationBeforeRedHot;

    float durationAtBurn;
    public enum InstabilityStatus
    {
        NotPickedUp,
        FreshPickedUp,
        Fizzing,

        RedHot,
        Screaming

    }

    public InstabilityStatus instabilityStatus;
    bool inSconce;
    public void EnteredSconce(Sconce sconce)
    {
        PlayerDroppedOrb();
        SetInSconce(sconce);
        StartCoroutine(MoveUs(transform.position, sconce.transform.position));
        transform.parent = sconce.transform;
    }

  

    public void SetOrbBeingChanneled()
    {
        //channeling should add a small time boost
        //a small animation should play around the orb to show time's been added to it 
        changeElapsedTime(5.0f);
        heldStatus = HeldStatuses.Channeled;
    }

    public void PickedUpByPlayer(Sconce sconce)
    {
        heldStatus = HeldStatuses.Carried;
        inSconce = false;
        previousSconce = currentSconce;
        currentSconce = null;
        instabilityStatus = InstabilityStatus.FreshPickedUp;
        StartCoroutine(MoveUs(transform.position, player.transform.position));
        transform.parent = player.transform;
        if (PickedUp != null)
        {
            PickedUp();
        }
        StartCoroutine(BeingCarried());
    }

    public void changeElapsedTime(float amount)
    {
        if (carriedOrChanneled)
        {
            //if the elapsed time of the orb being carried or channeled is ticking down, add or remove an amount from it
            elapsedTime += amount;
        }
    }

    bool carriedOrChanneled;
    public IEnumerator BeingCarried()
    {
        carriedOrChanneled = true;
        heldStartTime = Time.time;

        while (elapsedTime < durationHeld)
        {
            if (inSconce)
            {
                elapsedTime = 0;
                yield break;
            }
            if(cancelCarryCoroutine){
                elapsedTime = 0;
                yield break;
            }
            if (elapsedTime >= durationBeforeFizzing && instabilityStatus != InstabilityStatus.Fizzing)
            {
                instabilityStatus = InstabilityStatus.Fizzing;
                OrbFizzing();
            }
            if (elapsedTime >= durationBeforeRedHot && instabilityStatus != InstabilityStatus.RedHot)
            {
                instabilityStatus = InstabilityStatus.RedHot;
                OrbRedHot();

            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        elapsedTime = 0;
        carriedOrChanneled = false;
        if (heldStatus == HeldStatuses.Carried)
        {
            //if the player was carrying the orb when time ran out
            OrbOverheated();
        }
        else if (heldStatus == HeldStatuses.Channeled)
        {
            //if the player was channeling the orb when time ran out
            OrbScreamWrapper();
        }

    }


    public static event Action OrbScream;

    void OrbScreamWrapper()
    {
        if (OrbScream != null)
        {
            OrbScream();
        }
    }

    public void MoveUsWrapper(Vector2 startingPosition, Vector2 destination)
    {
        StartCoroutine(MoveUs(startingPosition, destination));
    }

    public IEnumerator MoveUs(Vector2 startingPosition, Vector2 destination)
    {
        while (Vector2.Distance(transform.position, destination) > 0.1f)
        {
            //TODO: uncomment the below and fix it
            transform.position = Vector2.MoveTowards(transform.position, destination, 5 * Time.deltaTime);
            yield return null;
        }
    }

    //TODO: Make sure this only applies to the automatic cancel travel vvvv
    bool cancelCarryCoroutine;
    public void ReturnToLastSconceWrapper(){
        cancelCarryCoroutine = true;
       StartCoroutine(ReturnToLastSconce()) ;
    }

    public IEnumerator ReturnToLastSconce()
    {
        yield return StartCoroutine(MoveUs(transform.position, previousSconce.transform.position));
        SetInSconce(previousSconce);
       // currentSconce = previousSconce;
        //transform.parent = currentSconce.gameObject.transform;

    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
