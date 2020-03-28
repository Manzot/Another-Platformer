using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public Transform swordPoint;
    public float inRange = 1f;
    Animator animator;
    public LayerMask EnemyLayer;
    Bullet move;

    private void Awake()
    {
        animator = GetComponent<Animator>();
       
    }
    public void Start()
    {
        if (move)
        {
            move = GameObject.Find("Bullet").GetComponent<Bullet>();
        }
    }
    public void Attack()
    {
        animator.SetTrigger("Attack");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(swordPoint.position, inRange, EnemyLayer);

        foreach (Collider2D enemy in hitEnemies) {
            Damage();
        }

    }

    private void Damage()
    {
        Debug.Log("D A M A G E   D O N E...");
    }
    private void OnDrawGizmosSelected()
    {
        if (swordPoint == null) return;
        Gizmos.DrawWireSphere(swordPoint.position, inRange);
    }
   
   
}
