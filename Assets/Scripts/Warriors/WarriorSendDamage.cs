using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorSendDamage : MonoBehaviour
{

    public MainAI mainAI;

    public void SendDamage()
    {
        mainAI.SendDamage();
    }

    public void Dead()
    {
        mainAI.Dead();
    }
}
