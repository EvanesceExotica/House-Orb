using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
public class Memory : MonoBehaviour, iInteractable
{

    public static Action HoveringOverMemoryObject;

    public static Action StoppedHoveringOverMemoryObject;
    void StoppedHoveringOverMemoryObjectWrapper()
    {
        if (StoppedHoveringOverMemoryObject != null)
        {
            StoppedHoveringOverMemoryObject();
        }
    }
    void HoveringOverMemoryObjectWrapper()
    {
        if (HoveringOverMemoryObject != null)
        {
            HoveringOverMemoryObject();
        }
    }
    // Use this for initialization
    public enum BuffGiven
    {
        AutoReflect,
        Hint,

        TimeDouble,

        PrevSconceTeleport
    }
    //Add a type of buff
    public void OnHoverMe(Player player)
    {
        //TODO: Play some sparkly effect for the object, make orb glow brighter
        Debug.Log("Press [E] to show object to house-father");
        HoveringOverMemoryObjectWrapper();
    }

    public void OnStopHoverMe(Player player)
    {
		StoppedHoveringOverMemoryObjectWrapper();
    }

    public void OnInteractWithMe(Player player)
    {
		if(givenBuff == BuffGiven.Hint){

		}
		else if(givenBuff == BuffGiven.TimeDouble){


		}
		else if(givenBuff == BuffGiven.AutoReflect){

		}
		else if(givenBuff == BuffGiven.PrevSconceTeleport){

		}
    }
    public BuffGiven givenBuff;

    void MoveToMemory(){
       GameHandler.fatherOrb.MoveUsWrapper(GameHandler.fatherOrbGO.transform.position, transform.position) ;
    }

    void MoveBack(){
        GameHandler.fatherOrb.MoveUsWrapper(this.gameObject.transform.position, GameHandler.player.transform.position);

    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
