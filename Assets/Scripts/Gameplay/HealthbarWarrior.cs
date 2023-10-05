using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthbarWarrior : MonoBehaviour
{

    public Transform scale;

    WarriorProperties warriorProperties;
    GunsController gunsController;

    int maxHP;

    StartAttack startAttack;

    private void Start()
    {
        startAttack = FindObjectOfType<StartAttack>();
        gunsController = FindObjectOfType<GunsController>();

        warriorProperties = transform.parent.GetComponent<WarriorProperties>();
        maxHP = warriorProperties.HP;
    }

    void Update()
    {
        this.transform.LookAt(Camera.main.transform);
        scale.localScale = new Vector3(Mathf.Round(warriorProperties.HP).Remap(0, maxHP, 1, 23), 1, 1);
        if (warriorProperties.HP <= 0)
        {
            Destroy(gameObject);
        }
    }
}
