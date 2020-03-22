using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PlayerController : MonoBehaviour
{
    public Vector2 ropeHook;

    public float speed;
    public float swingForce= 4f;
    public float jumpSpeed = 3f;
    private float jumpInput;
    private float horizontalInput;

    int jumpCount = 1;
    int flipX = 0;

    bool isJumping;
    public bool groundCheck;
    public bool isSwinging;
    public Transform feet;
    
    Collider2D groundCheckColi;
    SpriteRenderer sprite;
    Rigidbody2D rb;
    Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        jumpInput = Input.GetAxis("Jump");
        horizontalInput = Input.GetAxis("Horizontal");
        float halfHeight = transform.GetComponent<SpriteRenderer>().bounds.extents.y;
        groundCheck = Physics2D.OverlapCircle(feet.position, 0.2f, LayerMask.GetMask("Ground", "IObject"));
    }

    void FixedUpdate()
    {
        if (horizontalInput < 0f || horizontalInput > 0f)
        {
            animator.SetFloat("Speed", Mathf.Abs(horizontalInput));
            sprite.flipX = horizontalInput < 0f;
            rb.velocity = new Vector2(horizontalInput * speed * Time.fixedDeltaTime, rb.velocity.y);
            if (isSwinging)
            {
                //animator.SetBool("IsSwinging", true);

                // 1 - Get a normalized direction vector from the player to the hook point
                var playerToHookDirection = (ropeHook - (Vector2)transform.position).normalized;
                Debug.DrawLine(transform.position, playerToHookDirection, Color.red, 0f);
                // 2 - Inverse the direction to get a perpendicular direction
                Vector2 perpendicularDirection;
                if (horizontalInput < 0)
                {
                    perpendicularDirection = new Vector2(-playerToHookDirection.y, playerToHookDirection.x);
                    var leftPerpPos = (Vector2)transform.position + perpendicularDirection * -2f;
                    Debug.DrawLine(transform.position, leftPerpPos, Color.green, 0f);
                }
                else
                {
                    perpendicularDirection = new Vector2(playerToHookDirection.y, -playerToHookDirection.x);
                    var rightPerpPos = (Vector2)transform.position - perpendicularDirection * +2f;
                    Debug.DrawLine(transform.position, rightPerpPos, Color.green, 0f);
                }

                var force = perpendicularDirection * swingForce;
                rb.AddForce(force, ForceMode2D.Impulse);
            }
            else
            {
                //animator.SetBool("IsSwinging", false);
                if (groundCheck)
                {
                    var groundForce = speed * 2f;
                    rb.AddForce(new Vector2((horizontalInput * groundForce - rb.velocity.x) * groundForce, 0));
                    rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);
                }
            }
        }
        else
        {
            /*animator.SetBool("IsSwinging", false);
            */
            animator.SetFloat("Speed", 0f);
        }

        if (!isSwinging)
        {
            if (!groundCheck) return;

            isJumping = jumpInput > 0f;
            if (isJumping)
            {
                animator.SetBool("isJumping", true);
                rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
            }
            else
            {
                animator.SetBool("isJumping", false);
            }
        }
    }
   
}


