using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Endless.CooldownCore
{
    public class Cooldown : MonoBehaviour
    {
        public static float CdCalc(float cooldown)
        {
            return Time.time + cooldown;
        }
    }
}