﻿using System.Collections;
using UnityEngine;


    public class PlayerEconomy : MonoBehaviour
    {
        static PlayerEconomy singleton;
        public static PlayerEconomy Singleton
        {
            get
            {
                if (singleton == null) singleton = FindObjectOfType<PlayerEconomy>();
                return singleton;
            }
        }
        int playerMoney;
        public bool ChangePlayerMoney(int amount)
        {
            if (playerMoney + amount < 0) return false;
            //Past the fail state, change money
            playerMoney += amount;
            return true;
        }
        public int GetPlayerMoney()
        {
            return playerMoney;
        }
    }