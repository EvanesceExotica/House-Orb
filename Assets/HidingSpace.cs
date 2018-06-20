using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class HidingSpace : MonoBehaviour, iInteractable
{
    public Room parentRoom;

    public LayerMask defaultLayer;

    public LayerMask hidingLayer;

    public Transform hidingTransform;

    public Transform unhiddenSpace;
    bool alreadyHiding = false;
    public static event Action<MonoBehaviour> PlayerHiding;

    bool orbInHidingPlace;
    bool hidingSpace;

    public List<Sprite> sprites = new List<Sprite>();

   public enum SpriteVariations{
        Wood
    }
    public SpriteVariations ourSpriteVariation;

    SpriteRenderer spriteRenderer;
    GenerateNewBounds boundsGenerator;
    void Awake(){
        boundsGenerator = GetComponent<GenerateNewBounds>();
        //TODO: put the above back in after testing
        // spriteRenderer = GetComponent<SpriteRenderer>();
        // for(int i = 0; i < sprites.Count; i++){
        //     if((int)ourSpriteVariation == i){
        //         spriteRenderer.sprite = sprites[i];
        //     }
        // }
        // if(spriteRenderer.sprite != null){
        //     boundsGenerator.GenerateNewColliderSize();
        // }
        
    }
    public void PlayerHidingWrapper()
    {
		alreadyHiding = true;
        GameHandler.playerGO.transform.position = hidingTransform.position;
        GameHandler.playerGO.layer = LayerMask.NameToLayer("HidingSpace");//LayerMask.(int)hidingLayer;
        if (PlayerHiding != null)
        {
            PlayerHiding(this);
        }
    }


    public static event Action<MonoBehaviour> PlayerNoLongerHiding;
    public void PlayerStoppedHidingWrapper()
    {
		alreadyHiding = false;
        GameHandler.playerGO.transform.position = unhiddenSpace.position;
        GameHandler.playerGO.layer = GameHandler.defaultPlayerLayer;
        if (PlayerNoLongerHiding != null)
        {
            PlayerNoLongerHiding(this);
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
