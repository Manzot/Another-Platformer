﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPersistence
{

    public static void SaveData(PlayerController playerController)
    {
        PlayerPrefs.SetFloat("x", playerController.playerData.Location.x);
        PlayerPrefs.SetFloat("y", playerController.playerData.Location.y);
       /* PlayerPrefs.SetFloat("health", playerController.playerData.Health);*/


    }
    public static PlayerData LoadData(PlayerController playerController)
    {
        float x = PlayerPrefs.GetFloat("x");
        float y = PlayerPrefs.GetFloat("y");
        float health = PlayerPrefs.GetFloat("health");

        PlayerData playerData = new PlayerData()
        {
            Location = new Vector2(x, y),
            /*Health = health*/
        };
        return playerData;
    }
}


