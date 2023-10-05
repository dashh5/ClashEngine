using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TrapBehaviour : MonoBehaviour
{

    public BuildingProperties buildingProperties;
    public int rangeDistance = 1;
    public float timerToDamage = 1;

    Transform enemy;

    GunsController gunsController;

    private void Start()
    {
        gunsController = FindObjectOfType<GunsController>();
        StartCoroutine(Find());
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 1);
    }

    IEnumerator Find()
    {
        while (true)
        {
            if (StartAttack.isAttack && gunsController.landEnemies.Count != 0)
            {
                if (gunsController.landEnemies.Count > 1)
                    enemy = GetNearestEnemy();
                else
                    enemy = gunsController.landEnemies[0];

                if (enemy != null)
                {
                    if (Vector3.Distance(enemy.position, this.transform.position) < rangeDistance)
                    {
                        ParticleSystem ps = buildingProperties.levels[buildingProperties.level - 1].transform.GetChild(0).GetComponent<ParticleSystem>();
                        ps.transform.parent = null;
                        ps.Play();

                        while (timerToDamage > 0)
                        {
                            timerToDamage -= Time.deltaTime;
                            yield return new WaitForEndOfFrame();
                        }

                        for (int i = 0; i < gunsController.landEnemies.Count; i++)
                            if (Vector3.Distance(gunsController.landEnemies[i].position, this.transform.position) < rangeDistance)
                                gunsController.landEnemies[i].GetComponent<WarriorProperties>().HP -= buildingProperties.damage;

                        buildingProperties.gameObject.SetActive(false);
                    }
                }
            }

            yield return new WaitForEndOfFrame();
        }
    }

    Transform GetNearestEnemy()
    {
        return gunsController.landEnemies.Aggregate((o1, o2) => Vector3.Distance(o1.transform.position, this.transform.position) > Vector3.Distance(o2.transform.position, this.transform.position) ? o2 : o1);
    }

}
