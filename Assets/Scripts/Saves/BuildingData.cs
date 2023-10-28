using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The 'BuildingData' class provides a structure to store properties of multiple buildings.
//  It can hold their index, level, and position (x, y, z coordinates). 
// The class has a constructor that takes an array of 'BuildingProperties' and sets the 
// properties in the class based on this input. Additionally, there's a method 'SetArray'
// that allows resizing of the data arrays based on a provided length. This class is u
// seful for operations that require knowledge of the properties of multiple buildings, 
// possibly for saving/loading or other data manipulation tasks in a game environment.

[System.Serializable]
public class BuildingData 
{
    // Declare variables
    public int length;                                  // Store count of buildingProperties
    public int[] buildingIndex;                         // Array to store building indices
    public int[] level;                                 // Array to store building levels
    public float[][] position;                          // 2D Array to store building positions (x, y, z)

    // Constructor to initialize properties based on BuildingProperties array
    public BuildingData(BuildingProperties[] buildingProperties)
    {
        length = buildingProperties.Length;             // Set length to count of buildingProperties

        buildingIndex = new int[length];                // Initialize buildingIndex array
        level = new int[length];                        // Initialize level array
        position = new float[length][];                 // Initialize position 2D array

        for (int i = 0; i < length; i++)                // Iterate over each buildingProperty
        {
            buildingIndex[i] = buildingProperties[i].buildingIndex;  // Set building index
            level[i] = buildingProperties[i].level;                   // Set building level

            position[i] = new float[3];                               // Create float array for position
            position[i][0] = buildingProperties[i].transform.position.x;  // Set x-coordinate
            position[i][1] = buildingProperties[i].transform.position.y;  // Set y-coordinate
            position[i][2] = buildingProperties[i].transform.position.z;  // Set z-coordinate
        }
    }

    // Method to resize the data arrays based on provided length
    public void SetArray(int length)
    {
        buildingIndex = new int[length];                // Resize buildingIndex array
        level = new int[length];                        // Resize level array
        position = new float[length][];                 // Resize position 2D array
    }
}
