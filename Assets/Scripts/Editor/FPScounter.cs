using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// The FPScounter script is a Unity MonoBehaviour that calculates and 
// displays the current FPS (frames per second) on the user interface. It 
// utilizes Unity's UI Text component to display the FPS. The script has two 
// serialized fields: fpsText, which is a reference to the Text component, and 
// hudRefreshRate, which defines how frequently the FPS should be updated in seconds.

// Define FPScounter class that inherits from MonoBehaviour

public class FPScounter : MonoBehaviour
{
        // Serialize private Text component for displaying FPS on UI

    [SerializeField] private Text fpsText;
    [SerializeField] private float hudRefreshRate = 1f;

    // Private float to store timer for next FPS update

    private float timer;

    // Define Update method which is called once per frame


    private void Update()
    {
    
    // Check if the current unscaled time is greater than the timer

        if (Time.unscaledTime > timer)
        {

     // Calculate FPS based on the time taken to complete the last frame

            int fps = (int)(1f / Time.unscaledDeltaTime);

            // Update the fpsText UI element with the new FPS value

            fpsText.text = fps + " FPS";

            // Set the timer for the next FPS update

            timer = Time.unscaledTime + hudRefreshRate;
        }
    }
}
