using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : IManageables
{
    PlayerController player;
    private float spawnTime = 5f;
    #region Singleton

    private static PlayerManager instance = null;

    public PlayerManager() { }

    public static PlayerManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new PlayerManager();
            }
            return instance;
        }
    }

    #endregion
    public void Initialize()
    {
        GameObject playerPrefab = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Karan/Player"));
        player = playerPrefab.GetComponent<PlayerController>();
        player.transform.position = new Vector2(10, 13);
        player.Initialize();
    }
    public void PhysicsRefresh()
    {
        player.PhysicsRefresh();
    }
    public void PostInitialize()
    {
        player.PostInitialize();
    }
    public void Refresh()
    {
        if (player.isActiveAndEnabled) 
            player.Refresh();
        IsDead();

    }
    public void PlayerSpawn(GameObject go)
    {
        go.SetActive(true);
    }
    public void IsDead()
    {
        if (!player.isActiveAndEnabled)
        {
            Vector3 deathLoc = player.transform.position;
            spawnTime -= Time.deltaTime;
            if (spawnTime <= 0)
            {
                PlayerSpawn(player.gameObject);
                player.transform.position = deathLoc + new Vector3(0, 5, 0);
                player.transform.rotation = Quaternion.Euler(Vector3.zero);
                spawnTime = 5f;
            }
        }
    }
}
