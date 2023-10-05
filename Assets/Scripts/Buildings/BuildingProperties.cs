using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BuildingProperties : MonoBehaviour
{

    public int buildingIndex = -1;

    public enum BuildingType
    {
        Infrastructure = 0,
        Fence = 1,
        Defence = 2,
        Trap = 3,
        DeleteTool = 4
    }
    public BuildingType type;

    public enum InfrastructureType
    {
        None = 0,
        Mine = 1,
        Barracks = 2,
        Bastion = 3,
        Boiler = 4,
        Coffer = 5,
        Craft = 6,
        Dril = 7,
        Farm = 8,
        Generator = 9,
        LordHouse = 10,
        MagicianHouse = 11,
        Repairs = 12,
        Well = 13
    }
    public InfrastructureType infrastructureType;

    public enum TrapType
    {
        None = 0,
        Bomb = 1,
        Grinder = 2,
        Board = 3,
        Axe = 4,
        Spikes = 5
    }
    public TrapType trapType;

    public enum DefenceType
    {
        None = 0,
        Cannon = 1,
        Energy = 2,
        MilitaryTower = 3,
        Mortar = 4,
        Shooter = 5,
        BombsLauncher = 6,
        Flamethrower = 7
    }
    public DefenceType defenceType;

    public int level = 1;
    public int HP = 100;
    public int damage = 0;
    public int cost;
    public int range = 0;
    public float attackSpeed = 0;
    public int buildingTime = 0;

    public int maxHP = 100;
    public int maxDamage = 100;

    public GameObject[] levels;

    public Transform[] additionalSpace;

    public BuildConstruction buildConstruction;
    public float buildingHigh = 1;

    public int spaceWidth = 1;

    public bool canAttackLand = true;
    public bool canAttackFly;

    public BoxCollider boxCollider;

    public GameObject healthBar;

    [HideInInspector]
    public int startHeath;

    public GameObject bullet;


    private void Start()
    {
        if(healthBar != null)
            healthBar.SetActive(false);

        CountProperties();
    }

    public void CountProperties()
    {
        //---------INFRASTRUCTURE
        if (type == BuildingType.Infrastructure)
        {
            if (infrastructureType == InfrastructureType.LordHouse)
            {
                maxHP = 5000;

                if (level == 1) HP = 1500;
                else if (level == 2) HP = 2500;
                else if (level == 3) HP = 3500;

                if (level == 1) cost = 1500;
                else if (level == 2) cost = 3400;
                else if (level == 3) cost = -1;

                if (level == 1) buildingTime = 10;
                else if (level == 2) buildingTime = 15;
                else if (level == 3) buildingTime = 20;
            }

            if (infrastructureType == InfrastructureType.Barracks)
            {
                maxHP = 700;

                if (level == 1) HP = 500;
                else if (level == 2) HP = 700;

                if (level == 1) cost = 3000;
                else if (level == 2) cost = -1;

                if (level == 1) buildingTime = 15;
                else if (level == 2) buildingTime = 25;
            }

            if (infrastructureType == InfrastructureType.Bastion)
            {
                maxHP = 750;

                if (level == 1) HP = 550;
                else if (level == 2) HP = 750;

                if (level == 1) cost = 3500;
                else if (level == 2) cost = -1;

                if (level == 1) buildingTime = 15;
                else if (level == 2) buildingTime = 25;
            }

            if (infrastructureType == InfrastructureType.Boiler)
            {
                maxHP = 450;

                if (level == 1) HP = 400;
                else if (level == 2) HP = 450;

                if (level == 1) cost = 5000;
                else if (level == 2) cost = -1;

                if (level == 1) buildingTime = 10;
                else if (level == 2) buildingTime = 15;
            }

            if (infrastructureType == InfrastructureType.Coffer)
            {
                maxHP = 400;

                if (level == 1) HP = 350;
                else if (level == 2) HP = 400;

                if (level == 1) cost = 2000;
                else if (level == 2) cost = -1;

                if (level == 1) buildingTime = 10;
                else if (level == 2) buildingTime = 15;
            }

            if (infrastructureType == InfrastructureType.Craft)
            {
                maxHP = 400;

                if (level == 1) HP = 350;
                else if (level == 2) HP = 400;

                if (level == 1) cost = 2000;
                else if (level == 2) cost = -1;

                if (level == 1) buildingTime = 10;
                else if (level == 2) buildingTime = 15;
            }

            if (infrastructureType == InfrastructureType.Dril)
            {
                maxHP = 450;

                if (level == 1) HP = 350;
                else if (level == 2) HP = 450;

                if (level == 1) cost = 3000;
                else if (level == 2) cost = -1;

                if (level == 1) buildingTime = 10;
                else if (level == 2) buildingTime = 15;
            }

            if (infrastructureType == InfrastructureType.Farm)
            {
                maxHP = 600;

                if (level == 1) HP = 500;
                else if (level == 2) HP = 600;

                if (level == 1) cost = 4000;
                else if (level == 2) cost = -1;

                if (level == 1) buildingTime = 13;
                else if (level == 2) buildingTime = 20;
            }

            if (infrastructureType == InfrastructureType.Generator)
            {
                maxHP = 500;

                if (level == 1) HP = 450;
                else if (level == 2) HP = 500;

                if (level == 1) cost = 2800;
                else if (level == 2) cost = -1;

                if (level == 1) buildingTime = 10;
                else if (level == 2) buildingTime = 15;
            }

            if (infrastructureType == InfrastructureType.MagicianHouse)
            {
                maxHP = 700;

                if (level == 1) HP = 450;
                else if (level == 2) HP = 500;
                else if (level == 3) HP = 700;

                if (level == 1) cost = 2800;
                else if (level == 2) cost = 3500;
                else if (level == 3) cost = -1;

                if (level == 1) buildingTime = 10;
                else if (level == 2) buildingTime = 13;
                else if (level == 3) buildingTime = 16;
            }

            if (infrastructureType == InfrastructureType.Mine)
            {
                maxHP = 500;

                if (level == 1) HP = 450;
                else if (level == 2) HP = 500;

                if (level == 1) cost = 3000;
                else if (level == 2) cost = -1;

                if (level == 1) buildingTime = 10;
                else if (level == 2) buildingTime = 15;
            }

            if (infrastructureType == InfrastructureType.Repairs)
            {
                maxHP = 500;

                if (level == 1) HP = 450;
                else if (level == 2) HP = 500;

                if (level == 1) cost = 2500;
                else if (level == 2) cost = -1;

                if (level == 1) buildingTime = 10;
                else if (level == 2) buildingTime = 15;
            }

            if (infrastructureType == InfrastructureType.Well)
            {
                maxHP = 450;

                if (level == 1) HP = 400;
                else if (level == 2) HP = 450;

                if (level == 1) cost = 2300;
                else if (level == 2) cost = -1;

                if (level == 1) buildingTime = 10;
                else if (level == 2) buildingTime = 15;
            }
        }

        //-------DEFENCE
        if (type == BuildingType.Defence)
        {
            if (defenceType == DefenceType.Cannon)
            {
                range = 10;
                maxHP = 600;
                maxDamage = 35;
                attackSpeed = 1;

                if (level == 1) HP = 400;
                else if (level == 2) HP = 600;

                if (level == 1) cost = 2000;
                else if (level == 2) cost = -1;

                if (level == 1) damage = 30;
                else if (level == 2) damage = 35;

                if (level == 1) buildingTime = 10;
                else if (level == 2) buildingTime = 15;
            }

            if (defenceType == DefenceType.Shooter)
            {
                range = 15;
                maxHP = 500;
                maxDamage = 40;
                attackSpeed = 0.3f;

                if (level == 1) HP = 300;
                else if (level == 2) HP = 500;

                if (level == 1) cost = 4000;
                else if (level == 2) cost = -1;

                if (level == 1) damage = 20;
                else if (level == 2) damage = 40;

                if (level == 1) buildingTime = 8;
                else if (level == 2) buildingTime = 12;
                
                canAttackFly = true;
            }

            if (defenceType == DefenceType.BombsLauncher)
            {
                range = 12;
                maxHP = 500;
                maxDamage = 70;
                attackSpeed = 3;

                if (level == 1) HP = 450;
                else if (level == 2) HP = 500;

                if (level == 1) cost = 3000;
                else if (level == 2) cost = -1;

                if (level == 1) damage = 70;
                else if (level == 2) damage = 80;

                if (level == 1) buildingTime = 12;
                else if (level == 2) buildingTime = 15;

                canAttackLand = false;
                canAttackFly = true;
            }

            if (defenceType == DefenceType.Energy)
            {
                range = 8;
                maxHP =  400;
                maxDamage = 30;
                attackSpeed = 2f;

                if (level == 1) HP = 350;
                else if (level == 2) HP = 400;

                if (level == 1) cost = 2500;
                else if (level == 2) cost = -1;

                if (level == 1) damage = 25;
                else if (level == 2) damage = 30;

                if (level == 1) buildingTime = 8;
                else if (level == 2) buildingTime = 10;

                canAttackFly = true;
            }

            if (defenceType == DefenceType.Mortar)
            {
                range = 10;
                maxHP = 800;
                maxDamage = 70;
                attackSpeed = 4;

                if (level == 1) HP = 700;
                else if (level == 2) HP = 800;

                if (level == 1) cost = 7000;
                else if (level == 2) cost = -1;

                if (level == 1) damage = 60;
                else if (level == 2) damage = 70;

                if (level == 1) buildingTime = 15;
                else if (level == 2) buildingTime = 20;
            }

            if (defenceType == DefenceType.MilitaryTower)
            {
                range = 11;
                maxHP = 800;
                maxDamage = 35;
                attackSpeed = 0.7f;

                if (level == 1) HP = 550;
                else if (level == 2) HP = 600;

                if (level == 1) cost = 5000;
                else if (level == 2) cost = -1;

                if (level == 1) damage = 30;
                else if (level == 2) damage = 35;

                if (level == 1) buildingTime = 15;
                else if (level == 2) buildingTime = 20;

                canAttackFly = true;
            }

            if (defenceType == DefenceType.Flamethrower)
            {
                range = 5;
                maxHP = 550;
                maxDamage = 5;
                attackSpeed = 0.5f;

                if (level == 1) HP = 550;

                if (level == 1) cost = -1;

                if (level == 1) damage = 5;

                if (level == 1) buildingTime = 10;
            }
        }

        //------FENCE
        if (type == BuildingType.Fence)
        {
            buildingTime = 1;
            maxHP = 1000;

            if (level == 1) HP = 300;
            else if (level == 2) HP = 500;
            else if (level == 3) HP = 1000;

            if (level == 1) cost = 1000;
            else if (level == 2) cost = 2000;
            else if (level == 3) cost = -1;
        }


        //------TRAP
        if (type == BuildingType.Trap)
        {
            if (trapType == TrapType.Bomb)
            {
                maxDamage = 400;

                if (level == 1) cost = 3000;
                if (level == 2) cost = -1;

                if (level == 1) damage = 400;
                if (level == 2) damage = 500;

                if (level == 1) buildingTime = 3;
                if (level == 2) buildingTime = 6;
            }

            if (trapType == TrapType.Axe)
            {
                maxDamage = 300;

                if (level == 1) cost = -1;

                if (level == 1) damage = 200;

                if (level == 1) buildingTime = 3;
            }

            if (trapType == TrapType.Spikes)
            {
                maxDamage = 800;

                if (level == 1) cost = -1;

                if (level == 1) damage = 800;

                if (level == 1) buildingTime = 3;
            }

            if (trapType == TrapType.Grinder)
            {
                maxDamage = 300;

                if (level == 1) cost = -1;

                if (level == 1) damage = 300;

                if (level == 1) buildingTime = 3;
            }

            if (trapType == TrapType.Board)
            {
                maxDamage = 600;

                if (level == 1) cost = -1;

                if (level == 1) damage = 600;

                if (level == 1) buildingTime = 3;
            }
        }


        startHeath = HP;
    }

}
