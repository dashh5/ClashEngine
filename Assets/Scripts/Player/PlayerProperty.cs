using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The PlayerProperty class is a Unity script designed to manage and display 
// the player's resources in a strategy or building game. The resources managed
// by this class are "goldenPlayer" and "extractPlayer," which represent two types 
// of in-game currency or resources. The class keeps track of the current amount of
// each resource, as well as their maximum allowed values

public class PlayerProperty : MonoBehaviour
{
    
    // Declaring public static variables for player's resources and their maximum values

    public static int goldenPlayer = 45000;
    public static int extractPlayer = 50000;

    public static int maxGoldenPlayer = 50000;
    public static int maxExtractPlayer = 100000;


    CameraController cameraController;
    BuildingsMenu buildingMenu;

    // Method to initialize components and count resources at the start of the game

    private void Start()
    {
        cameraController = FindObjectOfType<CameraController>();
        buildingMenu = FindObjectOfType<BuildingsMenu>();

        CountResources();
    }


// Method to update the UI based on the current resource values

    public void CountResources()
    {
        
        // Updating the golden resource slider
        buildingMenu.GoldenSlider.size = ExtensionMethods.Remap(goldenPlayer, 0, maxGoldenPlayer, 0, 1);
        buildingMenu.ExtractSlider.size = ExtensionMethods.Remap(extractPlayer, 0, maxExtractPlayer, 0, 1);

        // Updating the extract resource slider

        buildingMenu.GoldenSliderText.text = (goldenPlayer.ToString() + '/' + maxGoldenPlayer.ToString()).ToString();
        buildingMenu.ExtractSliderText.text = (extractPlayer.ToString() + '/' + maxExtractPlayer.ToString()).ToString();

        buildingMenu.GoldenSlider.enabled = false;
        buildingMenu.ExtractSlider.enabled = false;
    }

}
