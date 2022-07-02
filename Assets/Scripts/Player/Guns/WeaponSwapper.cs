using System.Collections.Generic;
using UnityEngine;
using Endless.PlayerCore;

namespace Endless.GunSwap
{
    public class WeaponSwapper : MonoBehaviour
    {
        [SerializeField] int maxInInventory = 5;
        [SerializeField] public GameObject defaultGun;
        [SerializeField] List<GameObject> gunsInMap;

        [HideInInspector] public int weaponChoice = 0;
        [HideInInspector] public int currTotalGuns = 1;
        [HideInInspector] public List<GameObject> gunsInInventory;
        [HideInInspector] public GameObject currentGun;

        private void Awake()
        {
            currTotalGuns = 1;
            weaponChoice = 0;

            gunsInMap.Insert(0, defaultGun);
            gunsInInventory.Add(defaultGun);
            gunsInInventory.Add(gunsInMap[1]);

            LoadGuns(); 
            ActiveGun();
            currentGun = gunsInInventory[0];
        }

        public void GunSwap(int swap)
        {
            try
            {
                weaponChoice += swap;
                if (weaponChoice < 0) weaponChoice = gunsInInventory.Count - 1;
                else if (weaponChoice >= gunsInInventory.Count) weaponChoice = 0;

                currentGun = gunsInInventory[weaponChoice];
            }

            // Broke? Reset
            catch
            {
                weaponChoice = 0;
                currentGun = defaultGun;
            }
            ActiveGun();
        }

        public void LoadGuns()
        {
            for (int i = 0; i < gunsInInventory.Count; i++)
            {
                gunsInInventory[i] = Instantiate(gunsInInventory[i], GameObject.Find("UI Canvas").transform);
                gunsInInventory[i].SetActive(false);
            }
        }

        public void ActiveGun()
        {
            for (int i = 0; i < gunsInInventory.Count; i++)
            {
                if (i == weaponChoice) gunsInInventory[i].SetActive(true);
                else gunsInInventory[i].SetActive(false);
            }
        }

        private void AddGunToInventory(int pos, string gunToAdd)
        {
            GameObject gunAdder = gunsInMap.Find(x => x.name == gunToAdd);
            if (gunAdder != null)
            {
                if (currTotalGuns > maxInInventory)
                {
                    gunsInInventory[pos] = gunAdder;
                }
                else
                {
                    gunsInInventory[pos] = Instantiate(gunAdder, GameObject.Find("UI Canvas").transform);
                    gunsInInventory[currTotalGuns] = gunAdder;
                    currTotalGuns++;
                }
            }
        }
    }
}
