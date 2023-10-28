using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The 'HealthbarWarrior' class in Unity is designed to manage and display 
// the health bar of a warrior character in a game. This class handles the 
// positioning, scaling, and visibility of the health bar based on the warrior’s 
// current health. It communicates with several other components in the game to 
// function properly, including 'WarriorProperties', 'GunsController', and 'StartAttack'

public class HealthbarWarrior : MonoBehaviour
{

    public Transform scale;

    WarriorProperties warriorProperties;
    GunsController gunsController;

    int maxHP;

    StartAttack startAttack;


// Method to initialize components and properties at the start of the game

    private void Start()
    {
        startAttack = FindObjectOfType<StartAttack>();
        gunsController = FindObjectOfType<GunsController>();

        warriorProperties = transform.parent.GetComponent<WarriorProperties>();
        maxHP = warriorProperties.HP;
    }

// Method to update the health bar each frame

    void Update()
    {
        this.transform.LookAt(Camera.main.transform);
    
         // Scaling the health bar based on the warrior's current health

        scale.localScale = new Vector3(Mathf.Round(warriorProperties.HP).Remap(0, maxHP, 1, 23), 1, 1);
            
        // Destroying the health bar if the warrior's health drops to 0 or below

        if (warriorProperties.HP <= 0)
        {
            Destroy(gameObject);
        }
    }
}
