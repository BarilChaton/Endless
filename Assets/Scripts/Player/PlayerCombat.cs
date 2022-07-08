using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Endless.InterfaceCore;
using System;

namespace Endless.PlayerCore
{
    public class PlayerCombat : MonoBehaviour
    {
        public static float maxHp = 100;
        public static float maxArmour = 100;
        public bool playerDead;
        public float playerCurrHp = 0;
        public float playerCurrArmour = 0;
        private UiCore uiCoreGetter;

        [Header("Sound during combat.")]
        private AudioSource audioSource;
        public AudioClip hurt;
        public AudioClip die;

        void Start()
        {
            // Audio
            audioSource = GetComponent<AudioSource>();

            // Main getters
            uiCoreGetter = GameObject.Find("UI Canvas").GetComponent<UiCore>();

            if (!uiCoreGetter.GameStarted)
            {
                playerDead = false;
                playerCurrHp = maxHp;
                playerCurrArmour = 0;
                SetHealthBar();
                GetComponent<PlayerController>().gameObject.SetActive(true);
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

        public void PlayerGetArmour(float amount)
        {
            playerCurrArmour = Mathf.Clamp(playerCurrArmour + amount, 0, maxArmour);
        }

        public void PlayerHealed(float amount)
        {
            playerCurrHp = Mathf.Clamp(playerCurrHp + amount, 0, maxHp);
        }

        public void PlayerTakeDamage(float amount)
        {
            // Armour goes first, obviously
            if (playerCurrArmour > 0)
            {
                playerCurrArmour -= amount;
                if (playerCurrArmour < 0)
                {
                    amount = playerCurrArmour;
                    playerCurrArmour = 0;
                }
                else amount = 0;
            }
            // Then, health
            playerCurrHp = Mathf.Clamp(playerCurrHp - amount, 0f, maxHp);
            audioSource.PlayOneShot(hurt);

            // Player Dead stuffs!
            if (playerCurrHp <= 0)
            {
                AudioSource.PlayClipAtPoint(die, transform.position); //Need to play this when dying.
                Camera.main.transform.parent = null;
                GetComponent<PlayerController>().gameObject.SetActive(false);
                foreach (Transform child in GameObject.Find("UI Canvas").transform)
                {
                    Destroy(child.gameObject);
                }
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                GameObject deathscr = GetComponent<PlayerController>().deathScreen;
                deathscr = Instantiate(deathscr, transform.position + transform.forward * 2, transform.rotation);
                deathscr.transform.position = new Vector3(deathscr.transform.position.x, deathscr.transform.position.y + 0.5f, deathscr.transform.position.z);
            }
        }

        public float SetHealthBar()
        {
            float healthPerc = playerCurrHp / maxHp * 100;
            return healthPerc;
        }

        public float SetArmourBar()
        {
            float armourPerc = playerCurrArmour / maxArmour * 100;
            return armourPerc;
        }
    }
}