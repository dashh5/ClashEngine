using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


// This script is responsible for the behavior of a trap in a game. 
// The trap deals damage to enemies within a certain range after a 
// specified timer runs out. It can handle multiple enemies, targeting 
// the nearest one when there are multiple in range. The script uses a 
// coroutine to continuously check for enemies and handle the damage 
// dealing process. Additionally, there is functionality to visualize 
// the trap's range in the Unity Editor. The trap deactivates itself 
// after dealing damage.

public class TrapBehaviour : MonoBehaviour
{
    // Public variables to set properties of the trap, including its range, damage, and timer for dealing damage.
    public BuildingProperties buildingProperties;
    public int rangeDistance = 1;
    public float timerToDamage = 1;

    // A Transform variable to keep track of the enemy target.
    Transform enemy;

    // A reference to the GunsController script.
    GunsController gunsController;

    // Initialization method
    private void Start()
    {
        // Finding and assigning the GunsController script from the scene.
        gunsController = FindObjectOfType<GunsController>();
        // Starting the coroutine to find and damage enemies.
        StartCoroutine(Find());
    }

    // Method to visualize the trap's range when selected in the Unity Editor.
    void OnDrawGizmosSelected()
    {
        // Drawing a yellow sphere at the trap's position with a radius of 1 unit.
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 1);
    }

    // Coroutine to find and damage enemies.
    IEnumerator Find()
    {
        // An infinite loop to constantly check for enemies.
        while (true)
        {
            // Checking if the attack has started and there are land enemies present.
            if (StartAttack.isAttack && gunsController.landEnemies.Count != 0)
            {
                // If there are more than one enemies, find the nearest one. Otherwise, target the only enemy present.
                if (gunsController.landEnemies.Count > 1)
                    enemy = GetNearestEnemy();
                else
                    enemy = gunsController.landEnemies[0];

                // If an enemy is found and it is within the trap's range
                if (enemy != null)
                {
                    if (Vector3.Distance(enemy.position, this.transform.position) < rangeDistance)
                    {
                        // Play the trap's particle system.
                        ParticleSystem ps = buildingProperties.levels[buildingProperties.level - 1].transform.GetChild(0).GetComponent<ParticleSystem>();
                        ps.transform.parent = null;
                        ps.Play();

                        // Wait for the timer to run out before dealing damage.
                        while (timerToDamage > 0)
                        {
                            timerToDamage -= Time.deltaTime;
                            yield return new WaitForEndOfFrame();
                        }

                        // Deal damage to all land enemies within the trap's range.
                        for (int i = 0; i < gunsController.landEnemies.Count; i++)
                            if (Vector3.Distance(gunsController.landEnemies[i].position, this.transform.position) < rangeDistance)
                                gunsController.landEnemies[i].GetComponent<WarriorProperties>().HP -= buildingProperties.damage;

                        // Deactivate the trap after it has dealt damage.
                        buildingProperties.gameObject.SetActive(false);
                    }
                }
            }

            // Wait for the next frame before running the loop again.
            yield return new WaitForEndOfFrame();
        }
    }

    // Method to find the nearest enemy to the trap.
    Transform GetNearestEnemy()
    {
        // Using LINQ to find the enemy with the smallest distance to the trap.
        return gunsController.landEnemies.Aggregate((o1, o2) => Vector3.Distance(o1.transform.position, this.transform.position) > Vector3.Distance(o2.transform.position, this.transform.position) ? o2 : o1);
    }
}

