using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class HidingSpace : MonoBehaviour, iInteractable
{

    bool alreadyHiding = false;
    public static event Action<GameObject> PlayerHiding;

    bool orbInHidingPlace;
    bool hidingSpace;
    public void PlayerHidingWrapper()
    {
		alreadyHiding = true;
        if (PlayerHiding != null)
        {
            PlayerHiding(gameObject);
        }
    }


    public static event Action<GameObject> PlayerNoLongerHiding;
    public void PlayerStoppedHidingWrapper()
    {
		alreadyHiding = false;
        if (PlayerNoLongerHiding != null)
        {
            PlayerNoLongerHiding(gameObject);
        }

    }

    public void OnHoverMe(Player player)
    {
        Debug.Log("Press E to hide");
    }

    public void OnStopHoverMe(Player player)
    {

    }

    public void OnInteractWithMe(Player player)
    {
        if (!alreadyHiding)
        {
            PlayerHidingWrapper();
        }
        else
        {
            PlayerStoppedHidingWrapper();
        }

    }

    void HidePlayer(Player player)
    {
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
