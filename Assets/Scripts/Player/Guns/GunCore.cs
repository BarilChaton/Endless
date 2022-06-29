using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Endless.Control;

namespace Endless.PlayerCore
{
    public class GunCore : MonoBehaviour
    {
        [Header("Shooty Stuff")]
        [SerializeField] public float DamageAmt;
        [SerializeField] public float ShotCooldown = 1f;
        [HideInInspector] public float CurrentCD;
        [Header("Ammunation")]
        [SerializeField] public int MaxAmmo;
        [HideInInspector] public int AmmoPerClip;
        [SerializeField] public int CurrentTotalAmmo;
        [HideInInspector] public int CurrentAmmo;
        [Header("Art stuff")]
        [SerializeField] public GameObject TempBulletImpact;
        public Animator gunAnim;


        public void ShootGun(Camera playerCamera = null)
        {
            if (playerCamera != null)
            {
                if (CurrentTotalAmmo > 0)
                {
                    Ray ray = playerCamera.ViewportPointToRay(new Vector3(.5f, .5f, 0));

                    // Checking if it hit anything
                    if (Physics.Raycast(ray, out RaycastHit hit))
                    {
                        //Debug.Log("I'm shooting at" + hit.transform.name);

                        // Hitting an enemy specifically
                        if (hit.transform.tag == "Enemy")
                        {
                            float damage = DamageAmt;
                            hit.collider.GetComponent<EnemyCore>().TakeDamage(damage);
                        }

                        // Hitting anything else. TODO: Hitting decoration? Lamps? -- TBI
                        else
                        {
                            Instantiate(TempBulletImpact, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
                        }
                    }

                    // Hit fuckall
                    else
                    {
                        Debug.Log("I'm looking at nothing");
                    }

                    // Removing ammo and doing animations
                    CurrentTotalAmmo--;
                    gunAnim.SetTrigger("ShootTrigger");
                }
            }
        }

        public void ShootMachineGun(Camera playerCamera = null)
        {

            // ratatatatatattatatata

        }
    }
}