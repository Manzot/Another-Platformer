using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class RopeSystem : MonoBehaviour
{

    public SpringJoint2D ropeJoint;
    public PlayerController player;
    public bool isRopeAttached = false;
    private Vector2 playerPosition;

    public LineRenderer ropeRenderer;

    GameObject hookShoot;
    GameObject currentHook;
    private List<Vector2> ropePositions = new List<Vector2>();
    GameObject hookPrefab;
    Hook hookRef;
    GameObject hook;
    private float climbSpeed = 5f;

    private void Awake()
    {
        player = GetComponent<PlayerController>();
        ropeJoint.enabled = false;
        playerPosition = transform.position;
        hookPrefab = Resources.Load<GameObject>("Prefabs/Karan/RopeHook");
        hookRef = hookPrefab.GetComponent<Hook>();
        hookShoot = GameObject.Find("Shoot");

    }
    void Update()
    {  
           if (!isRopeAttached) { /*player.setCrosshairPoint(player.aimAngle);*/ player.isSwinging = false; }
            else
            {
                /*player.crosshairSprite.enabled = false;*/
                player.isSwinging = true;
                player.ropeHook = hookRef.hitloc;
            }
        
        HandleInput(player.angleDirection);
        if (hook)
        {
            UpdateRopePosition();
        }
        HandleRopeLength();
    }

    private void UpdateRopePosition()
    {
        ropeRenderer.SetPosition(0, hookShoot.transform.position);
        if (hookRef.canMove)
        {
            ropeRenderer.SetPosition(1, hook.transform.position);

        }
    }

    private void HandleInput(Vector2 aimDirection)
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {

            hook = GameObject.Instantiate<GameObject>(hookPrefab, hookShoot.transform.position, Quaternion.identity);
            ropeRenderer.enabled = true;

            if (isRopeAttached == true) return;

            else {
                player.ropeHook = hookRef.hitloc;
                ropePositions.Add(hookRef.hitloc);
                ropeJoint.distance = Vector2.Distance(playerPosition, hookRef.hitloc);
                ropeJoint.enabled = true;
                ropeJoint.connectedBody = hookRef.hookrb;
                
                isRopeAttached = true;

            }
        }
        else if ((Input.GetKeyDown(KeyCode.Mouse1) && player.isSwinging == true))
        {
          //  transform.GetComponent<Rigidbody2D>().AddForce(new Vector2(player.horizontal*100f, 2f)*2, ForceMode2D.Impulse) ;
            ropeJoint.enabled = false;
            isRopeAttached = false;
            ropeRenderer.enabled = false;

            Destroy(hook);
        }
    }
    private void HandleRopeLength()
    {
        if (Input.GetAxis("Vertical") >= 1f && isRopeAttached)
        {
        
            ropeJoint.distance -= Time.deltaTime * climbSpeed;
        }
        else if (Input.GetAxis("Vertical") < 0f && isRopeAttached)
        {
            ropeJoint.distance += Time.deltaTime * climbSpeed;
        }
    }
}

