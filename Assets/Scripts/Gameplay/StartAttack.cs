using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


// The 'StartAttack' class is a central manager for initiating and handling attacks 
// in a Unity game. It manages various game elements, such as warriors, buildings,
// and UI components, to facilitate an attack scenario. The class handles the setup
// and teardown of attacks, manages health bars, and cleans up game objects as necessary.

public class StartAttack : MonoBehaviour
{

    public static bool isAttack;

    public GameObject canvasMenu;
    public GameObject canvasButtons;

    public ParticleSystem smoke;

    [System.Serializable]
    public class Army
    {
        public GameObject warrior;
        public int amount;
    }
    public List<Army> army;

    public GameObject[] allWarriors;

    public List<Transform> allbuildings = new List<Transform>();
    public List<Transform> defence = new List<Transform>();
    public List<Transform> fence = new List<Transform>();

    SaveDataTrigger saveDataTrigger;
    BuildingsMenu buildingMenu;
    FenceGenerator fenceGenerator;
    CameraController cameraController;

    public int currentWarriorToSpawn;

    List<BuildingProperties> buildingHealthBarsArray = new List<BuildingProperties>();

    public List<WarriorProperties> warriorsHealthBarsArray = new List<WarriorProperties>();

    public List<GunBehaviour> allGunsBehaviour = new List<GunBehaviour>();

    public Transform bulletPooler;

    public List<GameObject> createdWarriors = new List<GameObject>();

    public TextMeshProUGUI[] warriorsTextCount;
    public GameObject[] warriorsButtons;

    private void Awake()
    {
        saveDataTrigger = FindObjectOfType<SaveDataTrigger>();
        buildingMenu = FindObjectOfType<BuildingsMenu>();
        fenceGenerator = FindObjectOfType<FenceGenerator>();
        cameraController = FindObjectOfType<CameraController>();

        //create warriors array
        for (int i = 0; i < allWarriors.Length; i++)
        {
            Army ar = new Army();
            ar.warrior = allWarriors[i];
            army.Add(ar);
        }

        warriorsTextCount = new TextMeshProUGUI[army.Count];
        warriorsButtons = new GameObject[army.Count];

        this.enabled = false;
    }


    // Start the Attack

    public void AttackStart()
    {
        CleanArrays();

        isAttack = true;

        this.enabled = true;

        saveDataTrigger.BuildingDataSave();

        smoke.Play();

        //menu
        canvasMenu.SetActive(true);
        canvasButtons.SetActive(false);
        saveDataTrigger.enabled = false;

        buildingMenu.warriorsBuy.SetActive(false);
        buildingMenu.activateMenu.SetActive(false);

        buildingMenu.CreateWarriorsAttackMenu();
        buildingMenu.warriorsAttack.SetActive(true);

        buildingMenu.exitBattle.SetActive(true);

        //building health bars
        for (int i = 0; i < allbuildings.Count; i++)
               buildingHealthBarsArray.Add(allbuildings[i].GetComponent<BuildingProperties>());

        for (int i = 0; i < fence.Count; i++)
            buildingHealthBarsArray.Add(fence[i].GetComponent<BuildingProperties>());


        for (int i = 0; i < allGunsBehaviour.Count; i++)
                allGunsBehaviour[i].enabled = true;

        for (int i = 0; i < fence.Count; i++)
            if (fence[i] == null)
                fence.RemoveAt(i);

        //dissable traps vision
        for (int i = 0; i < cameraController.buildingsParent.childCount; i++)
        {
            BuildingProperties bp = cameraController.buildingsParent.GetChild(i).GetComponent<BuildingProperties>();
            if (bp.type == BuildingProperties.BuildingType.Trap)
                bp.levels[bp.level - 1].SetActive(false);
        }

        int count = cameraController.allBuildings.Count + fenceGenerator.allRoads.Count;
        cameraController.whereCanntSpawnWarriors = new Vector2[count];
        for (int i = 0; i < cameraController.allBuildings.Count; i++)
            cameraController.whereCanntSpawnWarriors[i] = new Vector2(cameraController.allBuildings[i].position.x, cameraController.allBuildings[i].position.z);
        for (int i = 0; i < fenceGenerator.allRoads.Count; i++)
            cameraController.whereCanntSpawnWarriors[cameraController.allBuildings.Count + i] = new Vector2(fenceGenerator.allRoads[i].position.x, fenceGenerator.allRoads[i].position.z);
    }


    // Stop the Attack

    public void StopAttack()
    {
        isAttack = false;

        smoke.Play();

        //menu
        canvasMenu.SetActive(true);
        canvasButtons.SetActive(true);
        saveDataTrigger.enabled = true;
        
        buildingMenu.warriorsAttack.SetActive(false);

        buildingMenu.exitBattle.SetActive(false);

        for (int i = 0; i < createdWarriors.Count; i++)
            Destroy(createdWarriors[i]);
        createdWarriors = new List<GameObject>();

        for (int i = 0; i < allbuildings.Count; i++)
            Destroy(allbuildings[i].gameObject);
        allbuildings = new List<Transform>();

        for (int i = 0; i < fence.Count; i++)
            Destroy(fence[i].gameObject);
        fence = new List<Transform>();

        for (int i = 0; i < defence.Count; i++)
            Destroy(defence[i].gameObject);
        defence = new List<Transform>();

        allGunsBehaviour = new List<GunBehaviour>();

        if(cameraController.target != null)
            Destroy(cameraController.target.gameObject);

        fenceGenerator.allRoads = new List<Transform>();
        cameraController.allBuildings = new List<Transform>();
        saveDataTrigger.BuildingDataLoad();
    }

    private void Update()
    {

        if (allbuildings.Count == 0)
        {
            StopAttack();
            this.enabled = false;
        }
        else
        {
            //activate building's healthbar
            if (isActiveAndEnabled)
            {
                for (int i = 0; i < buildingHealthBarsArray.Count; i++)
                {
                    if (buildingHealthBarsArray[i].HP != buildingHealthBarsArray[i].startHeath)
                    {
                        buildingHealthBarsArray[i].healthBar.SetActive(true);
                        buildingHealthBarsArray.RemoveAt(i);
                    }
                }
            }

            //activate warriors's healthbar
            if (isActiveAndEnabled)
            {
                for (int i = 0; i < warriorsHealthBarsArray.Count; i++)
                {
                    if (warriorsHealthBarsArray[i].HP != warriorsHealthBarsArray[i].startHeath)
                    {
                        warriorsHealthBarsArray[i].healthBar.SetActive(true);
                        warriorsHealthBarsArray.RemoveAt(i);
                    }
                }
            }
        }
    }

    public void CleanArrays()
    {
        for (int i = 0; i < allbuildings.Count; i++)
            if (allbuildings[i] == null)
                allbuildings.RemoveAt(i);

        for (int i = 0; i < fence.Count; i++)
            if (fence[i] == null)
                fence.RemoveAt(i);

        for (int i = 0; i < allGunsBehaviour.Count; i++)
            if (allGunsBehaviour[i] == null)
                allGunsBehaviour.RemoveAt(i);
    }


}
