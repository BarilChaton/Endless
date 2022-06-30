using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Endless.PlayerCore;
using Endless.Control;
using System;

public class DamageEffects : MonoBehaviour
{
    [SerializeField] public bool canBePoisoned = true;
    [SerializeField] public bool isPoisoned = false;
    [HideInInspector] private float poisonTickSpeed = 0.5f;
    [HideInInspector] private float poisonDuration = 15;
    [HideInInspector] private float damageTickTemp = 0.5f;
    [HideInInspector] private float poisonDmgTemp = 0.5f;

    void Update()
    {
        if (isPoisoned)
        {
            Poisoned();
        }
    }

    public void Poisoned(float damage = 0f, float duration = 0f, float tickSpeed = 0f)
    {
        if (canBePoisoned)
        {
            // Setting values to repeat the attack through Update();
            poisonDmgTemp = damage; poisonTickSpeed = tickSpeed;
            if (duration < 60 && Time.time > 0)
            {
                poisonDuration = Time.time + duration;
            }

            // Should still be poisoned?
            if (poisonDuration > Time.time)
            {
                // Checking if it is time for the next tick
                if (Time.time < damageTickTemp)
                {
                    damageTickTemp = Time.time + poisonTickSpeed;
                    if (gameObject.tag == "Player") GetComponent<PlayerCombat>().PlayerTakeDamage(Convert.ToInt32(poisonDmgTemp));
                    if (gameObject.tag == "Enemy") GetComponent<EnemyCore>().TakeDamage(poisonDmgTemp);
                    print("Took damage");
                }

            }

            // Poison ran out
            else { isPoisoned = false; }
        }
        else print("Unit is immune");
    }
}
