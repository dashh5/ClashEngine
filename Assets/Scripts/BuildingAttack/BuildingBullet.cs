using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingBullet : MonoBehaviour
{

    public float speed;
    public ParticleSystem[] ps;
    public ParticleSystem psTrigger;
    public int damage;

    [HideInInspector]
    public bool targetIsBuilding;
    [HideInInspector]
    public Transform target;
    [HideInInspector]
    public bool sendDamage;
    [HideInInspector]
    public bool flyTarget;
    [HideInInspector]
    public float bias = 1;
    [HideInInspector]
    public bool mortar;
    [HideInInspector]
    public Vector3 targetPosSaved;
    [HideInInspector]
    public float distance;
    [HideInInspector]
    public float coef;
    [HideInInspector]
    public StartAttack startAttack;


    void Update()
    {
        if (target != null && sendDamage)
        {
            Vector3 targetPosition = new Vector3(target.position.x, target.position.y + bias, target.position.z);

            if (mortar)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosSaved, speed * Time.deltaTime);
                coef -= 0.02f;
                transform.Translate(Vector3.up * Time.deltaTime * coef);
                
                if (Vector3.Distance(transform.position, targetPosSaved) < 0.1f)
                {
                    LoopSetFalse();

                    for (int i = 0; i < startAttack.createdWarriors.Count; i++)
                    {
                        if(startAttack.createdWarriors[i] != null)
                            if(startAttack.createdWarriors[i].GetComponent<WarriorProperties>().fly == false)
                                if(Vector3.Distance(transform.position, startAttack.createdWarriors[i].transform.position) < 2)
                                    startAttack.createdWarriors[i].GetComponent<WarriorProperties>().HP -= damage;
                    }

                    if (psTrigger != null)
                        psTrigger.Play(true);

                    sendDamage = false;
                }
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

                if (transform.position == targetPosition)
                {
                    LoopSetFalse();

                    if (targetIsBuilding)
                        target.GetComponent<BuildingProperties>().HP -= damage;
                    else
                        target.GetComponent<WarriorProperties>().HP -= damage;

                    if (psTrigger != null)
                        psTrigger.Play(true);
                    
                    sendDamage = false;
                }
            }

        }
        else
        {

            if (targetIsBuilding)
            {
                LoopSetFalse();
            }
            else
            {
                if (mortar)
                {
                    LoopSetFalse();
                }
                else
                {
                    Vector3 pos = new Vector3(targetPosSaved.x, targetPosSaved.y + bias, targetPosSaved.z);

                    transform.position = Vector3.MoveTowards(transform.position, pos, speed * Time.deltaTime);

                    if (transform.position == pos)
                    {
                        LoopSetFalse();
                    }
                }
            }
        }
    }

    void LoopSetFalse()
    {
        for (int i = 0; i < ps.Length; i++)
        {
            var main = ps[i].main;
            main.loop = false;
        }
    }


}
