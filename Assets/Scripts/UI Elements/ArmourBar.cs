using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Endless.PlayerCore;

public class ArmourBar : MonoBehaviour
{
    [SerializeField] Image armourBarImage;
    private PlayerCombat player;

    void Update()
    {
        armourBarImage.fillAmount = player.SetArmourBar();
    }
}
