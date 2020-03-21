using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class RopeSystem : MonoBehaviour
{

    public SpringJoint2D ropeJoint;
    public Transform crosshair;
    public SpriteRenderer crosshairSprite;
    public PlayerController playerMovement;
    public bool isRopeAttached = false;
    private Vector2 playerPosition;

    public LineRenderer ropeRenderer;

    public Vector2 angleDirection;
    GameObject hookShoot;
    GameObject currentHook;
    private List<Vector2> ropePositions = new List<Vector2>();
    GameObject hookPrefab;
    Hook hookRef;
    GameObject hook;
    private float climbSpeed = 5f;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerController>();
        ropeJoint.enabled = false;
        playerPosition = transform.position;
        hookPrefab = Resources.Load<GameObject>("Prefabs/Karan/RopeHook");
        hookRef = hookPrefab.GetComponent<Hook>();
        hookShoot = GameObject.Find("Shoot");

    }
    void Update()
    {
        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
        Vector3 faceDirection = worldMousePosition - transform.position;
        float aimAngle = Mathf.Atan2(faceDirection.y, faceDirection.x);

        if (aimAngle < 0)
        {
            aimAngle = Mathf.PI * 2 + aimAngle;

        }

        angleDirection = Quaternion.Euler(0, 0, aimAngle * Mathf.Rad2Deg) * Vector2.right;
        playerPosition = transform.position;
       
            if (!isRopeAttached) { setCrosshairPoint(aimAngle); playerMovement.isSwinging = false; }
            else
            {
                crosshairSprite.enabled = false;
                playerMovement.isSwinging = true;
                playerMovement.ropeHook = ropePositions.Last();
            }
        
        HandleInput(angleDirection);
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

    private void setCrosshairPoint(float aimAngle)
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
    private void HandleInput(Vector2 aimDirection)
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {

            hook = GameObject.Instantiate<GameObject>(hookPrefab, hookShoot.transform.position, Quaternion.identity);
            ropeRenderer.enabled = true;

            if (isRopeAttached == true) return;

            else { 

                ropePositions.Add(hookRef.hitloc);
                ropeJoint.distance = Vector2.Distance(playerPosition, hookRef.hitloc);

                ropeJoint.enabled = true;
                ropeJoint.connectedBody = hookRef.hookrb;
                isRopeAttached = true;

            }
        }
        else if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            ropeJoint.enabled = false;
            isRopeAttached = false;
            ropeRenderer.enabled = false;

            Destroy(hook);
        }
    }
    private void HandleRopeLength()
    {
        // 1
        if (Input.GetAxis("Vertical") >= 1f && isRopeAttached)
        {
            Debug.Log("ok");
            ropeJoint.distance -= Time.deltaTime * climbSpeed;
        }
        else if (Input.GetAxis("Vertical") < 0f && isRopeAttached)
        {
            ropeJoint.distance += Time.deltaTime * climbSpeed;
        }
    }
}

