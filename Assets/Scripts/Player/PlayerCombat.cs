using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Endless.PlayerCore
{
    public class PlayerCombat : MonoBehaviour
    {
        [SerializeField] int maxHp = 100;
        [SerializeField] TextMeshProUGUI playerHpText;
        public bool playerDead;
        public int playerCurrHp = 0;

        void Start()
        {
            playerDead = false;
            playerCurrHp = maxHp;
            SetHealthBar();
        }

        void Update()
        {
            playerHpText.text = playerCurrHp.ToString();
        }

        public void TakeDamage(int amount)
        {
            playerCurrHp -= amount;
            if (playerCurrHp <= 0)
            {
                playerDead = true;
            }
        }

        private void SetHealthBar()
        {
            float healthPerc = playerCurrHp / maxHp;
        }
    }
}