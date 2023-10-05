using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healthbar : MonoBehaviour
{

    public Transform scale;

    BuildingProperties buildingProperties;

    int maxHP;

    StartAttack startAttack;

    private void Start()
    {
        startAttack = FindObjectOfType<StartAttack>();

        buildingProperties = transform.parent.GetComponent<BuildingProperties>();
        maxHP = buildingProperties.HP;
    }

    void Update()
    {
        this.transform.LookAt(Camera.main.transform);
        scale.localScale = new Vector3(Mathf.Round(buildingProperties.HP).Remap(0, maxHP, 1, 23), 1, 1);
        if (buildingProperties.HP <= 0)
        {
            startAttack.allbuildings.Remove(transform.parent.transform);

            if (buildingProperties.type == BuildingProperties.BuildingType.Fence)
                startAttack.fence.Remove(transform.parent.transform);

            Destroy(transform.parent.gameObject);
        }
    }
}
