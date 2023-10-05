using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBehaviour : MonoBehaviour
{

    public Transform enemy;
    public static int gunIndexes;
    public int currentGunIndex;
    public ParticleSystem PS;

    Transform tower;

    bool attack;

    IEnumerator attackCorutine;

    Vector2 position, enemyPosition;

    int rangePow, range;

    float minValue, distance, timer;

    //enemies 
    int enemyIndex;
    
    //other scripts
    GunsController gunsController;
    BuildingProperties buildingProperties;
    StartAttack startAttack;

    WarriorProperties warriorProperties;

    //attack properties
    int attackDamage;
    float attackSpeed;

    GameObject[] bullets;
    BuildingBullet[] buildingBullets;
    public int bulletsArrayLength = 3;
    int currentBullet;

    private void Start()
    {
        gunIndexes += 1;
        currentGunIndex = gunIndexes;

        gunsController = FindObjectOfType<GunsController>();
        buildingProperties = this.GetComponent<BuildingProperties>();
        startAttack = FindObjectOfType<StartAttack>();

        startAttack.allGunsBehaviour.Add(this);

        CountAfterUpgrade();

        StartCoroutine(FindEnemiesDelay());

        if (StartAttack.isAttack == false)
            this.enabled = false;

        attackDamage = buildingProperties.damage;
        attackSpeed = buildingProperties.attackSpeed;

        attackCorutine = Attack();

        //bullets array
        bullets = new GameObject[bulletsArrayLength];
        buildingBullets = new BuildingBullet[bulletsArrayLength];
        if (buildingProperties.bullet != null) {
            for (int i = 0; i < bulletsArrayLength; i++)
            {
                bullets[i] = Instantiate(buildingProperties.bullet, Vector3.zero, Quaternion.identity, startAttack.bulletPooler);
                buildingBullets[i] = bullets[i].GetComponent<BuildingBullet>();
                buildingBullets[i].targetIsBuilding = false;
               // bullets[i].SetActive(false);
            }
        }
    }

    public void CountAfterUpgrade()
    {
        rangePow = buildingProperties.range * buildingProperties.range;
        range = buildingProperties.range;

        if (buildingProperties.defenceType == BuildingProperties.DefenceType.Cannon || buildingProperties.defenceType == BuildingProperties.DefenceType.Shooter ||
             buildingProperties.defenceType == BuildingProperties.DefenceType.BombsLauncher || buildingProperties.defenceType == BuildingProperties.DefenceType.Energy || 
             buildingProperties.defenceType == BuildingProperties.DefenceType.Flamethrower || buildingProperties.defenceType == BuildingProperties.DefenceType.MilitaryTower ||
             buildingProperties.defenceType == BuildingProperties.DefenceType.Mortar)
            tower = buildingProperties.levels[buildingProperties.level - 1].transform.Find("Tower");

        position = new Vector2(transform.position.x, transform.position.z);

        timer = currentGunIndex / 100;
    }

    public IEnumerator FindEnemiesDelay()
    {
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        StartCoroutine(Find());
        yield return null;
    }

    private void Update()
    {
        if (tower != null && enemy != null)
            tower.LookAt(new Vector3(enemy.position.x, enemy.position.y + 1, enemy.position.z));
    }
    
    IEnumerator Find()
    {
        while (true)
        {
            if (enemy == null)
            {
                if (buildingProperties.canAttackLand)
                {
                    minValue = float.MaxValue;

                    for (int i = 0; i < gunsController.landEnemies.Count; i++)
                    {
                        if (gunsController.landEnemies[i] != null)
                        {
                            enemyPosition = new Vector2(gunsController.landEnemies[i].position.x, gunsController.landEnemies[i].position.z);

                            distance = (enemyPosition.x - position.x) * (enemyPosition.x - position.x) + (enemyPosition.y - position.y) * (enemyPosition.y - position.y);

                            if (distance < minValue)
                            {
                                minValue = distance;
                                enemyIndex = i;
                            }
                        }
                    }

                    if (minValue < rangePow)
                    {
                        enemy = gunsController.landEnemies[enemyIndex];
                        attack = true;
                        warriorProperties = gunsController.landEnemies[enemyIndex].GetComponent<WarriorProperties>();
                    }
                }
                
                if (buildingProperties.canAttackFly)
                {
                    minValue = float.MaxValue;

                    for (int i = 0; i < gunsController.flyEnemies.Count; i++)
                    {
                        if (gunsController.flyEnemies[i] != null)
                        {
                            enemyPosition = new Vector2(gunsController.flyEnemies[i].position.x, gunsController.flyEnemies[i].position.z);

                            distance = (enemyPosition.x - position.x) * (enemyPosition.x - position.x) + (enemyPosition.y - position.y) * (enemyPosition.y - position.y);

                            if (distance < minValue)
                            {
                                minValue = distance;
                                enemyIndex = i;
                            }
                        }
                    }

                    if (minValue < rangePow)
                    {
                        enemy = gunsController.flyEnemies[enemyIndex];
                        attack = true;
                        warriorProperties = gunsController.flyEnemies[enemyIndex].GetComponent<WarriorProperties>();
                    }
                }
            }
            else
            {
                if (attack)
                {
                    StopCoroutine(attackCorutine);
                    attack = false;
                    attackCorutine = Attack();
                    StartCoroutine(attackCorutine);
                }

                enemyPosition = new Vector2(warriorProperties.transform.position.x, warriorProperties.transform.position.z);
                distance = (enemyPosition.x - position.x) * (enemyPosition.x - position.x) + (enemyPosition.y - position.y) * (enemyPosition.y - position.y);

                if (distance > rangePow)
                {
                    enemy = null;
                    StopCoroutine(attackCorutine);
                }
            }
            
            yield return new WaitForSeconds(0.5f);
        }
    }
    
    public IEnumerator Attack()
    {
        while (enemy != null)
        {
            if(PS != null)
                PS.Play();

            //send bullet
            buildingBullets[currentBullet].ps[0].Play(true);

            if (buildingBullets[currentBullet].psTrigger != null)
                buildingBullets[currentBullet].psTrigger.Stop(true);

            bullets[currentBullet].transform.position = tower.position;
            bullets[currentBullet].transform.rotation = tower.rotation;
            buildingBullets[currentBullet].targetPosSaved = enemy.position; 
            if (buildingProperties.defenceType == BuildingProperties.DefenceType.Mortar)
            {
                buildingBullets[currentBullet].mortar = true;
                buildingBullets[currentBullet].distance = Vector3.Distance(transform.position, enemy.position);
            }
            buildingBullets[currentBullet].coef = 3;
            buildingBullets[currentBullet].startAttack = startAttack;
            buildingBullets[currentBullet].sendDamage = true;
            buildingBullets[currentBullet].target = enemy;
            buildingBullets[currentBullet].damage = buildingProperties.damage;
            if(warriorProperties.fly)
                buildingBullets[currentBullet].bias = 3.5f;
            else
                buildingBullets[currentBullet].bias = 1;
            for (int i = 0; i < buildingBullets[currentBullet].ps.Length; i++)
            {
                var main = buildingBullets[currentBullet].ps[i].main;
                main.loop = true;
            }
            if (currentBullet < bulletsArrayLength - 1)
                currentBullet += 1;
            else
                currentBullet = 0;

            yield return new WaitForSeconds(attackSpeed);
        }
    }

}
