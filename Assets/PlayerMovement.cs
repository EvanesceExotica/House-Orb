using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [HideInInspector] public bool facingRight = true;
    [HideInInspector] public bool jump = false;
    public float moveForce = 15f;
    public float maxSpeed = 5f;
    public float jumpForce = 1000f;
    public Transform groundCheck;


    private bool grounded = false;
    //private Animator anim;
    private Rigidbody2D rb;

    [SerializeField] bool cantMove = false;


    // Use this for initialization
    void Awake()
    {
        // anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        OrbController.ChannelingOrb += SetCantMove;
        //make this a 
        OrbController.StoppedChannelingOrb += SetYesCanMove;

        HidingSpace.PlayerHiding += SetCantMove;
        HidingSpace.PlayerNoLongerHiding += SetYesCanMove;
    }

    List<GameObject> incapacitators = new List<GameObject>();
    void SetCantMove(GameObject incapacitator)
    {
        Debug.Log(incapacitator.name + " made us not move ");
        incapacitators.Add(incapacitator);
        cantMove = true;
    }

    void SetYesCanMove(GameObject incapacitator)
    {
        incapacitators.Remove(incapacitator);
        if (incapacitators.Count == 0)
        {
            cantMove = false;
        }
    }
    // Update is called once per frame
    void Update()
    {
        grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Floor"));

        if (Input.GetButtonDown("Jump") && grounded)
        {
            jump = true;
        }
    }

    void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");

        // anim.SetFloat("Speed", Mathf.Abs(h));

        if (!cantMove)
        {
            if (h * rb.velocity.x < maxSpeed)
                rb.AddForce(Vector2.right * h * moveForce);

            if (Mathf.Abs(rb.velocity.x) > maxSpeed)
                rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxSpeed, rb.velocity.y);

            if (h > 0 && !facingRight)
                Flip();
            else if (h < 0 && facingRight)
                Flip();

            if (jump)
            {
                // anim.SetTrigger("Jump");
                rb.AddForce(new Vector2(0f, jumpForce));
                jump = false;
            }
        }
    }


    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;

        foreach (Transform child in transform)
        {
            Vector3 childScale = transform.localScale;
            childScale.x *= -1;
            transform.localScale = theScale;
        }
    }
}
// 	LayerMask floor;
// 	bool grounded;
// 	Transform groundCheck;
// 	// Use this for initialization
// 	void Start () {

// 	}

// 	// Update is called once per frame
// 	void Update () {
// 		grounded = Physics2D.OverlapCircle(groundCheck.transform.position, 0.5f);
// 		if(grounded && )
// 	}
// }
