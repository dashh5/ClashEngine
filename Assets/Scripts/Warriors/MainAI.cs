using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MainAI : MonoBehaviour
{

    public Vector3 targetAttack;
    
    Animator animator;

    Vector2 position;

    BuildingProperties enemyBuildingProperties;

    NavMeshAgent myNavMeshAgent;
    WarriorProperties warriorProperties;
    StartAttack startAttack;
    GunsController gunsController;

    GameObject targetGO;

    GameObject[] bullets;
    BuildingBullet[] buildingBullets;
    public int bulletsArrayLength = 3;
    int currentBullet;

    void Start()
    {
        myNavMeshAgent = GetComponent<NavMeshAgent>();
        warriorProperties = GetComponent<WarriorProperties>();
        startAttack = FindObjectOfType<StartAttack>();
        gunsController = FindObjectOfType<GunsController>();

        for (int i = 0; i < warriorProperties.levels.Length; i++)
            warriorProperties.levels[i].SetActive(false);
        warriorProperties.levels[warriorProperties.level - 1].SetActive(true);

        animator = warriorProperties.levels[warriorProperties.level - 1].GetComponent<Animator>();

        position = new Vector2(transform.position.x, transform.position.z);

        //bullets array
        if (warriorProperties.bullet != null)
        {
            bullets = new GameObject[bulletsArrayLength];
            buildingBullets = new BuildingBullet[bulletsArrayLength];
            if (warriorProperties.bullet != null)
            {
                for (int i = 0; i < bulletsArrayLength; i++)
                {
                    bullets[i] = Instantiate(warriorProperties.bullet, Vector3.zero, Quaternion.identity, startAttack.bulletPooler);
                    buildingBullets[i] = bullets[i].GetComponent<BuildingBullet>();
                    buildingBullets[i].targetIsBuilding = true;
                }
            }
        }

        CheckTargetDestination();
    }

    void Update()
    {
        if (startAttack.allbuildings.Count == 0)
        {
            animator.SetBool("Go", false);
            animator.SetBool("Attack", false);
        }
        else
        {
            if (warriorProperties.fly == false)
            {
                if (targetGO == null)
                {
                    CheckTargetDestination();
                }
                else
                {
                    if (warriorProperties.HP > 0)
                    {
                        if (Vector2.Distance(new Vector2(this.transform.position.x, this.transform.position.z), new Vector2(targetAttack.x, targetAttack.z)) > warriorProperties.range)
                        {
                            animator.SetBool("Attack", false);
                            if (myNavMeshAgent.hasPath == false)
                            {
                                animator.SetBool("Go", false);
                                FindBuildingToAttack(startAttack.fence, 0f);
                                myNavMeshAgent.SetDestination(targetAttack);
                            }
                            else
                            {
                                animator.SetBool("Go", true);
                            }
                        }
                        else
                        {
                            myNavMeshAgent.ResetPath();

                            Rotate();

                            animator.SetBool("Go", false);
                            animator.SetBool("Attack", true);
                        }
                    }
                    else
                    {
                        myNavMeshAgent.isStopped = true;
                        animator.SetBool("Go", false);
                        animator.SetBool("Attack", false);
                        animator.SetBool("Dead", true);
                        gunsController.landEnemies.Remove(transform);
                    }
                }
            }
            else
            {
                if (targetGO == null)
                {
                    CheckTargetDestination();
                }
                else
                {
                    if (warriorProperties.HP > 0)
                    {
                        if (Vector2.Distance(new Vector2(this.transform.position.x, this.transform.position.z), new Vector2(targetAttack.x, targetAttack.z)) > warriorProperties.range)
                        {
                            animator.SetBool("Attack", false);

                            transform.position = Vector3.MoveTowards(transform.position, targetGO.transform.position, myNavMeshAgent.speed * Time.deltaTime);

                            Rotate();
                        }
                        else
                        {
                            animator.SetBool("Attack", true);

                            Rotate();
                        }
                    }
                    else
                    {
                        animator.SetBool("Go", false);
                        animator.SetBool("Attack", false);
                        animator.SetBool("Dead", true);
                        gunsController.flyEnemies.Remove(transform);
                    }
                }
            }
        }
    }

    void Rotate()
    {
        Vector3 relativePos = targetAttack - transform.position;
        Quaternion toRotation = Quaternion.LookRotation(relativePos);
        transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, 3 * Time.deltaTime);
    }

    private void FindBuildingToAttack(List<Transform> listOfObjects, float bias)
    {
        int enemyIndex = -1;
        float minValue = float.MaxValue;

        position = new Vector2(transform.position.x, transform.position.z);

        for (int i = 0; i < listOfObjects.Count; i++)
        {
            Vector2 enemyPosition = new Vector2(listOfObjects[i].position.x + bias, listOfObjects[i].position.z + bias);

            float distance = (enemyPosition.x - position.x) * (enemyPosition.x - position.x) + (enemyPosition.y - position.y) * (enemyPosition.y - position.y);

            if (distance < minValue)
            {
                minValue = distance;
                enemyIndex = i;
            }
        }

        enemyBuildingProperties = listOfObjects[enemyIndex].GetComponent<BuildingProperties>();

        if (enemyIndex != -1)
        {
            targetAttack = new Vector3(listOfObjects[enemyIndex].position.x + bias, 0, listOfObjects[enemyIndex].position.z + bias);
            targetGO = listOfObjects[enemyIndex].gameObject;
        }
        else
        {
            targetAttack = Vector3.zero;
        }

    }

    void CheckTargetDestination()
    {
        FindBuildingToAttack(startAttack.allbuildings, 0.5f);

        if(warriorProperties.fly == false)
            myNavMeshAgent.SetDestination(targetAttack);
    }

    public void SendDamage()
    {
        if(warriorProperties.bullet != null)
        {
            //send bullet
            buildingBullets[currentBullet].ps[0].Play(true);
            if (buildingBullets[currentBullet].psTrigger != null)
                buildingBullets[currentBullet].psTrigger.Stop(true);
            bullets[currentBullet].transform.position = warriorProperties.bulletSpawner[warriorProperties.level - 1].position;
            bullets[currentBullet].transform.rotation = warriorProperties.bulletSpawner[warriorProperties.level - 1].rotation;
            buildingBullets[currentBullet].sendDamage = true;
            buildingBullets[currentBullet].target = targetGO.transform;
            buildingBullets[currentBullet].damage = warriorProperties.damage;
            for (int i = 0; i < buildingBullets[currentBullet].ps.Length; i++)
            {
                var main = buildingBullets[currentBullet].ps[i].main;
                main.loop = true;
            }
            if (currentBullet < bulletsArrayLength - 1)
                currentBullet += 1;
            else
                currentBullet = 0;
        }
        else
        {
            enemyBuildingProperties.HP -= warriorProperties.damage;
        }
    }

    public void Dead()
    {
        if (warriorProperties.bullet != null)
            for (int i = 0; i < bullets.Length; i++)
                Destroy(bullets[i].gameObject);

        Destroy(this.gameObject);
    }

}
