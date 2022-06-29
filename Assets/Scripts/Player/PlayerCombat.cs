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
        [SerializeField] int maxArmour = 100;
        [SerializeField] TextMeshProUGUI playerHpText;
        public bool playerDead;
        public int playerCurrHp = 0;
        public int playerCurrArmour = 0;
        private UiCore uiCoreGetter;

        void Start()
        {
            // Main getters
            uiCoreGetter = GameObject.Find("UI Canvas").GetComponent<UiCore>();

            if (!uiCoreGetter.GameStarted)
            {
                playerDead = false;
                playerCurrHp = maxHp;
                playerCurrArmour = 0;
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

        public void PlayerTakeDamage(int amount)
        {
            playerCurrHp -= amount;
            playerCurrHp = Mathf.Clamp(playerCurrHp, 0, maxHp);
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