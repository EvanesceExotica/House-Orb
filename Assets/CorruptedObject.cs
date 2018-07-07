using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
public class CorruptedObject : ParentTrigger
{

	Transform corruptionEffect;
    CircleCollider2D ourCollider;
    VisibleCollider ourVisibleCollider;

    float cooldownInterval;
    void Awake()
    {
		corruptionEffect = transform.GetChild(0);
        ourCollider = GetComponentInChildren<CircleCollider2D>();
 //       ourVisibleCollider = GetComponentInChildren<VisibleCollider>();
//        ourVisibleCollider.OurColliderType = VisibleCollider.ColliderTypes.Circle;
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
		StartCoroutine(Cooldown());
        if (StoppedCorrupting != null)
        {
            StoppedCorrupting();
        }
    }

    bool canGrowCorruption = false;
	public bool corrupting = false;
    void SetCanGrowCorruption(MonoBehaviour mono)
    {
        canGrowCorruption = true;
    }

    public IEnumerator GrowCollider()
    {
		Vector2 newVector = Vector2.zero;
        while (true)
        {
			if(!corrupting){
				break;
			}
			newVector = new Vector2(corruptionEffect.localScale.x + 0.5f, corruptionEffect.localScale.y + 0.5f);
			//Debug.Log("We should be growing to New vector: "  + newVector);
			corruptionEffect.DOScale(newVector, 0.5f);

		//	ourCollider.radius = Mathf.Lerp(ourCollider.radius, ourCollider.radius + 0.5f, 0.5f);
   //         ourCollider.radius += 0.5f;

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

	void ClearCorruption(){
		corruptionEffect.DOScale(Vector2.zero, 1.0f);
		corruptionEffect.gameObject.SetActive(false);
	}
    public override void OnChildTriggerEnter2D(Collider2D hit, GameObject child)
    {
        if (hit.gameObject == GameHandler.fatherOrbGO && canGrowCorruption)
        {
            CorruptingWrapper();
        }
    }

    public override void OnChildTriggerExit2D(Collider2D hit, GameObject child)
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
