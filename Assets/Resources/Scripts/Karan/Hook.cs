using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
   public Rigidbody2D hookrb;
    RopeSystem rope;
    public float moveSpeed = 30000f;
    public bool canMove=true;
    public Vector2 hitloc;
    public Vector3 range;

    public static Hook currentHook { get; private set; }

    public void Awake()
    {
        currentHook = this;
        range = new Vector3(10, 10, 0);
    }

   public void Start()
    {
        hookrb = GetComponent<Rigidbody2D>();
        rope = GameObject.Find("Player").GetComponent<RopeSystem>();
    }
    void FixedUpdate()
    {
        if (canMove)
        {
            movement(rope.angleDirection);
        }
    }

    private void  movement(Vector2 dir)
    {

        if (transform.position.y != range.y || transform.position.x != range.x)
        {
            transform.GetComponent<Rigidbody2D>().AddForce(dir * moveSpeed * Time.fixedDeltaTime, ForceMode2D.Impulse);
    
        }
        else
        {
            Destroy(this);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.layer == LayerMask.NameToLayer("Rope")&&canMove)
        {
            hookrb.velocity = Vector3.zero;
            RaycastHit2D hit;
            hit = Physics2D.Raycast(this.transform.position, rope.angleDirection);
            if(hit)
            {
                hitloc = hit.point;
                hookrb.isKinematic = true;
                rope.ropeJoint.connectedBody = hookrb;
            } 
           
        }
      
        else
        {
            rope.ropeRenderer.enabled = false;
            rope.isRopeAttached = false;
            canMove = false;
        }
    }

}
