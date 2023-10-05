using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BuildingData 
{
    public int length;
    public int[] buildingIndex;
    public int[] level;
    public float[][] position;

    public BuildingData(BuildingProperties[] buildingProperties)
    {
        length = buildingProperties.Length;

        buildingIndex = new int[length];
        level = new int[length];
        position = new float[length][];

        for (int i = 0; i < length; i++)
        {
            buildingIndex[i] = buildingProperties[i].buildingIndex;
            level[i] = buildingProperties[i].level;

            position[i] = new float[3];
            position[i][0] = buildingProperties[i].transform.position.x;
            position[i][1] = buildingProperties[i].transform.position.y;
            position[i][2] = buildingProperties[i].transform.position.z;
        }
    }

    public void SetArray(int length)
    {
        buildingIndex = new int[length];
        level = new int[length];
        position = new float[length][];
    }
    
}
