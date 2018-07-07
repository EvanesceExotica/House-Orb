using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(VisibleCollider))]
public class CorruptedObject : MonoBehaviour
{

    CircleCollider2D ourCollider;
    VisibleCollider ourVisibleCollider;

    float cooldownInterval;
    void Awake()
    {
        ourCollider = GetComponent<CircleCollider2D>();
        ourVisibleCollider = GetComponent<VisibleCollider>();
        ourVisibleCollider.OurColliderType = VisibleCollider.ColliderTypes.Circle;
        OrbController.ChannelingOrb += SetCanGrowCorruption;
		Sconce.OrbInSconce += SetOrbInSconce;
		Sconce.OrbRemovedFromSconce += SetOrbOutOfSconce;
    }
    public static event Action Corrupting;

    void CorruptingWrapper()
    {
		corrupting = true;
		StartCoroutine(GrowCollider());
        if (Corrupting != null)
        {
            Corrupting();
        }
    }
    public static event Action StoppedCorrupting;

    void StoppedCorruptingWrapper()
    {
		corrupting = false;
        if (StoppedCorrupting != null)
        {
            StoppedCorrupting();
        }
    }

    bool canGrowCorruption = false;
	bool corrupting = false;
    void SetCanGrowCorruption(MonoBehaviour mono)
    {
        canGrowCorruption = true;
    }

    public IEnumerator GrowCollider()
    {
        while (true)
        {
			if(!corrupting){
				break;
			}
            ourCollider.radius += 0.5f;
			yield return new WaitForSeconds(0.5f);
        }
    }

	bool orbInSconce = false;

	void SetOrbInSconce(MonoBehaviour mono){
		orbInSconce = true;
	}

	void SetOrbOutOfSconce(MonoBehaviour mono){
		orbInSconce = false;
	}

	public IEnumerator Cooldown(){
		while(true){
			if(corrupting){
				break;
			}
			ourCollider.radius -= 0.5f;
			if(orbInSconce){
				//the orb being in the sconce should drastically reduce the uncorruption time
				yield return new WaitForSeconds(1.0f);
			}
			else{
				yield return new WaitForSeconds(5.0f);
			}
		}
	}
    void OnTriggerEnter2D(Collider2D hit)
    {
        if (hit.gameObject == GameHandler.fatherOrbGO && canGrowCorruption)
        {
            CorruptingWrapper();
        }
    }

    void OnTriggerExit2D(Collider2D hit)
    {
        if (hit.gameObject == GameHandler.fatherOrbGO && canGrowCorruption)
        {
            StoppedCorruptingWrapper();
        }
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
