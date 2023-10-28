using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script is for a building construction mechanic in a Unity game.
// It manages the visual and functional aspects of constructing a building,
// including timing, rendering, and state management. The building process
// is visually represented and managed through a coroutine, and upon 
// completion, certain game objects and components are activated or 
// deactivated to reflect the building's new state. Optional rendering 
// effects can be included for additional visual feedback during 
// construction

public class BuildConstruction : MonoBehaviour
{

    int buildTime = 10;
    float buildingHigh = 1;

    Renderer[] rend;
    float timer;

    public bool builded;

    public ParticleSystem startBuildPS;
    public ParticleSystem finishBuildPS;

    public GameObject environment;
    public Transform renderers;

    [HideInInspector]
    public BuildingProperties target;
    [HideInInspector]
    public BuildingProperties buildingProperties;
    [HideInInspector]
    public CameraController cameraController;
    

    void Awake()
    {
        environment.SetActive(false);

        rend = new Renderer[this.transform.GetChild(0).childCount];
        for (int i = 0; i < this.transform.GetChild(0).childCount; i++)
        {
            rend[i] = this.transform.GetChild(0).GetChild(i).GetComponent<Renderer>();
            rend[i].material.SetFloat("_Build", 0);
            rend[i].gameObject.SetActive(false);
        }
    }

    public void StartBuild()
    {

        environment.SetActive(true);

        buildingHigh = buildingProperties.buildingHigh;
        buildTime = buildingProperties.buildingTime;
        StartCoroutine(BuildConstructionCorutine());

        ParticleSystem ps = startBuildPS;
        ps.Stop();
        var main = ps.main;
        main.duration = buildTime;
        startBuildPS.Play();

        for(int i = 0; i < buildingProperties.levels.Length; i++)
            buildingProperties.levels[i].SetActive(false);
        for (int i = 0; i < this.transform.GetChild(0).childCount; i++)
        {
            rend[i].gameObject.SetActive(true);
        }

    }

    public IEnumerator BuildConstructionCorutine()
    {
        while (builded == false)
        {
            timer += Time.deltaTime / buildTime;

            /*if (renderers.gameObject.activeSelf)
            {
                for (int i = 0; i < this.transform.GetChild(0).childCount; i++)
                {
                    rend[i].material.SetFloat("_Build", timer);
                    rend[i].material.SetFloat("_MaxHigh", buildingHigh);
                }
            }*/

            if (timer > 1)
            {
                timer = 0;
                this.gameObject.SetActive(false);
                buildingProperties.levels[buildingProperties.level - 1].SetActive(true); //set level
                builded = true;
                finishBuildPS.transform.parent = null;
                finishBuildPS.Play();
                buildingProperties.boxCollider.enabled = true;
                if(buildingProperties.type == BuildingProperties.BuildingType.Defence)
                    buildingProperties.gameObject.GetComponent<GunBehaviour>().enabled = true;
            }
            yield return new WaitForEndOfFrame();
        }

        yield return null;
    }
}

