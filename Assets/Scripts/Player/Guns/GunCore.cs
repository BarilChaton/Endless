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
        [Header("Gun Details")]
        [SerializeField] public bool isShotgun;
        [SerializeField] public int bulletsPerShot = 1;
        [Header("Art stuff")]
        [SerializeField] public GameObject TempBulletImpact;
        public Animator gunAnim;


        public void ShootGun(Camera playerCamera = null)
        {
            if (playerCamera != null)
            {
                if (CurrentTotalAmmo > 0)
                {
                    Ray[] rays;
                    if (isShotgun) rays = ShotgunStuff(playerCamera);
                    else rays = NotShotgunStuff(playerCamera);

                    // Checking if it hit anything
                    foreach (Ray r in rays)
                    {
                        if (Physics.Raycast(r, out RaycastHit hit))
                        {
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
                    }
                    // Removing ammo and doing animations
                    CurrentTotalAmmo--;
                    gunAnim.Play("Shoot");
                }
            }
        }

        public void ShootMachineGun(Camera playerCamera = null)
        {

            // ratatatatatattatatata

        }

        private Ray[] ShotgunStuff(Camera playerCamera = null)
        {
            Vector2[] points = new Vector2[5];
            GaussianDistribution gd = new GaussianDistribution(); // maybe send a Random state through the ctor? I don't really use Unity's random any more
            for (int i = 0; i < bulletsPerShot; i++)
            {
                points[i] = new Vector2(gd.Next(0f, 1f, -1f, 1f), gd.Next(0f, 1f, -1f, 1f));
            }

            Ray[] rays = new Ray[points.Length];
            for (int i = 0; i < points.Length; i++)
            {
                Vector3 p3d = new Vector3(points[i].x, points[i].y, 6f);
                rays[i] = new Ray(Vector3.zero, p3d.normalized);                
            }
            return rays;
        }

        private Ray[] NotShotgunStuff(Camera playerCamera = null)
        {
            Ray[] ray = new Ray[1];
            ray[0] = playerCamera.ViewportPointToRay(new Vector3(.5f, .5f, 0));
            return ray;
        }
    }
}