using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Endless.InterfaceCore;

namespace Endless.PlayerCore
{
    public class PlayerCombat : MonoBehaviour
    {
        [SerializeField] int maxHp = 100;
        [SerializeField] TextMeshProUGUI playerHpText;
        public bool playerDead;
        public int playerCurrHp = 0;
        private UiCore uiCoreGetter;


        void Start()
        {
            // Main getters
            uiCoreGetter = GameObject.Find("UI Canvas").GetComponent<UiCore>();

            if (!uiCoreGetter.GameStarted)
            {
                playerDead = false;
                playerCurrHp = maxHp;
                SetHealthBar();
                uiCoreGetter.GameStarted = true;
            }

            // Settings Menu Changes
            playerHpText.fontSize = uiCoreGetter.fontSize;
        }

        void Update()
        {
            // Checking for updates in settings
            if (uiCoreGetter.UpdateSettings)
            {
                Start();
                uiCoreGetter.UpdateSettings = false;
            }


            if (playerHpText != null)
            {
                playerHpText.text = SetHealthBar() + " / 100";
            }
        }

        public void TakeDamage(int amount)
        {
            playerCurrHp -= amount;
            if (playerCurrHp <= 0)
            {
                GetComponent<PlayerController>().PlayerDead = true;
            }
        }

        private string SetHealthBar()
        {
            int healthPerc = (playerCurrHp * 200 + maxHp) / (maxHp * 2);
            return healthPerc.ToString();
        }
    }
}