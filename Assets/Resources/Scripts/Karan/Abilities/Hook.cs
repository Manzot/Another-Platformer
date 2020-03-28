using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
    public Rigidbody2D hookrb;
    RopeSystem rope;
    public float moveSpeed = 30000f;
    public bool canMove = true;
    public Vector2 hitloc;
    public Vector3 range;
    PlayerController player;
    
    public LineRenderer ropeRenderer;
    private float step =25f;

    public static Hook currentHook { get; private set; }

    public void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        ropeRenderer = GetComponent<LineRenderer>();
       
    }

    public void Start()
    {
        
        hookrb = GetComponent<Rigidbody2D>();
        rope = GameObject.FindGameObjectWithTag("Player").GetComponent<RopeSystem>();
        ropeRenderer.enabled = true;
    }
    void FixedUpdate()
    {
        if (canMove)
        {
            Movement(player.angleDirection);
        }
    }

    private void Movement(Vector2 dir)
    {
            transform.GetComponent<Rigidbody2D>().AddForce(dir * moveSpeed * Time.fixedDeltaTime, ForceMode2D.Impulse);
   
        if (rope.hook)
        {
            UpdateRopePosition();
        }
    }

    private void UpdateRopePosition()
    {
        ropeRenderer.SetPosition(0, rope.hookShoot.transform.position);
        if (canMove)
        {
            ropeRenderer.SetPosition(1, rope.hook.transform.position);

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Rope") && canMove)
        {
            hookrb.velocity = Vector3.zero;
            RaycastHit2D hit;
            hit = Physics2D.Raycast(this.transform.position, player.angleDirection);
            if (hit)
            {
                Debug.Log(hit.collider.gameObject.name);
                hitloc = hit.point;
                
                rope.joint.connectedBody = hookrb;
                rope.joint.distance -= step*Time.deltaTime;
                hookrb.isKinematic = true;

            }

        }

        else
        {
            ropeRenderer.enabled = false;
            rope.isRopeAttached = false;
            canMove = false;
        }
    }

}
