using UnityEngine;
using System.Collections;
using System.Reflection.Emit;

public class AlienController : MonoBehaviour
{
    public float MaxSpeed = 10f;
    public float JumpForce = 750f;
    private bool _facingRight = true;

    private Rigidbody2D _rigidbody;
    private Animator _animator;

    private bool _grounded = false;
    public Transform GroundCheck;
    private float _groundRadius = .2f;
    public LayerMask GroundLayerMask;

    private void Start ()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _animator.speed = 5;
    }

    private void Update()
    {
        if (_grounded && Input.GetButtonDown("Jump"))
        {
            _animator.SetBool("Ground", false);
            _rigidbody.AddForce(new Vector2(0, JumpForce));
        }
    }

    private void FixedUpdate ()
    {
        _grounded = Physics2D.OverlapCircle(GroundCheck.position, _groundRadius, GroundLayerMask);
        _animator.SetBool("Ground", _grounded);
        
        _animator.SetFloat("vSpeed", _rigidbody.velocity.y);

        float move = Input.GetAxisRaw("Horizontal");
        _animator.SetFloat("Speed", Mathf.Abs(move));

        _rigidbody.velocity = new Vector2(move * MaxSpeed, _rigidbody.velocity.y);

        if(move > 0 && !_facingRight)
            Flip();
        else if(move <0 && _facingRight)
            Flip();
    }

    void Flip()
    {
        _facingRight = !_facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
