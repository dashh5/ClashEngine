using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProperty : MonoBehaviour
{
    
    public static int goldenPlayer = 45000;
    public static int extractPlayer = 50000;

    public static int maxGoldenPlayer = 50000;
    public static int maxExtractPlayer = 100000;


    CameraController cameraController;
    BuildingsMenu buildingMenu;

    private void Start()
    {
        cameraController = FindObjectOfType<CameraController>();
        buildingMenu = FindObjectOfType<BuildingsMenu>();

        CountResources();
    }


    public void CountResources()
    {
        buildingMenu.GoldenSlider.size = ExtensionMethods.Remap(goldenPlayer, 0, maxGoldenPlayer, 0, 1);
        buildingMenu.ExtractSlider.size = ExtensionMethods.Remap(extractPlayer, 0, maxExtractPlayer, 0, 1);

        buildingMenu.GoldenSliderText.text = (goldenPlayer.ToString() + '/' + maxGoldenPlayer.ToString()).ToString();
        buildingMenu.ExtractSliderText.text = (extractPlayer.ToString() + '/' + maxExtractPlayer.ToString()).ToString();

        buildingMenu.GoldenSlider.enabled = false;
        buildingMenu.ExtractSlider.enabled = false;
    }

}
