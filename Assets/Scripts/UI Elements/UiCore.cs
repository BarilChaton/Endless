using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Endless.PlayerCore;
using TMPro;

namespace Endless.InterfaceCore
{
    public class UiCore : MonoBehaviour
    {
        [HideInInspector] public int fontSize = 18;
        [HideInInspector] public bool GameStarted = false;
        [HideInInspector] public bool UpdateSettings = false;
        [HideInInspector] PlayerCombat player;

        [Header("Health")]
        [SerializeField] GameObject healthBar;
        TextMeshProUGUI HpText;
        [HideInInspector] private TextMeshProUGUI ErrorText;
        [HideInInspector] private string errorText = "Error: Something broke when creating the UI.\nPlease check the Canvas properties!";

        [Header("Armour")]
        [SerializeField] GameObject armourBar;
        TextMeshProUGUI ArmourText;


        private void Start()
        {
            player = GameObject.Find("Player").GetComponent<PlayerCombat>();

            try
            {
                // Creating Health bars
                Instantiate(healthBar, transform);
                HpText = GameObject.Find("HpText").GetComponent<TextMeshProUGUI>();
                HpText.fontSize = fontSize;
                HpText.color = Color.white;

                // Creating Armour bars
                armourBar = Instantiate(armourBar, transform);
                ArmourText = GameObject.Find("ArmourText").GetComponent<TextMeshProUGUI>();
                ArmourText.fontSize = fontSize;
                ArmourText.color = Color.blue;
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
                HpText.text = System.Math.Round(player.SetHealthBar(), 0) + " / 100";
            }

            // Armour updates
            if (player.SetArmourBar() <= 1f) armourBar.transform.localScale = Vector3.zero;
            else
            {
                armourBar.transform.localScale = Vector3.one;
                ArmourText.text = System.Math.Round(player.SetArmourBar(), 0) + " / 100";
            }
        }
    }
}
