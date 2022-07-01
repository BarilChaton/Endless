using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Endless.InterfaceCore;

namespace Endless.PlayerCore
{
    public class PlayerCombat : MonoBehaviour
    {
        [SerializeField] int maxHp = 100;
        [SerializeField] int maxArmour = 100;
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

        }

        void Update()
        {
            // Checking for updates in settings
            if (uiCoreGetter.UpdateSettings)
            {
                Start();
                uiCoreGetter.UpdateSettings = false;
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

        public float SetHealthBar()
        {
            float healthPerc = (playerCurrHp * 200 + maxHp) / (maxHp * 2);
            return healthPerc;
        }

        public float SetArmourBar()
        {
            float healthPerc = (playerCurrArmour * 200 + maxArmour) / (maxArmour * 2);
            return healthPerc;
        }
    }
}