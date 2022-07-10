using UnityEngine;
using Endless.PlayerCore;
using TMPro;
using Endless.GunSwap;

namespace Endless.InterfaceCore
{
    public class UiCore : MonoBehaviour
    {
        [HideInInspector] public int fontSizeUI = 18;
        [HideInInspector] public bool GameStarted = false;
        [HideInInspector] public bool UpdateSettings = false;
        [HideInInspector] PlayerCombat player;

        [Header("Health")]
        [SerializeField] GameObject healthBar;
        TextMeshProUGUI HpText;
        [SerializeField] TextMeshProUGUI AmmoText;
        [HideInInspector] private TextMeshProUGUI ErrorText;
        [HideInInspector] private string errorText = "Error: Something broke when creating the UI.\nPlease check the Canvas properties!";

        [Header("Armour")]
        [SerializeField] GameObject armourBar;
        TextMeshProUGUI ArmourText;
        private GunCore gc;

        private void Start()
        {
            player = GameObject.Find("Player").GetComponent<PlayerCombat>();

            try
            {
                // Creating Health bars
                healthBar = Instantiate(healthBar, transform);
                HpText = healthBar.GetComponentInChildren<TextMeshProUGUI>();
                HpText.fontSize = fontSizeUI;
                HpText.color = Color.white;


                // Creating Armour bars
                armourBar = Instantiate(armourBar, transform);
                ArmourText = armourBar.GetComponentInChildren<TextMeshProUGUI>();
                ArmourText.fontSize = fontSizeUI;
                ArmourText.color = Color.blue;

                // Ammo text
                AmmoText = Instantiate(AmmoText, transform);
                AmmoText.fontSize = fontSizeUI;
                AmmoText.color = Color.gray;
            }
            catch
            {
                ErrorText = gameObject.AddComponent<TextMeshProUGUI>();
                Instantiate(ErrorText, transform);
                ErrorText.text = errorText;
                ErrorText.color = Color.red;
            }
            Invoke(nameof(GetStuff), 0.5f);
        }

        private void GetStuff()
        {
            WeaponSwapper swapper = player.GetComponent<WeaponSwapper>();
            gc = swapper.gunsInInventory[swapper.weaponChoice].GetComponent<GunCore>();
        }

        private void Update()
        {
            // HP updates
            if (HpText != null)
            {
                HpText.text = System.Math.Round(player.SetHealthBar(), 0) + " / 100";
            }

            // Armour updates
            if (player.SetArmourBar() < 1) armourBar.SetActive(false);
            else
            {
                armourBar.SetActive(true);
                ArmourText.text = System.Math.Round(player.SetArmourBar(), 0) + " / 100";
            }

            try { AmmoText.text = "Ammo: " + gc.CurrentTotalAmmo.ToString() + " / " + gc.MaxAmmo.ToString(); }
            catch { Debug.Log("Gun not found yet"); }
        }
    }
}