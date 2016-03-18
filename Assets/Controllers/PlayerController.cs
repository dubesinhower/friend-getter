using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float MaxSpeed = 10f;
    public float JumpForce = 700;
    public Transform GroundCheck;
    public LayerMask Ground;

    bool FacingRight = true;
    Rigidbody2D Rigid;
    Animator Animator;
    bool Grounded = false;
    float GroundRadius = 0.1f; 

    void Awake()
    {
        Rigid = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
	    Grounded = Physics2D.OverlapCircle(GroundCheck.position, GroundRadius, Ground);
        Animator.SetBool("Ground", Grounded);

	    var move = Input.GetAxis("Horizontal");
        Animator.SetFloat("Speed", Mathf.Abs(move));
        Rigid.velocity = new Vector2(move * MaxSpeed, Rigid.velocity.y);

        if(move > 0 && !FacingRight)
            Flip();
        else if (move < 0 && FacingRight) 
            Flip();
	}

    void Update()
    {
        if (Grounded && Input.GetButtonDown("Jump"))
        {
            Animator.SetBool("Ground", false);
            Animator.SetTrigger("Jump");
            Rigid.AddForce(new Vector2(0, JumpForce));
        }
    }

    void Flip()
    {
        FacingRight = !FacingRight;
        var theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
