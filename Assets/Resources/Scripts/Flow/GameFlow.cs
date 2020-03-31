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

    public void Initialize()
    {
        PlayerManager.Instance.Initialize();
        EnemyManager.Instance.Initialize();
        BossManager.Instance.Initialize();
    }
    public void PostInitialize()
    {
        PlayerManager.Instance.PostInitialize();
        EnemyManager.Instance.PostInitialize();
        BossManager.Instance.PostInitialize();
        TimerDelg.Instance.PostInitialize();
    }
    public void Refresh()
    {
        PlayerManager.Instance.Refresh();
        EnemyManager.Instance.Refresh();
        BossManager.Instance.Refresh();
        TimerDelg.Instance.Refresh();
    }
    public void PhysicsRefresh()
    {
        PlayerManager.Instance.PhysicsRefresh();
        EnemyManager.Instance.PhysicsRefresh();
        BossManager.Instance.PhysicsRefresh();
    }

}
