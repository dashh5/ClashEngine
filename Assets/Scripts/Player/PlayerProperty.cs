// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class PlayerProperty : MonoBehaviour
// {
    
//     public static int coins = 0; // # of coins
//     public static int XP = 0; // # of XP

//     public static int maxGoldenPlayer = 50000;
//     public static int maxExtractPlayer = 100000;


//     CameraController cameraController;
//     BuildingsMenu buildingMenu;

//     private void Start()
//     {
//         cameraController = FindObjectOfType<CameraController>();
//         buildingMenu = FindObjectOfType<BuildingsMenu>();

//         CountResources();
//     }


//     public void CountResources()
//     {
//         buildingMenu.GoldenSlider.size = ExtensionMethods.Remap(coins, 0, maxGoldenPlayer, 0, 1);
//         buildingMenu.ExtractSlider.size = ExtensionMethods.Remap(XP, 0, maxExtractPlayer, 0, 1);

//         buildingMenu.GoldenSliderText.text = (coins.ToString() + '/' + maxGoldenPlayer.ToString()).ToString();
//         buildingMenu.ExtractSliderText.text = (XP.ToString() + '/' + maxExtractPlayer.ToString()).ToString();

//         buildingMenu.GoldenSlider.enabled = false;
//         buildingMenu.ExtractSlider.enabled = false;
        
        
        
//         // BuildingsMenu buildingsMenu = FindObjectOfType<BuildingsMenu>();
//         // buildingsMenu.CoinsText.text = coins.ToString();
//         // buildingsMenu.XPText.text = XP.ToString();
//         // //buildingMenu.GoldenSliderText.text = (coins.ToString() + '/' + maxGoldenPlayer.ToString()).ToString();

        
//     }
    
//     // public void Update()
//     //     {
//     //         CountResources();
//     //         buildingMenu.GoldenSliderText.text = (coins.ToString() + '/' + maxGoldenPlayer.ToString()).ToString();
//     //     }
// }
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProperty : MonoBehaviour
{
    public static int coins = 0; // # of coins
    public static int XP = 0; // # of XP
    public static int maxGoldenPlayer;
    public static int maxExtractPlayer;
    public static int coinsFromAbstience;
    private float lastLoginTime;

    CameraController cameraController;
    BuildingsMenu buildingMenu;
    void Start()
{
    buildingMenu = FindObjectOfType<BuildingsMenu>();
    lastLoginTime = PlayerPrefs.GetFloat("LastLoginTime", -1); // Load the last login time
    StartCoroutine(DailyLoginChecker());
    CountResources();
    
}

    // private void Start()
    // {
    //     cameraController = FindObjectOfType<CameraController>();
    //     buildingMenu = FindObjectOfType<BuildingsMenu>();

    //     CountResources();
    //     CheckDailyLogin();
    //     // Start a coroutine to increment coins and XP every second
    //     //StartCoroutine(CheckDailyLogin());
    // }

    public void CountResources()
    {
        
        buildingMenu.GoldenSlider.size = ExtensionMethods.Remap(100, 0, 100, 0, 1);
        buildingMenu.ExtractSlider.size = ExtensionMethods.Remap(100, 0, 100, 0, 1);

        buildingMenu.GoldenSliderText.text = coins.ToString();
        buildingMenu.ExtractSliderText.text = XP.ToString();

        buildingMenu.GoldenSlider.enabled = false;
        buildingMenu.ExtractSlider.enabled = false;
    }

    // IEnumerator IncrementResourcesOverTime()
    // {
    //     while(true)
    //     {
    //         yield return new WaitForSeconds(1); // wait for 1 second
    //         coins += 10; // increase coins by 10 every second
    //         XP += 20;    // increase XP by 20 every second

    //         // Update the displayed values
    //         CountResources();
    //     }
    // }
    //private float lastLoginTime; // Holds the last login time in seconds



IEnumerator DailyLoginChecker()
{
    while(true)
    {
        CheckDailyLogin();
        CountResources();
        yield return new WaitForSeconds(5); // Wait for 5 seconds before checking again
    }
}

void CheckDailyLogin()
{
    float currentTime = Time.time; // Get the current time in seconds since the start of the game

    if (lastLoginTime == -1) // First time playing
    {
        PlayerPrefs.SetInt("CurrentDayCount", 1); // Start with Day 1
        coins = 0;
        XP = 0;
    }
    else if (currentTime - lastLoginTime < 5) // Logged in within 5 seconds, do nothing
    {
        coins = PlayerPrefs.GetInt("Coins", 0); // Load coins
        XP = PlayerPrefs.GetInt("XP", 0);
    }
    else if (currentTime - lastLoginTime < 10) // Logged in within the next 5 seconds
    {
        int dayCount = PlayerPrefs.GetInt("CurrentDayCount", 1) + 1; // Increase day count
        PlayerPrefs.SetInt("CurrentDayCount", dayCount); // Update the day count

        // Calculate coins based on day count
       XP +=10; 
        if (dayCount <= 14)
            coins += 10 + (dayCount -1) * (5); // Add 5 coins for each of the first 14 days
        else
            coins += 75; // From Day 15 onwards, keep it 75
    }
    else // If more than 10 seconds passed without checking, relapse
    {
        coins /= 2; // Halve the coins

        if (coins < 0)
            coins = 0; // Ensure coins don't go negative

        PlayerPrefs.SetInt("CurrentDayCount", 1); // Reset to Day 1
    }

    // Store coins and last login time
    PlayerPrefs.SetInt("Coins", coins);
    lastLoginTime = currentTime; // Update the last login time for the next check
}

}
