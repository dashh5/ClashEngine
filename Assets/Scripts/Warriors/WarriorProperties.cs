using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorProperties : MonoBehaviour
{

    public int warriorIndex = -1;

    public enum WarriorType
    {
        Orc = 0,
        Drago = 1,
        Gnom = 2,
        Magician = 3,
        Elf = 4,
        Golem = 5
    }
    public WarriorType type;


    public int level = 1;
    public int HP = 1000;
    public int damage = 0;
    public int cost;
    public int costUpgrade;
    public int range = 0;

    public int maxHP;
    public int maxDamage;

    public int buildingTime;

    public GameObject[] levels;

    public bool fly = false;
    public bool canAttackFly = false;
    public bool attackDefenceFirst = false;

    public GameObject healthBar;

    [HideInInspector]
    public int startHeath;

    public GameObject bullet;
    public Transform[] bulletSpawner;
    
    void Awake()
    {
        if (healthBar != null)
            healthBar.SetActive(false);

        CountProperties();
    }

    public void CountProperties()
    {

        if (type == WarriorType.Orc)
        {
            range = 2;
            maxHP = 600;
            maxDamage = 12;

            if (level == 1) HP = 400;
            else if (level == 2) HP = 600;

            if (level == 1) cost = 100;
            else if (level == 2) cost = 120;

            if (level == 1) costUpgrade = 6000;
            else if (level == 2) costUpgrade = -1;

            if (level == 1) damage = 10;
            else if (level == 2) damage = 12;

            if (level == 1) buildingTime = 5;
            else if (level == 2) buildingTime = 6;
        }

        if (type == WarriorType.Golem)
        {
            range = 2;
            maxHP = 1200;
            maxDamage = 14;

            if (level == 1) HP = 1000;
            else if (level == 2) HP = 1200;

            if (level == 1) cost = 300;
            else if (level == 2) cost = 320;

            if (level == 1) costUpgrade = 8000;
            else if (level == 2) costUpgrade = -1;

            if (level == 1) damage = 12;
            else if (level == 2) damage = 14;

            if (level == 1) buildingTime = 7;
            else if (level == 2) buildingTime = 8;
        }

        if (type == WarriorType.Gnom)
        {
            range = 2;
            maxHP = 350;
            maxDamage = 10;

            if (level == 1) HP = 300;
            else if (level == 2) HP = 350;

            if (level == 1) cost = 80;
            else if (level == 2) cost = 90;

            if (level == 1) costUpgrade = 4000;
            else if (level == 2) costUpgrade = -1;

            if (level == 1) damage = 8;
            else if (level == 2) damage = 10;

            if (level == 1) buildingTime = 4;
            else if (level == 2) buildingTime = 5;
        }

        if (type == WarriorType.Magician)
        {
            range = 5;
            maxHP = 250;
            maxDamage = 25;

            if (level == 1) HP = 200;
            else if (level == 2) HP = 250;

            if (level == 1) cost = 300;
            else if (level == 2) cost = 350;

            if (level == 1) costUpgrade = 7000;
            else if (level == 2) costUpgrade = -1;

            if (level == 1) damage = 20;
            else if (level == 2) damage = 25;

            if (level == 1) buildingTime = 6;
            else if (level == 2) buildingTime = 7;
        }

        if (type == WarriorType.Drago)
        {
            range = 5;
            maxHP = 700;
            maxDamage = 40;

            if (level == 1) HP = 600;
            else if (level == 2) HP = 700;

            if (level == 1) cost = 1000;
            else if (level == 2) cost = 1200;

            if (level == 1) costUpgrade = 12000;
            else if (level == 2) costUpgrade = -1;

            if (level == 1) damage = 35;
            else if (level == 2) damage = 40;

            if (level == 1) buildingTime = 8;
            else if (level == 2) buildingTime = 9;

            fly = true;
        }

        if (type == WarriorType.Elf)
        {
            range = 2;
            maxHP = 200;
            maxDamage = 40;

            if (level == 1) HP = 170;
            else if (level == 2) HP = 200;

            if (level == 1) cost = 300;
            else if (level == 2) cost = 330;

            if (level == 1) costUpgrade = 3000;
            else if (level == 2) costUpgrade = -1;

            if (level == 1) damage = 25;
            else if (level == 2) damage = 30;

            if (level == 1) buildingTime = 4;
            else if (level == 2) buildingTime = 5;
        }

        startHeath = HP;

    }
}
