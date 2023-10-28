using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script is responsible for handling the attacking 
// mechanics of a gun in a game. It covers targeting of both land and 
// flying enemies, attacking them using bullets, and visual effects using 
// particle systems. The script makes use of coroutines for delayed 
// execution and managing attack intervals. The gun's tower (if any) is 
// rotated to face the current target enemy. The bullets are managed 
// efficiently through an object pooling mechanism, and various properties 
// such as attack range, attack speed, and damage are configurable and can 
// be updated upon upgrading the gun. The script interacts with other 
// components and scripts to obtain necessary information and to integrate 
// with the broader game mechanics. The gun is capable of targeting the 
// closest enemy within its attack range, providing an automated and 
// dynamic combat experience.

// Declare GunBehaviour class, inheriting from MonoBehaviour
public class GunBehaviour : MonoBehaviour {
    // Declare public and private variables
    public Transform enemy;
    public static int gunIndexes;
    public int currentGunIndex;
    public ParticleSystem PS;
    private Transform tower;
    private bool attack;
    private IEnumerator attackCoroutine;
    private Vector2 position, enemyPosition;
    private int rangePow, range;
    private float minValue, distance, timer;
    private int enemyIndex;
    private GunsController gunsController;
    private BuildingProperties buildingProperties;
    private StartAttack startAttack;
    private WarriorProperties warriorProperties;
    private int attackDamage;
    private float attackSpeed;
    private GameObject[] bullets;
    private BuildingBullet[] buildingBullets;
    public int bulletsArrayLength = 3;
    private int currentBullet;

    // In Start method:
    private void Start() {
        // Initialize variables and setup references
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
        attackCoroutine = Attack();

        // Initialize and setup bullets array
        bullets = new GameObject[bulletsArrayLength];
        buildingBullets = new BuildingBullet[bulletsArrayLength];
        if (buildingProperties.bullet != null) {
            for (int i = 0; i < bulletsArrayLength; i++) {
                bullets[i] = Instantiate(buildingProperties.bullet, Vector3.zero, Quaternion.identity, startAttack.bulletPooler);
                buildingBullets[i] = bullets[i].GetComponent<BuildingBullet>();
                buildingBullets[i].targetIsBuilding = false;
                // bullets[i].SetActive(false);
            }
        }
    }

    // CountAfterUpgrade method:
    public void CountAfterUpgrade() {
        // Calculate range and setup position
        rangePow = buildingProperties.range * buildingProperties.range;
        range = buildingProperties.range;
        if (buildingProperties.defenceType == BuildingProperties.DefenceType.Cannon /* ... other types ... */ || buildingProperties.defenceType == BuildingProperties.DefenceType.Mortar) {
            tower = buildingProperties.levels[buildingProperties.level - 1].transform.Find("Tower");
        }
        position = new Vector2(transform.position.x, transform.position.z);
        timer = currentGunIndex / 100f;
    }

    // FindEnemiesDelay coroutine:
    public IEnumerator FindEnemiesDelay() {
        // Wait for timer to finish
        while (timer > 0) {
            timer -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        StartCoroutine(Find());
        yield return null;
    }

    // Update method:
    private void Update() {
        // Rotate tower to face enemy
        if (tower != null && enemy != null) {
            tower.LookAt(new Vector3(enemy.position.x, enemy.position.y + 1, enemy.position.z));
        }
    }

    // Find coroutine:
    IEnumerator Find() {
        // Infinite loop to find enemies
        while (true) {
            // ... (Logic to find closest enemy within range and start/stop attack coroutine) ...
            yield return new WaitForSeconds(0.5f);
        }
    }

    // Attack coroutine:
    public IEnumerator Attack() {
        // Infinite loop to attack enemy
        while (enemy != null) {
            // ... (Logic to attack enemy, manage bullets, and play particle systems) ...
            yield return new WaitForSeconds(attackSpeed);
        }
    }
}

