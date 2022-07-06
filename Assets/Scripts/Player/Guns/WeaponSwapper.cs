using System.Collections.Generic;
using UnityEngine;
using Endless.PlayerCore;

namespace Endless.GunSwap
{
    public class WeaponSwapper : MonoBehaviour
    {
        [SerializeField] int maxInInventory = 5;
        [SerializeField] public GameObject defaultGun;
        [SerializeField] public List<GameObject> gunsInMap;

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

        public void AddGunToInventory(string gunToAdd, int bullets, int pos = 0)
        {
            GameObject gunAdder = gunsInInventory.Find(x => x.name.Contains(gunToAdd));
            if (gunAdder == null)
            {
                gunAdder = gunsInMap.Find(x => x.name == gunToAdd);
                if (pos == 0 && currTotalGuns < maxInInventory)
                {
                    gunsInInventory.Add(gunAdder);
                    pos = currTotalGuns;
                }

                if (gunAdder != null)
                {
                    if (currTotalGuns < maxInInventory) currTotalGuns++;
                    gunsInInventory[pos] = Instantiate(gunsInInventory[pos], GameObject.Find("UI Canvas").transform);
                }
                weaponChoice = pos;
                GunSwap(0);
                gunsInInventory[pos].GetComponent<GunCore>().CurrentTotalAmmo = bullets;
            }
            else
            {
                pos = gunsInInventory.IndexOf(gunAdder);
                GunCore thisGun = gunsInInventory[pos].GetComponent<GunCore>();
                thisGun.CurrentTotalAmmo += bullets;
                if (thisGun.CurrentTotalAmmo > thisGun.MaxAmmo) thisGun.CurrentTotalAmmo = thisGun.MaxAmmo;
            }
        }
    }
}
