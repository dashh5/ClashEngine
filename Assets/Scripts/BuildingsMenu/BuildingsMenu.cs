using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.AI;

public class BuildingsMenu : MonoBehaviour
{
    [System.Serializable]
    public class BuildingsAsset
    {
        public string name;
        public GameObject type;
        public GameObject[] buildings;
        public int minPos, maxPos;
    }
    private List<GameObject> buildingsParents = new List<GameObject>();
    private List<GameObject> buildingsTypes = new List<GameObject>();

    public Camera menuCamera;
    public Grid grid;
    public Canvas canvas;
    public TextMeshProUGUI textPRO;
    public Button typeButton;
    public Button warriorButtonType;
    public Button buildingButton;
    public GameObject activateMenu;
    public GameObject warriorsBuy;
    public GameObject warriorsAttack;
    public GameObject buildingProperties;
    public GameObject buildingButtons;
    public GameObject warriorTextCount;
    public Scrollbar GoldenSlider;
    public Scrollbar ExtractSlider;
    public Scrollbar warriorTimerScrollbar;
    public TextMeshProUGUI GoldenSliderText;
    public TextMeshProUGUI ExtractSliderText;
    public GameObject exitBattle;

    public Transform types;
    public BuildingsAsset[] buildings;
    public GameObject deleteBuilding;
    int minTypePos, maxTypePos;
    int minPos, maxPos;
    int previousX, nextX;

    public Transform warriors;
    public Transform warriorsForAttack;

    bool changePos = false;

    private Vector3 toPos;
    private float posX;

    private Vector3 dragStartPos;
    private Vector3 dragTargetPos;

    private CameraController cameraController;
    private ControlPanelProperties controlPanelProperties;
    private StartAttack startAttack;
    private PlayerProperty playerProperty;
    private SaveDataTrigger saveDataTrigger;
    private GunAttackRadius gunAttackRadius;

    bool doubleClick;

    private int objsSize = 25;

    List<int> warriorsQueue = new List<int>();
    List<float> warriorsQueueTimer = new List<float>();
    float warriorBuyingTimer;
    bool finishBuying;
    Scrollbar[] scrollingTimerArray;
    TextMeshProUGUI[] textCountArray;
    TextMeshProUGUI[] textCountBuyArray;
    int[] queueCurrentWarrior;

    int currentIndex = 0;
    int currentIndex2 = 0;
    int currentIndex3 = 0;

    private void Awake()
    {
        controlPanelProperties = FindObjectOfType<ControlPanelProperties>();
        cameraController = FindObjectOfType<CameraController>();
        startAttack = FindObjectOfType<StartAttack>();
        playerProperty = FindObjectOfType<PlayerProperty>();
        saveDataTrigger = FindObjectOfType<SaveDataTrigger>();

        CreateTypes();
        CreateWarriorsMenu();

        minPos = minTypePos;
        maxPos = maxTypePos;

        activateMenu.SetActive(false);
        warriorsBuy.SetActive(false);
        grid.enabled = false;

        buildingProperties.SetActive(false);
        buildingButtons.SetActive(false);

        warriorsAttack.SetActive(false);

        exitBattle.SetActive(false);
    }

    private void Update()
    {
        MouseInput();

        if (activateMenu.activeSelf)
        {
            if (types.localPosition.x > maxPos)
            {
                types.localPosition = new Vector3(Mathf.Lerp(types.localPosition.x, maxPos, Time.deltaTime), 0, 0);
                if (posX < 0)
                    posX = posX / 2;
            }
            if (types.localPosition.x < minPos)
            {
                types.localPosition = new Vector3(Mathf.Lerp(types.localPosition.x, minPos, Time.deltaTime), 0, 0);
                if (posX > 0)
                    posX = posX / 2;
            }

            types.localPosition = new Vector3(Mathf.Lerp(types.localPosition.x, types.localPosition.x + -posX * 5, Time.deltaTime), 0, 0);
        }

        if (warriorsBuy.gameObject.activeSelf)
        {
            if (warriorsBuy.transform.localPosition.x > maxPos)
            {
                warriorsBuy.transform.localPosition = new Vector3(Mathf.Lerp(warriorsBuy.transform.localPosition.x, maxPos, Time.deltaTime), 0, 0);
                if (posX < 0)
                    posX = posX / 2;
            }
            if (warriorsBuy.transform.localPosition.x < minPos)
            {
                warriorsBuy.transform.localPosition = new Vector3(Mathf.Lerp(warriorsBuy.transform.localPosition.x, minPos, Time.deltaTime), 0, 0);
                if (posX > 0)
                    posX = posX / 2;
            }

            warriorsBuy.transform.localPosition = new Vector3(Mathf.Lerp(warriorsBuy.transform.localPosition.x, warriorsBuy.transform.localPosition.x + -posX * 5, Time.deltaTime), 0, 0);
        }


        //buying warrior
        if (warriorsQueue.Count > 0)
        {

            if (finishBuying && warriorBuyingTimer <= 0)
            {
                // 2
                scrollingTimerArray[currentIndex].gameObject.SetActive(false);
                startAttack.army[currentIndex].amount += 1;
                queueCurrentWarrior[currentIndex] -= 1;
                if(queueCurrentWarrior[currentIndex] != 0)
                    textCountBuyArray[currentIndex].text = 'x' + queueCurrentWarrior[currentIndex].ToString();
                else
                    textCountBuyArray[currentIndex].text = "";
                textCountArray[currentIndex].text = startAttack.army[currentIndex].amount.ToString();
                warriorsQueue.RemoveAt(currentIndex3);
                warriorsQueueTimer.RemoveAt(currentIndex2);
            }
            if (warriorBuyingTimer <= 0 && finishBuying == false)
            {
                // 1
                currentIndex = warriorsQueue[0];
                currentIndex2 = 0;
                currentIndex3 = 0;
               
                textCountBuyArray[currentIndex].text = 'x' + queueCurrentWarrior[currentIndex].ToString();
                warriorBuyingTimer = warriorsQueueTimer[currentIndex2];
                finishBuying = true;
                scrollingTimerArray[currentIndex].gameObject.SetActive(true);
            }
            if (warriorBuyingTimer <= 0 && finishBuying)
            {
                // 3
                finishBuying = false;
            }
            if (warriorBuyingTimer > 0)
            {
                scrollingTimerArray[currentIndex].size = warriorBuyingTimer.Remap(warriorsQueueTimer[currentIndex2], 0, 0, 1);
                warriorBuyingTimer -= Time.deltaTime;
            }
        }
    }


    void LateUpdate()
    {
        float pinchAmount = 0;
        Quaternion desiredRotation = transform.rotation;

        DetectTouchMovement.Calculate();

        if (Mathf.Abs(DetectTouchMovement.pinchDistanceDelta) > 0)
        { // zoom
            pinchAmount = DetectTouchMovement.pinchDistanceDelta;
        }

        if (Mathf.Abs(DetectTouchMovement.turnAngleDelta) > 0)
        { // rotate
            Vector3 rotationDeg = Vector3.zero;
            rotationDeg.z = -DetectTouchMovement.turnAngleDelta;
            desiredRotation *= Quaternion.Euler(rotationDeg);
        }

        transform.rotation = desiredRotation;
        transform.position += Vector3.forward * pinchAmount;
    }
    

    void MouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = menuCamera.ScreenPointToRay(Input.mousePosition);

            float entry;
            if (plane.Raycast(ray, out entry))
            {
                dragStartPos = ray.GetPoint(entry);
            }
        }
        if (Input.GetMouseButton(0))
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = menuCamera.ScreenPointToRay(Input.mousePosition);

            float entry;
            if (plane.Raycast(ray, out entry))
            {
                dragTargetPos = ray.GetPoint(entry);
                toPos = transform.position + dragStartPos - dragTargetPos;
                if (Mathf.Abs(toPos.x) > Mathf.Abs(toPos.z))
                    if (dragStartPos.z < 750)
                        posX = toPos.x;
            }
        }
        else
        {
            if (posX > 0)
                posX -= Time.deltaTime * 100;
            if (posX < 0)
                posX += Time.deltaTime * 100;
        }
    }

    //create warrior buying menu
    void CreateWarriorsMenu()
    {
        scrollingTimerArray = new Scrollbar[startAttack.allWarriors.Length];
        textCountArray = new TextMeshProUGUI[startAttack.allWarriors.Length];
        textCountBuyArray = new TextMeshProUGUI[startAttack.allWarriors.Length];
        queueCurrentWarrior = new int[startAttack.allWarriors.Length];

        int posType = 0;
        for (int i = 0; i < startAttack.allWarriors.Length; i++)
        {
            //gameobject
            GameObject type = Instantiate(startAttack.allWarriors[i], new Vector3(0, 0, 0), Quaternion.identity, warriors);
            type.transform.localPosition = new Vector3(-posType, 0, 0);
            type.transform.localScale = new Vector3(50, 50, 50);
            type.transform.rotation = Quaternion.Euler(0, 180, 0);

            foreach (Transform trans in type.GetComponentsInChildren<Transform>(true))
                trans.gameObject.layer = 5;

            WarriorProperties properties = type.GetComponent<WarriorProperties>();

            if(properties.fly)
                type.transform.GetChild(0).localPosition = new Vector3(type.transform.GetChild(0).localPosition.x,
                    type.transform.GetChild(0).localPosition.y - 3, type.transform.GetChild(0).localPosition.z);

            for (int b = 0; b < properties.levels.Length; b++)
                properties.levels[b].SetActive(false);
            properties.levels[properties.level-1].SetActive(true);

            type.GetComponent<MainAI>().enabled = false;
            type.GetComponent<NavMeshAgent>().enabled = false;

            if (posType >= 0)
                posType += 125;
            posType *= -1;

            // button
            Button button = Instantiate(warriorButtonType, new Vector3(0, 0, 0), Quaternion.identity, type.transform);
            button.transform.localPosition = new Vector3(0, 0.45f, -0.25f);
            button.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
            button.gameObject.name = startAttack.allWarriors[i].name;

            //Text for warriors count
            Transform textCount = Instantiate(warriorTextCount.transform, new Vector3(0,0,0), Quaternion.identity, type.transform);
            textCount.localPosition = new Vector3(-0.77f, 1.3f, 0);
            textCount.localRotation = Quaternion.Euler(0, 180, 0);
            textCount.localScale = new Vector3(0.013f, 0.013f, 0.013f);
            textCount.GetComponent<TextMeshProUGUI>().text = startAttack.army[i].amount.ToString();
            textCountArray[i] = textCount.GetComponent<TextMeshProUGUI>();

            //Text for warriors in building
            Transform textCountBuy = Instantiate(warriorTextCount.transform, new Vector3(0,0,0), Quaternion.identity, type.transform);
            textCountBuy.localPosition = new Vector3(0.77f, 1.3f, 0);
            textCountBuy.localRotation = Quaternion.Euler(0, 180, 0);
            textCountBuy.GetComponent<TextMeshProUGUI>().text = "";
            textCountBuyArray[i] = textCountBuy.GetComponent<TextMeshProUGUI>();

            //timer
            Transform timerScrollbar = Instantiate(warriorTimerScrollbar.transform, new Vector3(0, 0, 0), Quaternion.identity, type.transform);
            timerScrollbar.localPosition = new Vector3(0f, -0.3f, 0);
            timerScrollbar.localScale = new Vector3(0.01f, 0.01f, 0.01f);
            scrollingTimerArray[i] = timerScrollbar.GetComponent<Scrollbar>();
            scrollingTimerArray[i].gameObject.SetActive(false);

            //call button click
            int warriorIndex = i;
            button.onClick.AddListener(() => BuyWarrior(warriorIndex, properties.cost, textCount, timerScrollbar.GetComponent<Scrollbar>(), properties.buildingTime));
        }
    }

    public void BuyWarrior(int warriorIndex, int cost, Transform textCount, Scrollbar timerScrollbar, float timerBuying)
    {
        if (PlayerProperty.extractPlayer - cost > 0)
        {
            warriorsQueue.Add(warriorIndex);
            warriorsQueueTimer.Add(timerBuying);
            queueCurrentWarrior[warriorIndex] += 1;

            textCountBuyArray[warriorsQueue[warriorsQueue.Count - 1]].text = 'x' + queueCurrentWarrior[warriorsQueue[warriorsQueue.Count - 1]].ToString();

            PlayerProperty.extractPlayer -= cost;
            playerProperty.CountResources();
        }
    }

    public void CreateWarriorsAttackMenu()
    {
        int posType = 0;
        for (int i = 0; i < startAttack.army.Count; i++)
        {
            if (startAttack.army[i].amount != 0)
            {
                //gameobject
                GameObject type = Instantiate(startAttack.army[i].warrior, new Vector3(0, 0, 0), Quaternion.identity, warriorsForAttack);
                type.transform.localPosition = new Vector3(-posType, 0, 0);
                type.transform.localScale = new Vector3(50, 50, 50);
                type.transform.rotation = Quaternion.Euler(0, 180, 0);

                foreach (Transform trans in type.GetComponentsInChildren<Transform>(true))
                    trans.gameObject.layer = 5;

                WarriorProperties properties = type.GetComponent<WarriorProperties>();
                
                properties.CountProperties();
                if (properties.fly)
                    type.transform.GetChild(0).localPosition = new Vector3(type.transform.GetChild(0).localPosition.x,
                        type.transform.GetChild(0).localPosition.y - 3, type.transform.GetChild(0).localPosition.z);

                for (int b = 0; b < properties.levels.Length; b++)
                    properties.levels[b].SetActive(false);
                properties.levels[properties.level - 1].SetActive(true);

                type.GetComponent<MainAI>().enabled = false;
                type.GetComponent<NavMeshAgent>().enabled = false;

                if (posType >= 0)
                    posType += 125;
                posType *= -1;

                // button
                Button button = Instantiate(typeButton, new Vector3(0, 0, 0), Quaternion.identity, type.transform);
                button.transform.localPosition = new Vector3(0, 0.55f, 0);
                button.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
                button.gameObject.name = startAttack.army[i].warrior.name;
                startAttack.warriorsButtons[i] = button.gameObject;

                //Text for warriors count
                Transform textCount = Instantiate(warriorTextCount.transform, new Vector3(0, 0, 0), Quaternion.identity, type.transform);
                textCount.localPosition = new Vector3(-0.77f, 1.3f, 0);
                textCount.localRotation = Quaternion.Euler(0, 180, 0);
                textCount.localScale = new Vector3(0.03f, 0.03f, 0.03f);
                textCount.GetComponent<TextMeshProUGUI>().text = startAttack.army[i].amount.ToString();
                startAttack.warriorsTextCount[i] = textCount.GetComponent<TextMeshProUGUI>();

                //call button click
                int warriorIndex = i;
                button.onClick.AddListener(() => ChooseWarriorToSpawn(warriorIndex));
            }
        }
        warriorsAttack.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        warriorsAttack.SetActive(false);
    }

    public void ChooseWarriorToSpawn(int index)
    {
        startAttack.currentWarriorToSpawn = index;
        Transform war = Instantiate(startAttack.army[index].warrior, new Vector3(0, 0, 0), Quaternion.identity).transform;
        war.GetComponent<MainAI>().enabled = false;
        war.GetComponent<NavMeshAgent>().enabled = false;

        WarriorProperties warProp = war.GetComponent<WarriorProperties>();
        for (int i = 0; i < warProp.levels.Length; i++)
            warProp.levels[i].SetActive(false);
        warProp.levels[warProp.level - 1].SetActive(true);

        if (cameraController.target != null)
            Destroy(cameraController.target.gameObject);

        cameraController.target = war;
        cameraController.moveTarget = true;
        cameraController.targetIsWarrior = true;
    }

    void CreateTypes()
    {
        int posType = 0;
        for (int i = 0; i < buildings.Length; i++)
        {
            //types
            GameObject type = Instantiate(buildings[i].type, new Vector3(0, 0, 0), Quaternion.identity, types);
            type.transform.localPosition = new Vector3(-posType, 0, 0);
            type.transform.localScale = new Vector3(objsSize, objsSize, objsSize);
            type.transform.rotation = Quaternion.Euler(0, 180, 0);
            type.name = buildings[i].type.name;
            foreach (Transform trans in type.GetComponentsInChildren<Transform>(true))
                trans.gameObject.layer = 5;
            buildingsTypes.Add(type);

            BuildingProperties properties = type.GetComponent<BuildingProperties>();
            for (int b = 0; b < properties.levels.Length; b++)
                properties.levels[b].SetActive(false);
            properties.levels[0].SetActive(true);               //set level

            properties.levels[0].transform.localPosition = new Vector3(0, 0, 0.5f);

            if (properties.type == BuildingProperties.BuildingType.Defence)
                properties.gameObject.GetComponent<GunBehaviour>().enabled = false;

                TextMeshProUGUI text = Instantiate(textPRO, new Vector3(0, 0, 0), Quaternion.identity, type.transform);
            text.transform.localPosition = new Vector3(0, 2.5f, 0);
            text.text = buildings[i].name;

            Button button = Instantiate(typeButton, new Vector3(0, 0, 0), Quaternion.identity, type.transform);
            button.transform.localPosition = new Vector3(0, 1, 0);
            button.onClick.AddListener(ClickCheck);
            button.gameObject.name = buildings[i].type.name;

            if (properties.boxCollider != null)
                properties.boxCollider.enabled = false;

            //find min and max position of type menu
            if (posType > maxTypePos)
                maxTypePos = posType;
            if (posType < minTypePos)
                minTypePos = posType;

            if (posType >= 0)
                posType += 125;
            posType *= -1;
            
            //buildings
            GameObject newObj = new GameObject("new");
            GameObject parent = Instantiate(newObj, new Vector3(0, 0, 0), Quaternion.identity, types);
            parent.transform.localPosition = new Vector3(0, 0, 0);
            parent.name = buildings[i].type.name + "_buildings";
            Destroy(newObj.gameObject);
            buildingsParents.Add(parent);

            int posBuild = 0;
            previousX = 0;
            changePos = true;
            for (int u = 0; u < buildings[i].buildings.Length; u++)
            {
                posBuild -= 70;
                int nextZ = -24;
                int buttonPos = -1;
                for (int y = 0; y < buildings[i].buildings[u].GetComponent<BuildingProperties>().spaceWidth; y++)
                {
                    posBuild -= 18;
                    nextZ += 24;
                    buttonPos += 1;
                }

                GameObject build = Instantiate(buildings[i].buildings[u], new Vector3(0, 0, 0), Quaternion.identity, parent.transform);
                build.transform.localPosition = new Vector3(-posBuild, 0, nextZ);
                build.transform.localScale = new Vector3(objsSize, objsSize, objsSize);
                build.transform.rotation = Quaternion.Euler(0, 180, 0);
                build.name = buildings[i].buildings[u].name;
                foreach (Transform trans in build.GetComponentsInChildren<Transform>(true))
                    trans.gameObject.layer = 5;
                parent.SetActive(false);

                Button buttonBuild = Instantiate(buildingButton, new Vector3(0, 0, 0), Quaternion.identity, build.transform);
                buttonBuild.transform.localPosition = new Vector3(0.5f, 1, buttonPos);
                buttonBuild.onClick.AddListener(CreateBuilding);
                buttonBuild.gameObject.name = buildings[i].buildings[u].name;

                properties = build.GetComponent<BuildingProperties>();

                for (int b = 0; b < properties.levels.Length; b++)
                    properties.levels[b].SetActive(false);
                properties.levels[0].SetActive(true);               //set level

                if (properties.type == BuildingProperties.BuildingType.Defence)
                    properties.gameObject.GetComponent<GunBehaviour>().enabled = false;

                if (properties.boxCollider != null)
                    properties.boxCollider.enabled = false;

                //find min and max position of each type
                if (posBuild > buildings[i].maxPos)
                    buildings[i].maxPos = posBuild;
                if (posBuild < buildings[i].minPos)
                    buildings[i].minPos = posBuild;

                if (build.GetComponent<BuildingProperties>().buildConstruction != null)
                    build.GetComponent<BuildingProperties>().buildConstruction.enabled = false;
            }
        }

        //delete buildings button
        GameObject typeDel = Instantiate(deleteBuilding, new Vector3(0, 0, 0), Quaternion.identity, types);
        typeDel.transform.localPosition = new Vector3(-posType, 0, 0);
        typeDel.transform.localScale = new Vector3(objsSize, objsSize, objsSize);
        typeDel.name = deleteBuilding.name;
        foreach (Transform trans in typeDel.GetComponentsInChildren<Transform>(true))
            trans.gameObject.layer = 5;
        buildingsTypes.Add(typeDel);

        TextMeshProUGUI textDel = Instantiate(textPRO, new Vector3(0, 0, 0), Quaternion.identity, typeDel.transform);
        textDel.transform.localPosition = new Vector3(0, 2.5f, 0);
        textDel.text = deleteBuilding.name;

        Button buttonDel = Instantiate(typeButton, new Vector3(0, 0, 0), Quaternion.identity, typeDel.transform);
        buttonDel.transform.localPosition = new Vector3(0, 1, 0);
        buttonDel.onClick.AddListener(DeleteBuilding);
        buttonDel.gameObject.name = deleteBuilding.name;

    }


    public void ActivateMenu()
    {
        if (warriorsBuy.activeSelf == false)
        {
            buildingButtons.SetActive(false);

            if (cameraController.target != null)
                Destroy(cameraController.target.gameObject);
            else
                grid.enabled = !grid.isActiveAndEnabled;

            cameraController.target = null;
            cameraController.moveTarget = false;

            for (int i = 0; i < buildingsParents.Count; i++)
                buildingsParents[i].SetActive(false);
            for (int i = 0; i < buildingsTypes.Count; i++)
                buildingsTypes[i].SetActive(true);

            activateMenu.SetActive(!activateMenu.activeSelf);
            types.transform.localPosition = Vector3.zero;

            minPos = minTypePos;
            maxPos = maxTypePos;
        }
    }


    public void ClickCheck()
    {
        if (cameraController.doubleClick)
        {
            cameraController.doubleClick = false;
            cameraController.lastClickTime = 0;
            for (int i = 0; i < buildingsParents.Count; i++)
            {
                if (EventSystem.current.currentSelectedGameObject.name + "_buildings" == buildingsParents[i].name)
                {
                    buildingsParents[i].SetActive(true);
                    minPos = buildings[i].minPos;
                    maxPos = buildings[i].maxPos;
                }
            }
            for (int i = 0; i < buildingsTypes.Count; i++)
            {
                buildingsTypes[i].SetActive(false);
            }
            //set active delete tool
        }
    }


    public void CreateBuilding()
    {
        if (cameraController.doubleClick)
        {
            cameraController.doubleClick = false;
            cameraController.lastClickTime = 0;

            for (int i = 0; i < buildings.Length; i++)
            {
                for (int u = 0; u < buildings[i].buildings.Length; u++)
                {
                    if (buildings[i].buildings[u].name == EventSystem.current.currentSelectedGameObject.name)
                    {
                        cameraController.moveTarget = true;
                        Transform target = Instantiate(buildings[i].buildings[u], new Vector3(0, 0, 0), Quaternion.identity).transform;

                        BuildingProperties carentProperties = target.GetComponent<BuildingProperties>();

                        if (carentProperties.boxCollider != null)
                            carentProperties.boxCollider.enabled = false;

                        for (int y = 0; y < carentProperties.levels.Length; y++)
                            carentProperties.levels[y].SetActive(false);
                        carentProperties.levels[0].SetActive(true);

                        if (carentProperties.type == BuildingProperties.BuildingType.Defence)
                            carentProperties.gameObject.GetComponent<GunBehaviour>().enabled = false;

                        cameraController.target = target;
                    }
                }
            }

            for (int i = 0; i < buildingsParents.Count; i++)
            {
                buildingsTypes[i].SetActive(true);
                buildingsParents[i].SetActive(false);
            }

            activateMenu.SetActive(!activateMenu.activeSelf);

            minPos = minTypePos;
            maxPos = maxTypePos;
        }
    }


    public void DeleteBuilding()
    {
        cameraController.moveTarget = true;
        Transform target = Instantiate(deleteBuilding, new Vector3(0, 0, 0), Quaternion.identity).transform;
        target.transform.GetChild(0).localPosition = new Vector3(0, 2, 0);
        cameraController.target = target;
        activateMenu.SetActive(!activateMenu.activeSelf);
    }

    public void BuildingPropertiesController()
    {
        cameraController.enabled = false;
        buildingButtons.SetActive(false);
        buildingProperties.SetActive(true);
        controlPanelProperties.Control(cameraController.selectedBuilding.gameObject, cameraController.selectedBuilding.name, cameraController.selectedBuilding.cost, 
            cameraController.selectedBuilding.HP, cameraController.selectedBuilding.maxHP, cameraController.selectedBuilding.damage, cameraController.selectedBuilding.maxDamage, 
            cameraController.selectedBuilding.range);
    }

    public void BuildingPropertiesExit()
    {
        cameraController.enabled = true;
        buildingProperties.SetActive(false);
    }

    public void ActivateWarriorBuyMenu()
    {
        if (activateMenu.activeSelf == false)
        {
            if (cameraController.target != null)
                Destroy(cameraController.target.gameObject);
            if (grid.enabled)
                grid.enabled = false;

            cameraController.target = null;
            cameraController.moveTarget = false;

            warriorsBuy.SetActive(!warriorsBuy.activeSelf); 
        }
    }


}


