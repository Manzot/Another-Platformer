using System.Collections;
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
        PlayerManager.Instance.Initialize();
        EnemyManager.Instance.Initialize();
        /*AbilityManager.Instance.Initialize(player);*/
    }
    public void PostInitialize()
    {
        PlayerManager.Instance.PostInitialize();
        EnemyManager.Instance.PostInitialize();
        TimerDelg.Instance.PostInitialize();
        /*AbilityManager.Instance.PostInitialize();*/
    }
    public void Refresh()
    {
        PlayerManager.Instance.Refresh();
        EnemyManager.Instance.Refresh();
        TimerDelg.Instance.Refresh();
        /*AbilityManager.Instance.Refresh();*/
    }
    public void PhysicsRefresh()
    {
        PlayerManager.Instance.PhysicsRefresh();
        EnemyManager.Instance.PhysicsRefresh();
      /*  AbilityManager.Instance.PhysicsRefresh();*/
    }

}
