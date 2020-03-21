using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveForward : MonoBehaviour
{
    public float moveSpeed = 2f;
    // Start is called before the first frame update
    public Vector3 dir;
    Transform Player;

    void Awake()
    {
        dir = GameObject.Find("Player").GetComponent<Transform>().position;
    }

    // Update is called once per frame
    void Update()
    {
        Move(dir);
    }

    public void Move(Vector3 dir)
    {
        this.transform.position += dir * Time.deltaTime * moveSpeed;
    }
}
