using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Endless.PlayerCore;

public class ArmourBar : MonoBehaviour
{
    private Image armourBarImage;
    private PlayerCombat player;

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<PlayerCombat>();
        armourBarImage = GetComponent<Image>();
    }

    public void Update()
    {
        armourBarImage.fillAmount = player.SetArmourBar() / 100;
    }
}
