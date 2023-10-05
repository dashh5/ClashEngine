using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FenceGenerator : MonoBehaviour
{
    private Transform newRoad, newRoadType, newRoadSaved;

    public List<Transform> allRoads = new List<Transform>();
    public Transform[] nearRoads = new Transform[4];
    public Transform[] nearBuildings = new Transform[8];
    public List<BuildingProperties> nearRoadsProperties = new List<BuildingProperties>();
    BuildingProperties newRoadSave;

    public Transform roadZero, roadOne, roadTwo, roadTwoTurn, roadThree, roadFour;

    bool top, down, left, right;
    int countWays;
    int roadRotationY;

    BuildingsMenu buildingsMenu;
    CameraController cameraController;
    StartAttack startAttack;

    private void Awake()
    {
        nearRoads = new Transform[4];
        nearBuildings = new Transform[8];
        buildingsMenu = FindObjectOfType<BuildingsMenu>();
        cameraController = FindObjectOfType<CameraController>();
        startAttack = FindObjectOfType<StartAttack>();
    }

    public bool CheckFenceType(Transform road)
    {
        top = down = right = left = false;
        countWays = 0;
        roadRotationY = 0;
        for (int i = 0; i < nearRoads.Length; i++)
            nearRoads[i] = null;
        for (int i = 0; i < nearBuildings.Length; i++)
            nearBuildings[i] = null;
        nearRoadsProperties.Clear();

        //find another roads
        for (int i = 0; i < allRoads.Count; i++)
        {
            if (Mathf.Round(allRoads[i].position.x) == Mathf.Round(road.position.x) && Mathf.Round(allRoads[i].position.z) == Mathf.Round(road.position.z - 1))
            {
                top = true;
                nearRoads[0] = allRoads[i];
            }
            if (Mathf.Round(allRoads[i].position.x) == Mathf.Round(road.position.x) && Mathf.Round(allRoads[i].position.z) == Mathf.Round(road.position.z + 1))
            {
                down = true;
                nearRoads[1] = allRoads[i];
            }
            if (Mathf.Round(allRoads[i].position.x) - 1 == Mathf.Round(road.position.x) && Mathf.Round(allRoads[i].position.z) == Mathf.Round(road.position.z))
            {
                right = true;
                nearRoads[2] = allRoads[i];
            }
            if (Mathf.Round(allRoads[i].position.x) + 1 == Mathf.Round(road.position.x) && Mathf.Round(allRoads[i].position.z) == Mathf.Round(road.position.z))
            {
                left = true;
                nearRoads[3] = allRoads[i];
            }
            if (Mathf.Round(allRoads[i].position.x) == Mathf.Round(road.position.x) && allRoads[i].position.z == Mathf.Round(road.position.z))
            {
                return false;
            }
        }

        for (int i = 0; i < cameraController.allBuildings.Count; i++)
        {
            if (Mathf.Round(cameraController.allBuildings[i].transform.position.x) == Mathf.Round(road.position.x) &&
                        Mathf.Round(cameraController.allBuildings[i].transform.position.z) == Mathf.Round(road.position.z))
            {
                return false;
            }
        }

        RotateRoad();

        //final procedure
        newRoadType = newRoad;
        newRoad = Instantiate(newRoad, road.position, Quaternion.Euler(road.rotation.eulerAngles.x, roadRotationY, road.rotation.eulerAngles.z), cameraController.buildingsParent);

        BuildingProperties buildingProperties = newRoad.GetComponent<BuildingProperties>();
        buildingProperties.level = road.GetComponent<BuildingProperties>().level;
        for (int y = 0; y < buildingProperties.levels.Length; y++)
            buildingProperties.levels[y].SetActive(false);
        buildingProperties.levels[buildingProperties.level - 1].SetActive(true);

        newRoadSaved = newRoad;
        allRoads.Remove(road);
        allRoads.Add(newRoad);
        startAttack.fence.Add(newRoad);

        Destroy(road.gameObject);

        //repair near roads
        for (int i = 0; i < nearRoads.Length; i++)
            if (nearRoads[i] != null)
                CheckCreatedRoads(nearRoads[i], i);

        return true;
    }


    void RotateRoad()
    {
        //count near roads
        if (top) countWays += 1; if (down) countWays += 1; if (right) countWays += 1; if (left) countWays += 1;

        if (countWays == 0) newRoad = roadZero;
        if (countWays == 1) newRoad = roadOne;
        if (countWays == 2) newRoad = roadTwo;
        if (countWays == 3) newRoad = roadThree;
        if (countWays == 4) newRoad = roadFour;
        
        //choose road type
        if (newRoad == roadTwo)
        {
            if (top && down) newRoad = roadTwo;
            if (right && left) newRoad = roadTwo;

            if (right && down) newRoad = roadTwoTurn;
            if (left && down) newRoad = roadTwoTurn;
            if (top && right) newRoad = roadTwoTurn;
            if (top && left) newRoad = roadTwoTurn;
        }

        //rotation
        if (newRoad == roadOne)
        {
            if (right) roadRotationY = -90;
            if (left) roadRotationY = 90;
            if (down) roadRotationY = 180;
        }
        if (newRoad == roadTwo)
        {
            if (left && right) roadRotationY = 90;
        }
        if (newRoad == roadTwoTurn)
        {
            if (top && right) roadRotationY = 180;
            if (top && left) roadRotationY = -90;
            if (down && right) roadRotationY = 90;
            if (down && left) roadRotationY = 0;
        }
        if (newRoad == roadThree)
        {
            if (top && down && right) roadRotationY = 90;
            if (top && down && left) roadRotationY = -90;
            if (top && right && left) roadRotationY = 180;
            if (down && right && left) roadRotationY = 0;
        }
    }

    void CheckCreatedRoads(Transform road, int u)
    {
        top = down = right = left = false;
        countWays = 0;
        roadRotationY = 0;

        //find another roads
        for (int i = 0; i < allRoads.Count; i++)
        {
            if (Mathf.Round(allRoads[i].transform.position.x) == Mathf.Round(road.position.x) && Mathf.Round(allRoads[i].transform.position.z) == Mathf.Round(road.position.z - 1))
            {
                top = true;
                nearRoadsProperties.Add(allRoads[i].GetComponent<BuildingProperties>());
            }
            if (Mathf.Round(allRoads[i].transform.position.x) == Mathf.Round(road.position.x) && Mathf.Round(allRoads[i].transform.position.z) == Mathf.Round(road.position.z + 1))
            {
                down = true;
                nearRoadsProperties.Add(allRoads[i].GetComponent<BuildingProperties>());
            }
            if (Mathf.Round(allRoads[i].transform.position.x) - 1 == Mathf.Round(road.position.x) && Mathf.Round(allRoads[i].transform.position.z) == Mathf.Round(road.position.z))
            {
                right = true;
                nearRoadsProperties.Add(allRoads[i].GetComponent<BuildingProperties>());
            }
            if (Mathf.Round(allRoads[i].transform.position.x) + 1 == Mathf.Round(road.position.x) && Mathf.Round(allRoads[i].transform.position.z) == Mathf.Round(road.position.z))
            {
                left = true;
                nearRoadsProperties.Add(allRoads[i].GetComponent<BuildingProperties>());
            }
        }

        RotateRoad();

        //final procedure
        newRoad = Instantiate(newRoad, road.position, Quaternion.Euler(road.rotation.eulerAngles.x, roadRotationY, road.rotation.eulerAngles.z), cameraController.buildingsParent);

        BuildingProperties buildingProperties = newRoad.GetComponent<BuildingProperties>();
        buildingProperties.level = road.GetComponent<BuildingProperties>().level;
        for (int y = 0; y < buildingProperties.levels.Length; y++)
            buildingProperties.levels[y].SetActive(false);
        buildingProperties.levels[buildingProperties.level - 1].SetActive(true);

        nearRoads[u] = newRoad;
        nearRoadsProperties.Add(newRoad.GetComponent<BuildingProperties>());

        allRoads.Remove(road);
        allRoads.Add(newRoad);

        startAttack.fence.Remove(road);
        startAttack.fence.Add(newRoad);

        Destroy(road.gameObject);
    }

    void CloseOfCloseRoads()
    {
        for (int i = 0; i < allRoads.Count; i++)
        {
            for (int y = 0; y < nearRoadsProperties.Count; y++)
            {

                Debug.Log(nearRoadsProperties[y].name);
                if (Mathf.Round(allRoads[i].position.x) == Mathf.Round(nearRoadsProperties[y].transform.position.x) && Mathf.Round(allRoads[i].position.z) == Mathf.Round(nearRoadsProperties[y].transform.position.z - 1))
                    if (nearRoadsProperties.Contains(allRoads[i].GetComponent<BuildingProperties>()) == false)
                        nearRoadsProperties.Add(allRoads[i].GetComponent<BuildingProperties>());
                if (Mathf.Round(allRoads[i].position.x) == Mathf.Round(nearRoadsProperties[y].transform.position.x) && Mathf.Round(allRoads[i].position.z) == Mathf.Round(nearRoadsProperties[y].transform.position.z + 1))
                    if (nearRoadsProperties.Contains(allRoads[i].GetComponent<BuildingProperties>()) == false)
                        nearRoadsProperties.Add(allRoads[i].GetComponent<BuildingProperties>());
                if (Mathf.Round(allRoads[i].position.x) - 1 == Mathf.Round(nearRoadsProperties[y].transform.position.x) && Mathf.Round(allRoads[i].position.z) == Mathf.Round(nearRoadsProperties[y].transform.position.z))
                    if (nearRoadsProperties.Contains(allRoads[i].GetComponent<BuildingProperties>()) == false)
                        nearRoadsProperties.Add(allRoads[i].GetComponent<BuildingProperties>());
                if (Mathf.Round(allRoads[i].position.x) + 1 == Mathf.Round(nearRoadsProperties[y].transform.position.x) && Mathf.Round(allRoads[i].position.z) == Mathf.Round(nearRoadsProperties[y].transform.position.z))
                    if (nearRoadsProperties.Contains(allRoads[i].GetComponent<BuildingProperties>()) == false)
                        nearRoadsProperties.Add(allRoads[i].GetComponent<BuildingProperties>());
            }
        }
    }


    public void DeleteRoad(Transform road)
    {
        top = down = right = left = false;
        countWays = 0;
        roadRotationY = 0;
        for (int i = 0; i < nearRoads.Length; i++)
            nearRoads[i] = null;
        for (int i = 0; i < nearBuildings.Length; i++)
            nearBuildings[i] = null;
        nearRoadsProperties.Clear();

        //find another roads
        for (int i = 0; i < allRoads.Count; i++)
        {
            if (Mathf.Round(allRoads[i].position.x) == Mathf.Round(road.position.x) && Mathf.Round(allRoads[i].position.z) == Mathf.Round(road.position.z - 1))
            {
                top = true;
                nearRoads[0] = allRoads[i];
            }
            if (Mathf.Round(allRoads[i].position.x) == Mathf.Round(road.position.x) && Mathf.Round(allRoads[i].position.z) == Mathf.Round(road.position.z + 1))
            {
                down = true;
                nearRoads[1] = allRoads[i];
            }
            if (Mathf.Round(allRoads[i].position.x) - 1 == Mathf.Round(road.position.x) && Mathf.Round(allRoads[i].position.z) == Mathf.Round(road.position.z))
            {
                right = true;
                nearRoads[2] = allRoads[i];
            }
            if (Mathf.Round(allRoads[i].position.x) + 1 == Mathf.Round(road.position.x) && Mathf.Round(allRoads[i].position.z) == Mathf.Round(road.position.z))
            {
                left = true;
                nearRoads[3] = allRoads[i];
            }
        }


        RotateRoad();

        //repair near roads
        for (int i = 0; i < nearRoads.Length; i++)
        {
            if (nearRoads[i] != null)
            {
                CheckCreatedRoads(nearRoads[i], i);
            }
        }
    }

}
