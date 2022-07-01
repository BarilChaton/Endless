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
        [HideInInspector] public int fontSize = 36;
        [HideInInspector] public bool GameStarted = false;
        [HideInInspector] public bool UpdateSettings = false;
        [HideInInspector] PlayerCombat player;

        [Header("Health")]
        [SerializeField] Image healthBarCore;
        [SerializeField] Image healthBarTip;
        [SerializeField] Image healthBarBackground;
        [SerializeField] TextMeshProUGUI HpText;
        [HideInInspector] private TextMeshProUGUI ErrorText = new TextMeshProUGUI();
        [HideInInspector] private string errorText = "Error: Something broke when creating the UI.\nPlease check the Canvas properties!";

        [Header("Armour")]
        [SerializeField] Image armourBarCore;
        [SerializeField] Image armourBarTip;
        [SerializeField] Image armourBarBackground;
        [SerializeField] TextMeshProUGUI ArmourText;


        private void Awake()
        {
            // Create Healthbars
            try
            {
                Instantiate(healthBarBackground, transform);
                Instantiate(healthBarCore, transform);
                Instantiate(healthBarTip, transform);
                Instantiate(HpText, transform);
                HpText.fontSize = fontSize;
                HpText.color = Color.white;

                // Create Armourbars
                Instantiate(armourBarBackground, transform);
                Instantiate(armourBarCore, transform);
                Instantiate(armourBarTip, transform);
                Instantiate(ArmourText, transform);
                ArmourText.fontSize = fontSize;
                ArmourText.color = Color.white;
            }
            catch
            {
                Instantiate(ErrorText, transform);
                ErrorText.text = errorText;
                ErrorText.color = Color.red;
            }
        }

        private void Update()
        {
            healthBarCore.fillAmount = player.SetHealthBar();
            if (HpText != null)
            {
                HpText.text = player.SetHealthBar() + " / 100";
            }
        }
    }
}
