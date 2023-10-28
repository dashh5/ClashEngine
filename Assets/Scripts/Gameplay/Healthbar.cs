using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// The 'Healthbar' class in Unity is responsible for managing and displaying the 
// health bar of a building in a game. It ensures that the health bar reflects the
// building’s current health status, scales appropriately, and remains oriented towards
// the camera. The class interacts with various other components, such as 
// 'BuildingProperties' and 'StartAttack', to function correctly and react to changes
// in the building's health.

public class Healthbar : MonoBehaviour
{

    public Transform scale;

    BuildingProperties buildingProperties;

    int maxHP;

    StartAttack startAttack;

 // Method to initialize components and properties at the start of the game


    private void Start()
    {
        startAttack = FindObjectOfType<StartAttack>();

        buildingProperties = transform.parent.GetComponent<BuildingProperties>();
        maxHP = buildingProperties.HP;
    }

    // Method to update the health bar each frame

    void Update()
    {
       // Checking if the building's health drops to 0 or below

        this.transform.LookAt(Camera.main.transform);
        scale.localScale = new Vector3(Mathf.Round(buildingProperties.HP).Remap(0, maxHP, 1, 23), 1, 1);
        if (buildingProperties.HP <= 0)
        {
            // Removing the building from the list of all buildings

            startAttack.allbuildings.Remove(transform.parent.transform);

            // Removing the building from the fence list if it is a fence


            if (buildingProperties.type == BuildingProperties.BuildingType.Fence)
                startAttack.fence.Remove(transform.parent.transform);

                // Destroying the building GameObject

            Destroy(transform.parent.gameObject);
        }
    }
}
