using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum Abilities { Grappler,Rewind,SlowMotion}
public class PlayerController : MonoBehaviour
{
    public Vector2 ropeHook;

    public float speed;
    public float swingForce = 4f;
    public float jumpForce = 3f;
    private float jumpInput;
    public float horizontalInput;
    public float aimAngle;

    int jumpCount = 1;
    int flipX = 0;

    bool isJumping;
    public bool groundCheck;
    public bool isSwinging;


    public Transform feet;
    public Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Animator animator;

    public Vector2 angleDirection;

    public Transform crosshair;
    public SpriteRenderer crosshairSprite;
    TimeManager timeManager;

    public void Initialize()
    {
        timeManager = GetComponent<TimeManager>();
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }
    public void PostInitialize()
    {

    }
    public void Refresh()
    {
        jumpInput = Input.GetAxis("Jump");
        horizontalInput = Input.GetAxis("Horizontal");
        float halfHeight = sprite.bounds.extents.y;
        groundCheck = Physics2D.OverlapCircle(feet.position, 0.2f, LayerMask.GetMask("Ground", "IObject"));


        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
        Vector3 faceDirection = worldMousePosition - transform.position;
        aimAngle = Mathf.Atan2(faceDirection.y, faceDirection.x);

        if (aimAngle < 0)
        {
            aimAngle = Mathf.PI * 2 + aimAngle;

        }

        angleDirection = Quaternion.Euler(0, 0, aimAngle * Mathf.Rad2Deg) * Vector2.right;

        if (Input.GetKeyDown(KeyCode.L))
        {
            timeManager.slowMotion();
        }
    }

    public void PhysicsRefresh()
    {
        if (horizontalInput < 0f || horizontalInput > 0f)
        {
            animator.SetFloat("Speed", Mathf.Abs(horizontalInput));
            sprite.flipX = horizontalInput < 0f;

            rb.velocity = new Vector2(horizontalInput * speed * Time.fixedDeltaTime, rb.velocity.y);

            if (isSwinging)
            {
                Vector2 perpendicularDirection = CalculatePerpendicularDirection();
                var force = perpendicularDirection * swingForce;
                rb.AddForce(force, ForceMode2D.Impulse);
            }
            else
            {
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
            animator.SetFloat("Speed", 0f);
        }

        if (!isSwinging)
        {
            if (!groundCheck) return;

            IsJumping();
        }
      
    }

    /*Get a normalized direction vector from the player to the hook point 
     * and  Inverse the direction to get a perpendicular direction based on the HorizontalInput */
    private Vector2 CalculatePerpendicularDirection()
    {
        Vector2 perpendicularDirection;

        var playerToHookDirection = (ropeHook - (Vector2)transform.position).normalized;
        Debug.DrawLine(transform.position, playerToHookDirection, Color.red, 0f);

        if (horizontalInput > 0)
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

        return perpendicularDirection;
    }

    /* If the Player is Not Swinging or not on the ground */
    private void IsJumping()
    {
        isJumping = jumpInput > 0f;
        if (isJumping)
        {
            animator.SetBool("isJumping", true);
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
        else
        {
            animator.SetBool("isJumping", false);
        }
    }

    public void setCrosshairPoint(float aimAngle)
    {
        if (!crosshairSprite.enabled)
        {
            crosshairSprite.enabled = true;
        }
        float x = transform.position.x + 2f * Mathf.Cos(aimAngle);
        float y = transform.position.y + 2f * Mathf.Sin(aimAngle);

        Vector3 crosshairPosition = new Vector3(x, y, 0);
        crosshair.transform.position = crosshairPosition;
    }
}

