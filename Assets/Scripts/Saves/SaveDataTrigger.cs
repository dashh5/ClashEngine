using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveDataTrigger : MonoBehaviour
{

    public BuildingProperties[] buildingsPropertiesForIndex;
    BuildingProperties[] buildingsPropertiesBuilded;

    CameraController cameraController;
    FenceGenerator fenceGenerator;
    StartAttack startAttack;

    public Transform forest;
    List<Transform> forestObj = new List<Transform>();

    private void Awake()
    {
        cameraController = FindObjectOfType<CameraController>();
        fenceGenerator = FindObjectOfType<FenceGenerator>();
        startAttack = FindObjectOfType<StartAttack>();

        for (int i = 0; i < forest.childCount; i++)
            forestObj.Add(forest.GetChild(i));

        for (int i = 0; i < buildingsPropertiesForIndex.Length; i++)
            buildingsPropertiesForIndex[i].buildingIndex = i;

        BuildingDataLoad();
    }

    public void BuildingDataSave()
    {
        buildingsPropertiesBuilded = new BuildingProperties[cameraController.buildingsParent.childCount];

        for (int i = 0; i < cameraController.buildingsParent.childCount; i++)
        {
            buildingsPropertiesBuilded[i] = cameraController.buildingsParent.GetChild(i).GetComponent<BuildingProperties>();
        }

        SaveSystem.SaveBuildings(buildingsPropertiesBuilded);
    }

    public void BuildingDataLoad()
    {
        BuildingData data = SaveSystem.LoadBuildings();
        if (data != null)
        {
            for (int i = 0; i < data.length; i++)
            {
                GameObject building = Instantiate(buildingsPropertiesForIndex[data.buildingIndex[i]].gameObject, new Vector3(0, 0, 0), Quaternion.identity, cameraController.buildingsParent);
                BuildingProperties buildingProperties = building.GetComponent<BuildingProperties>();

                if (buildingProperties.type != BuildingProperties.BuildingType.Fence &&
                    buildingProperties.type != BuildingProperties.BuildingType.Trap)
                {
                    startAttack.allbuildings.Add(building.transform);
                    cameraController.allBuildings.Add(building.transform);
                }

                for (int u = 0; u < buildingProperties.additionalSpace.Length; u++)
                    cameraController.allBuildings.Add(buildingProperties.additionalSpace[u]);

                buildingProperties.level = data.level[i];

                Vector3 position;
                position.x = data.position[i][0];
                position.y = data.position[i][1];
                position.z = data.position[i][2];
                building.transform.position = position;

                if (buildingProperties.boxCollider != null)
                    buildingProperties.boxCollider.enabled = true;

                for (int u = 0; u < buildingProperties.levels.Length; u++)
                    buildingProperties.levels[u].SetActive(false);
                buildingProperties.levels[buildingProperties.level - 1].SetActive(true);

                if (buildingProperties.type == BuildingProperties.BuildingType.Fence)
                    fenceGenerator.CheckFenceType(building.transform);

                //forest
                for (int y = 0; y < forestObj.Count; y++)
                {
                    if (forestObj[y].position.x == building.transform.position.x && forestObj[y].position.z == building.transform.position.z)
                    {
                        forestObj[y].gameObject.SetActive(false);
                    }

                    for (int u = 0; u < buildingProperties.additionalSpace.Length; u++)
                    {

                        if (forestObj[y].position.x == buildingProperties.additionalSpace[u].position.x && forestObj[y].position.z == buildingProperties.additionalSpace[u].position.z)
                        {
                            forestObj[y].gameObject.SetActive(false);
                        }
                    }
                }
            }
        }
        else
        {
            Debug.Log("Save not found");
        }
    }

}
