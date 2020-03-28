using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class RopeSystem : MonoBehaviour
{

    public SpringJoint2D joint;
    public PlayerController player;
    public bool isRopeAttached = false;
    private Vector2 playerPosition;

    private List<Vector2> ropePositions = new List<Vector2>();
    GameObject hookPrefab;
    Hook hookRef;
    public GameObject hookShoot;
    public GameObject hook;
    private float climbSpeed = 5f;

    private void Awake()
    {
        player = GetComponent<PlayerController>();
        joint = GetComponent<SpringJoint2D>();
        joint.enabled = false;
        playerPosition = transform.position;
        hookPrefab = Resources.Load<GameObject>("Prefabs/Karan/RopeHook");
        hookRef = hookPrefab.GetComponent<Hook>();
        hookShoot = GameObject.Find("Shoot");


    }
    void Update()
    {  
           if (!isRopeAttached) { player.SetCrosshairPoint(player.aimAngle); player.isSwinging = false; }
            else
            {
              
                player.isSwinging = true;
                player.ropeHook = hookRef.hitloc;
            }
        
        HandleInput(player.angleDirection);
       
        HandleRopeLength();
    }


    private void HandleInput(Vector2 aimDirection)
    {
        if (Input.GetMouseButtonDown(0))
        {



            if (!hook)
            {
                hook = GameObject.Instantiate<GameObject>(hookPrefab, hookShoot.transform.position, Quaternion.identity);
            }
            else
            {
                Destroy(hook.gameObject);
                hook = GameObject.Instantiate<GameObject>(hookPrefab, hookShoot.transform.position, Quaternion.identity);
            }
           
            hookRef.ropeRenderer.enabled = true;

            if (isRopeAttached == true) return;

            else {
                player.ropeHook = hookRef.hitloc;
                ropePositions.Add(hookRef.hitloc);
               
                joint.autoConfigureConnectedAnchor = false;
                joint.connectedAnchor = hookRef.hitloc;
                joint.distance = Vector2.Distance(playerPosition, hookRef.hitloc);
                joint.enabled = true;
                isRopeAttached = true;
                player.isSwinging = true;

            }
        }
        else if ((Input.GetMouseButtonUp(0) && player.isSwinging == true))
        {
            player.transform.GetComponent<Rigidbody2D>().AddForce(new Vector2(player.horizontal*2, 1f)*10, ForceMode2D.Impulse) ;
            joint.enabled = false;
            isRopeAttached = false;
            hookRef.ropeRenderer.enabled = false;

            Destroy(hook);
        }
    }
    private void HandleRopeLength()
    {
        if (Input.GetAxis("Vertical") >= 1f && isRopeAttached)
        {

            joint.distance -= Time.deltaTime * climbSpeed;
        }
        else if (Input.GetAxis("Vertical") < 0f && isRopeAttached)
        {
            joint.distance += Time.deltaTime * climbSpeed;
        }
    }
}

