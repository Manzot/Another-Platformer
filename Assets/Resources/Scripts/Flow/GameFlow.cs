﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFlow: IManageables
{
    #region Singleton

    private static GameFlow instance = null;

    public GameFlow() { }

    public static GameFlow Instance {
        get
        {
            if (instance == null)
            {
                instance = new GameFlow();
            }
            return instance;
        }
    }

    #endregion
    PlayerController player;

    public void Initialize()
    {
        EnemyManager.Instance.Initialize();
        PlayerManager.Instance.Initialize();
        /*AbilityManager.Instance.Initialize(player);*/
    }
    public void PostInitialize()
    {
        EnemyManager.Instance.PostInitialize();
        PlayerManager.Instance.PostInitialize();
        /*AbilityManager.Instance.PostInitialize();*/
    }
    public void Refresh()
    {
        EnemyManager.Instance.Refresh();
        PlayerManager.Instance.Refresh();
        /*AbilityManager.Instance.Refresh();*/
    }
    public void PhysicsRefresh()
    {
        EnemyManager.Instance.PhysicsRefresh();
        PlayerManager.Instance.PhysicsRefresh();
      /*  AbilityManager.Instance.PhysicsRefresh();*/
    }

}
