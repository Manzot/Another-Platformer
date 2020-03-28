using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shoot : MonoBehaviour
{
    public GameObject shootBullet;
    float counter = 1f;
    GameObject ok;
    PlayerController player;
    // Start is called before the first frame update
    void Start()
    {
        ok=GameObject.Find("Shooter");
        player = FindObjectOfType<PlayerController>();

    }

    // Update is called once per frame
    void Update()
    {
        counter -= Time.deltaTime;
            if (counter < 0) {
            if (player.isAlive)
            {
                GameObject.Instantiate(shootBullet, ok.transform.position, Quaternion.identity, this.transform);
            }
            counter = 1f;
            
        }
    }
   
}
