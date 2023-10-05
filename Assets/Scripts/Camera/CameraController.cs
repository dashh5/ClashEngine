using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    public Transform buildingsParent;

    public bool moveTarget = false;
    public Transform target;
    public bool targetIsWarrior;
    public int gridSize = 2;

    public Transform cameraHolder;
    public Transform cameraTransform;

    public float normalSpeed;
    public float fastSpeed;
    public float movSpeed;
    private Vector3 toPos;

    private Vector3 dragStartPos;
    private Vector3 dragTargetPos;

    public float rotationScale;
    private Vector3 rotateStartPosition;
    private Vector3 rotateTargetPosition;
    private Quaternion toRot;

    public Vector3 zoomScale;
    private Vector3 toZoom;
    public float minZoom;
    public float maxZoom;

    private BuildingsMenu buildingMenu;
    private FenceGenerator fenceGenerator;
    private ControlPanelProperties controlPanelProperties;
    private SaveDataTrigger saveDataTrigger;
    private StartAttack startAttack;
    private GunsController gunsController;
    private GunAttackRadius gunAttackRadius;

    public Transform forest;
    List<Transform> forestObj = new List<Transform>();

    public List<Transform> allBuildings = new List<Transform>();

    private bool dontBuild;

    public bool doubleClick;
    public float lastClickTime;
    float catchTime = 0.3f;

    Transform selectedBuildingMesh;
    [HideInInspector]
    public BuildingProperties selectedBuilding;

    bool findPositionAfterMuiltyIputs;

    [HideInInspector]
    public Vector2[] whereCanntSpawnWarriors;

    public Transform gunRadius;

    void Awake()
    {
        buildingMenu = FindObjectOfType<BuildingsMenu>();
        fenceGenerator = FindObjectOfType<FenceGenerator>();
        controlPanelProperties = FindObjectOfType<ControlPanelProperties>();
        saveDataTrigger = FindObjectOfType<SaveDataTrigger>();
        startAttack = FindObjectOfType<StartAttack>();
        gunsController = FindObjectOfType<GunsController>();
        gunAttackRadius = FindObjectOfType<GunAttackRadius>();

        toPos = cameraHolder.transform.position;
        toRot = cameraHolder.transform.rotation;
        toZoom = cameraTransform.localPosition;
        
        for (int i = 0; i < forest.childCount; i++)
            forestObj.Add(forest.GetChild(i)); 

    }
    
    void Update()
    {
        MouseInut();
        KeyboardInput();
        if(buildingMenu.activateMenu.activeSelf == false)
            SetPosition();

        if (Input.GetMouseButtonDown(0))
        {
            if (Time.time - lastClickTime < catchTime)
            {
                doubleClick = true;
            }
            else
            {
                doubleClick = false;
            }
            lastClickTime = Time.time;

            if (target == null && buildingMenu.activateMenu.activeSelf == false && buildingMenu.warriorsBuy.activeSelf == false && buildingMenu.buildingButtons.activeSelf == false)
            {
                //select building
                RaycastHit hitInfo = new RaycastHit();
                bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
                if (hit && hitInfo.transform.GetComponent<BuildingProperties>())
                {
                    gunAttackRadius.gameObject.SetActive(true);
                    gunAttackRadius.radius = hitInfo.transform.GetComponent<BuildingProperties>().range;
                    gunAttackRadius.GetComponent<GunAttackRadius>().CreatePoints();

                    gunRadius.transform.position = new Vector3(hitInfo.transform.position.x + 0.5f, hitInfo.transform.position.y + 0.1f, hitInfo.transform.position.z + 0.5f);
                    selectedBuilding = hitInfo.transform.GetComponent<BuildingProperties>();
                    selectedBuildingMesh = hitInfo.transform.GetComponent<BuildingProperties>().levels[hitInfo.transform.GetComponent<BuildingProperties>().level - 1].transform;
                    StartCoroutine(SelectBuildingScale());
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
            if (buildingMenu.buildingButtons.activeSelf)
                gunAttackRadius.gameObject.SetActive(false);

    }

    IEnumerator SelectBuildingScale()
    {
        bool returnScale = false;

        if (returnScale == false)
        {
            while (selectedBuildingMesh.localScale.x < 1.3f)
            {
                selectedBuildingMesh.localScale += new Vector3(0.1f, 0.1f, 0.1f);
                yield return new WaitForEndOfFrame();
            }
            returnScale = true;
        }
        if (returnScale)
        {
            while (selectedBuildingMesh.localScale.x > 1)
            {
                selectedBuildingMesh.localScale -= new Vector3(0.1f, 0.1f, 0.1f);
                yield return new WaitForEndOfFrame();
            }

            buildingMenu.buildingButtons.SetActive(true);
            yield return null;
        }

        yield return null;
    }

    private void LateUpdate()
    {
        
        if (moveTarget && target != null)
        {
            float planeY = 0;
            Plane plane = new Plane(Vector3.up, Vector3.up * planeY); // ground plane
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float distance; // the distance from the ray origin to the ray intersection of the plane
            if (plane.Raycast(ray, out distance))
            {
                target.position = Vector3.Lerp(target.position, new Vector3(Mathf.Floor((ray.GetPoint(distance).x + gridSize / 2) / gridSize) * gridSize,
                   0, Mathf.Floor((ray.GetPoint(distance).z + gridSize / 2) / gridSize) * gridSize), Time.deltaTime * 50);
            }
        }

        if (Input.GetMouseButtonUp(0))
            buildingMenu.buildingButtons.SetActive(false);
    }

    void OnGUI()
    {
        //spawn building
        if (doubleClick)
        {
            //spawn warrior to attack
            dontBuild = false;
            if (target != null && targetIsWarrior)
            {
                doubleClick = false;
                for (int i = 0; i < allBuildings.Count + fenceGenerator.allRoads.Count; i++)
                {
                    if (whereCanntSpawnWarriors[i].x == target.position.x &&
                        whereCanntSpawnWarriors[i].y == target.position.z)
                    {
                        dontBuild = true;
                        break;
                    }
                }

                if (target.position.x < 250)
                    dontBuild = true;
                if (target.position.x > 304)
                    dontBuild = true;
                if (target.position.z < 250)
                    dontBuild = true;
                if (target.position.z > 304)
                    dontBuild = true;

                if (dontBuild == false)
                {
                    Transform warrior = Instantiate(startAttack.army[startAttack.currentWarriorToSpawn].warrior, target.position, Quaternion.identity).transform;

                    startAttack.army[startAttack.currentWarriorToSpawn].amount -= 1;
                    if (startAttack.army[startAttack.currentWarriorToSpawn].amount <= 0)
                    {
                        startAttack.warriorsButtons[startAttack.currentWarriorToSpawn].SetActive(false);
                        Destroy(target.gameObject);
                    }
                    startAttack.warriorsTextCount[startAttack.currentWarriorToSpawn].text = startAttack.army[startAttack.currentWarriorToSpawn].amount.ToString();

                    if (warrior.GetComponent<WarriorProperties>().fly == false)
                        gunsController.landEnemies.Add(warrior);
                    else
                        gunsController.flyEnemies.Add(warrior);

                    startAttack.warriorsHealthBarsArray.Add(warrior.GetComponent<WarriorProperties>());
                    startAttack.createdWarriors.Add(warrior.gameObject);
                }

            }

            dontBuild = false;
            if (target != null && targetIsWarrior == false)
            {
                if (target.GetComponent<BuildingProperties>().type == BuildingProperties.BuildingType.Infrastructure ||
                    target.GetComponent<BuildingProperties>().type == BuildingProperties.BuildingType.Defence ||
                    target.GetComponent<BuildingProperties>().type == BuildingProperties.BuildingType.Trap)
                {
                    BuildingProperties targetBuildProp = target.GetComponent<BuildingProperties>();

                    doubleClick = false;
                    lastClickTime = 0;

                    target.parent = buildingsParent;
                    for (int i = 0; i < allBuildings.Count; i++)
                    {
                        if (allBuildings[i].position.x == target.position.x &&
                            allBuildings[i].position.z == target.position.z)
                        {
                            dontBuild = true;
                            break;
                        }

                        for (int u = 0; u < targetBuildProp.additionalSpace.Length; u++)
                        {
                            if (allBuildings[i].position.x == targetBuildProp.additionalSpace[u].position.x &&
                            allBuildings[i].position.z == targetBuildProp.additionalSpace[u].position.z)
                            {
                                dontBuild = true;
                                break;
                            }
                        }
                    }

                    for (int i = 0; i < fenceGenerator.allRoads.Count; i++)
                    {
                        if (fenceGenerator.allRoads[i].position.x == target.position.x &&
                            fenceGenerator.allRoads[i].position.z == target.position.z)
                        {
                            dontBuild = true;
                            break;
                        }

                        for (int u = 0; u < targetBuildProp.additionalSpace.Length; u++)
                        {
                            if (fenceGenerator.allRoads[i].position.x == targetBuildProp.additionalSpace[u].position.x &&
                            fenceGenerator.allRoads[i].position.z == targetBuildProp.additionalSpace[u].position.z)
                            {
                                dontBuild = true;
                                break;
                            }
                        }
                    }

                    if (target.position.x < 250)
                        dontBuild = true;
                    if (target.position.x > 304)
                        dontBuild = true;
                    if (target.position.z < 250)
                        dontBuild = true;
                    if (target.position.z > 304)
                        dontBuild = true;

                    if (dontBuild == false)
                    {
                        for (int i = 0; i < forestObj.Count; i++)
                        {
                            if (forestObj[i].position.x == target.position.x && forestObj[i].position.z == target.position.z)
                            {
                                forestObj[i].gameObject.SetActive(false);
                            }

                            for (int u = 0; u < targetBuildProp.additionalSpace.Length; u++)
                            {

                                if (forestObj[i].position.x == targetBuildProp.additionalSpace[u].position.x && forestObj[i].position.z == targetBuildProp.additionalSpace[u].position.z)
                                {
                                    forestObj[i].gameObject.SetActive(false);
                                }
                            }
                        }

                        allBuildings.Add(target);

                        for (int i = 0; i < targetBuildProp.additionalSpace.Length; i++)
                            allBuildings.Add(targetBuildProp.additionalSpace[i].transform);

                        if (target.GetComponent<BuildingProperties>().type != BuildingProperties.BuildingType.Trap)
                            startAttack.allbuildings.Add(target);

                        target.position = new Vector3(target.position.x, 0, target.position.z);

                        //building animation
                        BuildConstruction buildConstProp = targetBuildProp.buildConstruction.GetComponent<BuildConstruction>();
                        buildConstProp.buildingProperties = targetBuildProp;
                        buildConstProp.target = targetBuildProp;
                        buildConstProp.StartBuild();
                        buildConstProp.cameraController = this;

                        //menu
                        buildingMenu.grid.enabled = false;
                        moveTarget = false;
                        target = null;
                    }
                }

                else if (target.GetComponent<BuildingProperties>().type == BuildingProperties.BuildingType.Fence)
                {
                    target.parent = buildingsParent;
                    target.position = new Vector3((int)target.position.x, 0, (int)target.position.z);
                    bool roadSpawnWasSuccessful = true;

                    dontBuild = false;
                    if (target.position.x < 250)
                        dontBuild = true;
                    if (target.position.x > 304)
                        dontBuild = true;
                    if (target.position.z < 250)
                        dontBuild = true;
                    if (target.position.z > 304)
                        dontBuild = true;

                    if (dontBuild == false)
                    {
                        roadSpawnWasSuccessful = fenceGenerator.CheckFenceType(target);
                        if (roadSpawnWasSuccessful)
                        {
                            for (int i = 0; i < forestObj.Count; i++)
                            {
                                if (forestObj[i].position.x == target.position.x && forestObj[i].position.z == target.position.z)
                                {
                                    forestObj[i].gameObject.SetActive(false);
                                    break;
                                }
                            }

                            Transform target1 = Instantiate(target, new Vector3(0, 0, 0), Quaternion.identity).transform;
                            target = target1;

                            BuildingProperties buildingProperties = target.GetComponent<BuildingProperties>();
                            for (int y = 0; y < buildingProperties.levels.Length; y++)
                                buildingProperties.levels[y].SetActive(false);
                            buildingProperties.levels[0].SetActive(true);

                            doubleClick = false;
                            lastClickTime = 0;
                        }
                    }
                    doubleClick = false;
                }
                else if (target.GetComponent<BuildingProperties>().type == BuildingProperties.BuildingType.DeleteTool)
                {

                    doubleClick = false;
                    lastClickTime = 0;

                    for (int i = 0; i < allBuildings.Count; i++)
                    {
                        if (allBuildings[i].position.x == target.position.x &&
                            allBuildings[i].position.z == target.position.z)
                        {
                            if (allBuildings[i].tag == "Space")
                            {
                                BuildingProperties spaceBuildingProperty = allBuildings[i].parent.parent.GetComponent<BuildingProperties>();
                                for (int y = 0; y < spaceBuildingProperty.additionalSpace.Length; y++)
                                {
                                    Destroy(spaceBuildingProperty.additionalSpace[y].gameObject);
                                    allBuildings.Remove(spaceBuildingProperty.additionalSpace[y]);
                                }

                                Destroy(spaceBuildingProperty.gameObject);
                                allBuildings.Remove(spaceBuildingProperty.transform);
                            }
                            else
                            {
                                for (int y = 0; y < allBuildings[i].GetComponent<BuildingProperties>().additionalSpace.Length; y++)
                                    allBuildings.Remove(allBuildings[i].GetComponent<BuildingProperties>().additionalSpace[y]);

                                Destroy(allBuildings[i].gameObject);
                                allBuildings.Remove(allBuildings[i]);

                            }

                            break;
                        }
                    }
                    for (int i = 0; i < fenceGenerator.allRoads.Count; i++)
                    {
                        if (fenceGenerator.allRoads[i].position.x == target.position.x &&
                            fenceGenerator.allRoads[i].position.z == target.position.z)
                        {
                            Destroy(fenceGenerator.allRoads[i].gameObject);
                            fenceGenerator.allRoads.Remove(fenceGenerator.allRoads[i]);
                            fenceGenerator.DeleteRoad(target.transform);
                            break;
                        }
                    }
                }
            }
        }
    }

    void SetPosition()
    {
        cameraHolder.transform.position = Vector3.Lerp(cameraHolder.transform.position, toPos, Time.deltaTime * 5);

        cameraHolder.transform.rotation = Quaternion.Lerp(cameraHolder.transform.rotation, toRot, Time.deltaTime * 5);

        toZoom.y = Mathf.Clamp(toZoom.y, -minZoom, maxZoom);
        toZoom.z = Mathf.Clamp(toZoom.z, -maxZoom, minZoom);
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, toZoom, Time.deltaTime * 5);
    }

    void MouseInut()
    {
        //Scrolling
        if (Input.mouseScrollDelta.y != 0)
            toZoom += Input.mouseScrollDelta.y * zoomScale;

        //Mouse movement
        if (Input.touchCount != 2 && (Mathf.FloorToInt(toRot.eulerAngles.y) - Mathf.FloorToInt(cameraHolder.transform.eulerAngles.y) < 8 && Mathf.FloorToInt(toRot.eulerAngles.y) - Mathf.FloorToInt(cameraHolder.transform.eulerAngles.y) > -10))
        {
            if (findPositionAfterMuiltyIputs)
            {
                Plane plane = new Plane(Vector3.up, Vector3.zero);
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                float entry;
                if (plane.Raycast(ray, out entry))
                {
                    dragStartPos = ray.GetPoint(entry);
                }

                findPositionAfterMuiltyIputs = false;
            }


            if (Input.GetMouseButtonDown(0))
            {
                Plane plane = new Plane(Vector3.up, Vector3.zero);
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                float entry;
                if (plane.Raycast(ray, out entry))
                {
                    dragStartPos = ray.GetPoint(entry);
                }
            }
            if (Input.GetMouseButton(0))
            {
                Plane plane = new Plane(Vector3.up, Vector3.zero);
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                float entry;
                if (plane.Raycast(ray, out entry))
                {
                    dragTargetPos = ray.GetPoint(entry);
                    toPos = cameraHolder.transform.position + dragStartPos - dragTargetPos;
                }
            }
        }

        //Mouse rotation
        if (Input.GetMouseButtonDown(2))
        {
            rotateStartPosition = Input.mousePosition;
        }
        if (Input.GetMouseButton(2))
        {
            rotateTargetPosition = Input.mousePosition;
            Vector3 difference = rotateStartPosition - rotateTargetPosition;
            rotateStartPosition = rotateTargetPosition;
            toRot *= Quaternion.Euler(Vector3.up * (-difference.x / 5f));
        }

        //Rotate with two fingers
        if (Input.touchCount == 2)
        {
            if (Input.GetTouch(1).deltaPosition.y < 2f)
            {
                toRot *= Quaternion.Euler(Vector3.up * Input.GetTouch(1).deltaPosition.x / 2);
                findPositionAfterMuiltyIputs = true;
            }
        //Scale with two fingers
            if (Input.GetTouch(1).deltaPosition.x < 2f)
                toZoom += Input.GetTouch(1).deltaPosition.y * zoomScale * 0.03f;
        }

        //rotate building
      /*  if (Input.GetMouseButtonDown(1))
        {
            if (target != null)
            {
                float y = target.rotation.eulerAngles.y + 90;
                target.rotation = Quaternion.Euler(0,y,0);
            }
        }*/
    }

    void KeyboardInput()
    {
        //Shifting
        if (Input.GetKey(KeyCode.LeftShift))
            movSpeed = fastSpeed;
        else
            movSpeed = normalSpeed;
        
        //Movement
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            toPos += cameraHolder.transform.forward * movSpeed;
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            toPos += cameraHolder.transform.forward * -movSpeed;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            toPos += cameraHolder.transform.right * movSpeed;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            toPos += cameraHolder.transform.right * -movSpeed;

        //Rotation
        if (Input.GetKey(KeyCode.Q))
            toRot *= Quaternion.Euler(Vector3.up * rotationScale);
        if (Input.GetKey(KeyCode.E))
            toRot *= Quaternion.Euler(Vector3.up * -rotationScale);

        //Zooming
        if (Input.GetKey(KeyCode.R))
            toZoom += zoomScale;
        if (Input.GetKey(KeyCode.F))
            toZoom -= zoomScale;

    }

    private void OnDisable()
    {
        if (StartAttack.isAttack == false)
            saveDataTrigger.BuildingDataSave();
    }

    private void OnApplicationPause(bool pause)
    {
        if (StartAttack.isAttack == false)
        {
            saveDataTrigger = FindObjectOfType<SaveDataTrigger>();
            saveDataTrigger.BuildingDataSave();
        }
    }

    private void OnApplicationQuit()
    {
        if(StartAttack.isAttack == false)
            saveDataTrigger.BuildingDataSave();
    }

}

