using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float moveSpeed = 1f;

    public Vector3 dir;
    Transform Player;
    public Rigidbody2D rb;

    void Awake()
    {
        dir = (GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>().position- this.transform.position).normalized;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Move(dir);
        
    }

    public void Move(Vector3 dir)
    {
        transform.position += dir * moveSpeed * Time.deltaTime;
    }
}
