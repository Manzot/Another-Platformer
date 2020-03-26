using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public enum Abilities { Grappler, Rewind, SlowMotion }
public class PlayerController : MonoBehaviour
{
    const float SLOMO_FACTOR = 0.3f;

    public Vector2 ropeHook;
    Collider2D groundCheckColi;
    public float speed;
    public float swingForce = 4f;
    public float jumpForce = 3f;
    private float jumpInput;
    float horizontal;
    public float aimAngle;
    float timeSlowCooldown = 10f;

    bool timeSlow;

    //int jumpCount = 1;
    int flipX = 0;

    bool isJumping;
    bool isAttacking;
    public bool groundCheck;
    public bool isSwinging;


    public Transform feet;
    public Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Animator animator;

    public Vector2 angleDirection;

    public GameObject crosshair;
    TimeManager timeManager;

    List<Bullet> bulletList;
    Vector3 deflect = new Vector3(-1, 1, -1);
    Bullet bullet;



    public void Initialize()
    {
        //timeManager = GetComponent<TimeManager>();
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        timeManager = new TimeManager();
    }
    public void PostInitialize()
    {
        bulletList = new List<Bullet>();
        bulletList.Add(bullet);
    }
    public void Refresh()
    {

        foreach (Bullet b in bulletList)
        {
            bullet = GameObject.FindObjectOfType<Bullet>();
        }

        MovementAndDoubleJump();
        setCrosshairPoint(CrossairDirection());

        if (Input.GetKeyDown(KeyCode.T))
        {
            TimeSlowAbility();
        }
        TimeSlowReset();
        if (Input.GetKeyDown(KeyCode.K))
        {
            Attack();           
        }
    }
    public void PhysicsRefresh()
    {


    }

    private void TimeSlowAbility()
    {
        if (!timeSlow)
        {
            timeManager.SlowMotion(SLOMO_FACTOR);
            timeSlow = true;
        }
    }
   
    private void TimeSlowReset()
    {
        if (timeSlow)
        {
            if (timeManager.TimeReset(timeSlowCooldown) >= 1)
            {
                timeSlow = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Bullet"))
        {
            if (timeSlow && isAttacking)
            {
                DeflectBullet(collision);
            }
        }
    }

    private void DeflectBullet(Collider2D collision)
    {

            if (timeSlow == true)
            {
                bullet.rb.gravityScale = 0;
                bullet.moveSpeed = bullet.moveSpeed / 1.7f;
                bullet.dir = -1 * bullet.dir;
            }

    }

    /*Get a normalized direction vector from the player to the hook point 
     * and  Inverse the direction to get a perpendicular direction based on the HorizontalInput */
    private Vector2 CalculatePerpendicularDirection()
    {
        Vector2 perpendicularDirection;

        var playerToHookDirection = (ropeHook - (Vector2)transform.position).normalized;
        Debug.DrawLine(transform.position, playerToHookDirection, Color.red, 0f);

        if (horizontal > 0)
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

    public void MovementAndDoubleJump()
    {
        horizontal = Input.GetAxis("Horizontal");
        if (rb.velocity.x > 0)
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        else if (rb.velocity.x < 0)
            transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));


        rb.velocity = new Vector2(horizontal * speed * Time.deltaTime, rb.velocity.y);
        animator.SetFloat("Speed", Mathf.Abs(horizontal));

        if (Grounded())
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                animator.SetBool("isJumping", true);
                rb.AddForce(new Vector2(rb.velocity.x, jumpForce * Time.deltaTime), ForceMode2D.Impulse);
               
                // jumpCount--;
            }
            else
            {
                animator.SetBool("isJumping", false);
            }
            // if (jumpCount < 1)
            // jumpCount = 1;
            //}
            //else
            //{
            //    if (jumpCount > 0 && Input.GetKeyDown(KeyCode.Space))
            //    {
            //        rb.AddForce(new Vector2(rb.velocity.x, jumpForce / 1.4f * Time.deltaTime), ForceMode2D.Impulse);
            //        jumpCount--;
            //    }
        }
    }

    public bool Grounded()
    {
        return groundCheckColi = Physics2D.OverlapCircle(feet.position, 0.1f, LayerMask.GetMask("Ground", "IObject"));
    }

    float CrossairDirection()
    {
        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
        Vector3 faceDirection = worldMousePosition - transform.position;

        aimAngle = Mathf.Atan2(faceDirection.y, faceDirection.x);

        if (aimAngle < 0)
        {
            return aimAngle = Mathf.PI * 2 + aimAngle;
        }
        // angleDirection = Quaternion.Euler(0, 0, aimAngle * Mathf.Rad2Deg) * Vector2.right;
        return aimAngle;
    }

    public void setCrosshairPoint(float aimAngle)
    {
        //if (!crosshairSprite.enabled)
        //{
        //    crosshairSprite.enabled = true;
        //}
        float x = transform.position.x + 2f * Mathf.Cos(aimAngle);
        float y = transform.position.y + 2f * Mathf.Sin(aimAngle);

        Vector3 crosshairPosition = new Vector3(x, y, 0);
        crosshair.transform.position = crosshairPosition;
    }

    public void SwingDirectionForce()
    {
        if (rb.velocity.x < 0f || rb.velocity.x > 0f)
        {

            if (isSwinging)
            {
                Vector2 perpendicularDirection = CalculatePerpendicularDirection();
                var force = perpendicularDirection * swingForce;
                rb.AddForce(force, ForceMode2D.Impulse);
            }
            //else
            //{
            //    if (groundCheck)
            //    {
            //        var groundForce = speed * 2f;
            //        rb.AddForce(new Vector2((horizontal * groundForce - rb.velocity.x) * groundForce, 0));
            //        rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);
            //    }
            //}
        }
    }

    public void Attack()
    {
        if (!isAttacking)
        {
            animator.SetTrigger("Attack");
            isAttacking = true;
        }
    }

    public void DisableBools()
    {
        isAttacking = false;
    }

}

