using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocationHandler : MonoBehaviour
{

    LayerMask sconceLayer;
    LayerMask orbLayer;

    public List<iInteractable> objectsWereHovering = new List<iInteractable>();
    public List<GameObject> gameObjectsWereHovering = new List<GameObject>();
    Player player;

    Sconce sconceHoveredOver;

    FatherOrb orbHoveredOver;
    iInteractable objectHovering = null;

    void Awake()
    {
        player = GetComponentInParent<Player>();
    }
    public void OnTriggerEnter2D(Collider2D hit)
    {
        iInteractable interactableObject = hit.GetComponent<iInteractable>();

        if (interactableObject != null)
        {

            interactableObject.OnHoverMe(player);
            objectsWereHovering.Add(interactableObject);
            gameObjectsWereHovering.Add(hit.gameObject);
        }

    }

    public void OnTriggerExit2D(Collider2D hit)
    {
        iInteractable interactableObject = hit.GetComponent<iInteractable>();
        if (interactableObject != null)
        {

            if (objectsWereHovering.Contains(interactableObject))
            {
                objectsWereHovering.Remove(interactableObject);
            }
            if (gameObjectsWereHovering.Contains(hit.gameObject))
            {
                gameObjectsWereHovering.Remove(hit.gameObject);
            }
            //objectHovering.OnHoverMe(player);
        }

    }
    bool CheckIfHoveringOrbAndSconce()
    {
        bool hoveringSconce = false;
        bool hoveringOrb = false;
        foreach (GameObject go in gameObjectsWereHovering)
        {
            if (go.GetComponent<Sconce>() != null)
            {
                hoveringSconce = true;
            }
            if (go.GetComponent<FatherOrb>() != null)
            {
                hoveringOrb = true;
            }

        }
        if (hoveringSconce && hoveringOrb)
        {
            return true;
        }
        else { return false; }
    }

    // bool CheckIfHoveringOrbAndSconce(){
    //     foreach(iInteractable interactableObject in objectsWereHovering){

    //         }
    //     }
    // }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            //todo -- if hovering multiple objects, make sure 
            if (objectsWereHovering.Count > 0)
            {
                objectsWereHovering[0].OnInteractWithMe(player);
            }
            // if (objectsWereHovering.Count >  1)
            // {
            //     objectsWereHovering[0].OnInteractWithMe(player);
            // }
        }
    }
}
