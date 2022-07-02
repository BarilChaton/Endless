using UnityEngine;
using Endless.PlayerCore;
using TMPro;
using Endless.GunSwap;
using System.Collections.Generic;

namespace Endless.InterfaceCore
{
    public class UiCore : MonoBehaviour
    {
        [HideInInspector] public int fontSizeUI = 18;
        [HideInInspector] public bool GameStarted = false;
        [HideInInspector] public bool UpdateSettings = false;
        [HideInInspector] PlayerCombat playerStats;
        [HideInInspector] WeaponSwapper playerGun;
        [HideInInspector] GameObject player;

        [Header("Health")]
        [SerializeField] GameObject healthBar;
        TextMeshProUGUI HpText;
        [HideInInspector] private TextMeshProUGUI ErrorText;
        [HideInInspector] private string errorText = "Error: Something broke when creating the UI.\nPlease check the Canvas properties!";

        [Header("Armour")]
        [SerializeField] GameObject armourBar;
        TextMeshProUGUI ArmourText;

        private void Awake()
        {
            player = GameObject.Find("Player");
            playerStats = player.GetComponent<PlayerCombat>();
            playerGun = player.GetComponent<WeaponSwapper>();

            try
            {
                // Creating Health bars
                healthBar = Instantiate(healthBar, transform);
                HpText = GameObject.Find("HpText").GetComponent<TextMeshProUGUI>();
                HpText.fontSize = fontSizeUI;
                HpText.color = Color.white;

                // Creating Armour bars
                armourBar = Instantiate(armourBar, transform);
                ArmourText = GameObject.Find("ArmourText").GetComponent<TextMeshProUGUI>();
                ArmourText.fontSize = fontSizeUI;
                ArmourText.color = Color.blue;

                // Creating Gun
            }
            catch
            {
                ErrorText = gameObject.AddComponent<TextMeshProUGUI>();
                Instantiate(ErrorText, transform);
                ErrorText.text = errorText;
                ErrorText.color = Color.red;
            }
        }

        private void Update()
        {
            // HP updates
            if (HpText != null)
            {
                HpText.text = System.Math.Round(playerStats.SetHealthBar(), 0) + " / 100";
            }

            // Armour updates
            if (playerStats.SetArmourBar() < 1) armourBar.transform.localScale = Vector3.zero;
            else
            {
                armourBar.transform.localScale = Vector3.one;
                ArmourText.text = System.Math.Round(playerStats.SetArmourBar(), 0) + " / 100";
            }
        }

        public static void WeaponActivator(string gunName)
        {

        }
    }
}
