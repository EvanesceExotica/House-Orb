using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public class OrbController : MonoBehaviour
{
    public static event Action<GameObject> ChannelingOrb;
    void ChannelingOrbWrapper(GameObject orbGO)
    {
        if (ChannelingOrb != null)
        {
            ChannelingOrb(orbGO);
        }
    }
    public static event Action<GameObject> StoppedChannelingOrb;
    void StoppedChannelingOrbWrapper(GameObject orbGO)
    {
        if (StoppedChannelingOrb != null)
        {
            StoppedChannelingOrb(orbGO);
        }
    }
    bool canChannelOrb = false;
    bool channelingOrb = false;
    // Use this for initialization
    Rigidbody2D orbRigidBody;
    FatherOrb orb;
    float speed = 5.0f;

    void Awake()
    {
        FatherOrb.PickedUp += SetCanBeChanneled;
        FatherOrb.Dropped += SetCanNOTBeChanneled;
        HiddenSconce.SconceRevealed += StopOrbBeingChanneled;
        orb = GetComponent<FatherOrb>();
        orbRigidBody = GetComponent<Rigidbody2D>();

    }

    void SetCanBeChanneled()
    {

        canChannelOrb = true;
    }

    void SetCanNOTBeChanneled()
    {
        canChannelOrb = false;
    }

    void StartOrbBeingChanelled()
    {
        channelingOrb = true;
        orb.SetOrbBeingChanneled();
        ChannelingOrbWrapper(gameObject);
        orbRigidBody.bodyType = RigidbodyType2D.Dynamic;
        orbRigidBody.gravityScale = 0;

    }

    void StopOrbBeingChanneled(FatherOrb.HeldStatuses statusNow)
    {

        channelingOrb = false;
        //todo: whether or not this is carried or handled depends on whether or not
        //todo: maybe the orb begins screaming when outside of sconce and hand for too long
        orbRigidBody.velocity = Vector2.zero;
        orb.heldStatus = statusNow;
        StoppedChannelingOrbWrapper(gameObject);
        orbRigidBody.bodyType = RigidbodyType2D.Kinematic;
    }

   

    void FixedUpdate()
    {
        if (channelingOrb)
        {
            Debug.Log("Orb is being channeled now");
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");
            
            if (moveHorizontal > 0)
            {

                transform.Translate(Vector3.right * speed * Time.deltaTime);
            }
            if (moveHorizontal < 0)
            {

                transform.Translate(-Vector3.right * speed * Time.deltaTime);
            }
            if(moveVertical > 0){
                transform.Translate(Vector3.up * speed * Time.deltaTime);

            }
            if(moveVertical < 0 ){
                transform.Translate(-Vector3.up * speed * Time.deltaTime);
            }
            Vector2 movement = new Vector2(moveHorizontal, moveVertical);
            orbRigidBody.velocity = movement * speed;
        }

    }

    void ReturnToPlayer()
    {
        orb.MoveUsWrapper(transform.position, GameHandler.playerGO.transform.position);
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (canChannelOrb && !channelingOrb)
            {
                StartOrbBeingChanelled();
            }
            else if (channelingOrb)
            {
                StopOrbBeingChanneled(FatherOrb.HeldStatuses.Carried);
                ReturnToPlayer();
            }
        }
    }
}
