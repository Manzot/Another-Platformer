using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deflect : MonoBehaviour
{
    List<Bullet> bulletList;
    Vector3 deflect = new Vector3(-1, 1, -1);
    Bullet bullet;

    public void Awake()
    {

    }
    public void Start()
    {
        bulletList = new List<Bullet>();
        bulletList.Add(bullet);

    }

    private void Update()
    {
        foreach (Bullet b in bulletList)
        {
            bullet = GameObject.FindObjectOfType<Bullet>();
            
       
        }

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.layer == LayerMask.NameToLayer("Bullet"))
        {
            bullet.rb.gravityScale = 0;
            bullet.dir = -1*bullet.dir;

        }
    }
}
