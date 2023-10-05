using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ControlPanelProperties : MonoBehaviour
{
    public TextMeshProUGUI objectName;
    public Scrollbar HPScrollbar;
    public TextMeshProUGUI HPtext;
    public Scrollbar DamageScrollbar;
    public TextMeshProUGUI Damagetext;
    public TextMeshProUGUI RangeText;
    public TextMeshProUGUI upgradeCostText;
    public Transform cameraRT;

    CameraController cameraController;
    PlayerProperty playerProperty;
    BuildingsMenu buildingsMenu;

    int buyCost;
    GameObject currentObj;

    private void Start()
    {
        cameraController = FindObjectOfType<CameraController>();
        playerProperty = FindObjectOfType<PlayerProperty>();
        buildingsMenu = FindObjectOfType<BuildingsMenu>();
    }

    public void Control(GameObject obj, string name, int cost, int HP, int maxHP, int damage, int damageMax, int range)
    {
        currentObj = obj;
        BuildingProperties buildingProperties = obj.GetComponent<BuildingProperties>();

        //name
        if (buildingProperties.type == BuildingProperties.BuildingType.Fence)
            objectName.text = "Fence";
        else
            objectName.text = name.Replace("(Clone)", "");

        //HP 
        HPScrollbar.size = ExtensionMethods.Remap(HP, 0, maxHP, 0, 1);
        HPScrollbar.enabled = false;

        HPtext.text = HP.ToString() + " / " + maxHP.ToString();

        //range
        RangeText.transform.parent.gameObject.SetActive(true);
        if (range != 0)
            RangeText.text = range.ToString();
        else
            RangeText.transform.parent.gameObject.SetActive(false);

        //damage
        if (damage != 0)
        {
            DamageScrollbar.gameObject.SetActive(true);
            DamageScrollbar.size = ExtensionMethods.Remap(damage, 0, damageMax, 0, 1);
            DamageScrollbar.enabled = false;
        }
        else
        {
            DamageScrollbar.gameObject.SetActive(false);
        }

        Damagetext.text = damage.ToString() + " / " + damageMax.ToString();

        //cost
        upgradeCostText.transform.parent.gameObject.SetActive(true);
        if (cost != -1)
        {
            upgradeCostText.text = cost.ToString();
            buyCost = cost;
        }
        else
        {
            upgradeCostText.transform.parent.gameObject.SetActive(false);
        }

        //image
        if(buildingProperties.spaceWidth > 1)
            cameraRT.position = new Vector3(obj.transform.position.x - 0.5f, obj.transform.position.y + 4, obj.transform.position.z - 2.5f);
        else
            cameraRT.position = new Vector3(obj.transform.position.x - 0.5f, obj.transform.position.y + 3, obj.transform.position.z - 2.5f);
    }

    public void BuyUpgrade()
    {
        if(PlayerProperty.goldenPlayer - buyCost >= 0)
        {
            PlayerProperty.goldenPlayer -= buyCost;
            playerProperty.CountResources();


            BuildingProperties buildingProperty = currentObj.GetComponent<BuildingProperties>();
            buildingProperty.levels[currentObj.GetComponent<BuildingProperties>().level - 1].SetActive(false);
            buildingProperty.level = currentObj.GetComponent<BuildingProperties>().level + 1;

            buildingProperty.buildConstruction.gameObject.SetActive(true);
            buildingProperty.buildConstruction.buildingProperties = buildingProperty;
            buildingProperty.buildConstruction.builded = false;

            buildingProperty.CountProperties();

            buildingProperty.buildConstruction.StartBuild();

            try
            {
                currentObj.GetComponent<GunBehaviour>().CountAfterUpgrade();
            }
            catch
            {

            }

            buildingsMenu.BuildingPropertiesExit();
        }
    }

}

public static class ExtensionMethods
{

    public static float Remap(this float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

}
