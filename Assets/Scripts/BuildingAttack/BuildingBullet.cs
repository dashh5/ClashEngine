using System.Collections;
using System.Collections.Generic;
using UnityEngine;



// Summary: 

    // This script defines the behavior of a bullet in a game 
    // inspired by "Clash of Clans". The bullet moves towards its 
    // target and delivers damage when it reaches its destination. 
    // There's special behavior if the bullet is a "mortar" type, and 
    // there are particle effects associated with the bullet that 
    // can be triggered or looped.




// DEFINE a new class called BuildingBullet that inherits from 'MonoBehaviour'.


    // 'MonoBehaviour' is a foundational class in Unity that provides the essential 
    // functionalities and integrations for creating interactive behaviors in the Unity engine. 
    // Almost every script you write in Unity will derive from MonoBehaviour if it's intended to 
    // interact directly with game objects and utilize Unity's event-driven system


    // When something is declared 'public', it means that it can be accessed and used by other classes 
    // and methods from any part of the program or even outside assemblies.  

public class BuildingBullet : MonoBehaviour
{


    // DECLARE public variables for bullet speed, particle systems, damage amount, and more.

    public float speed;
    public ParticleSystem[] ps;
    public ParticleSystem psTrigger;
    public int damage;

    // DECLARE hidden variables that control specific game mechanics.

        // By making them hidden, the script's author ensures that they 
        // aren't altered unintentionally from the Unity Editor but can 
        // still be accessed and manipulated by other scripts if necessary.

    [HideInInspector]
    public bool targetIsBuilding; //  Indicates if the current target of the bullet is a building
    [HideInInspector]
    public Transform target; // A reference to the Transform component of the target the bullet is heading towards
    [HideInInspector]
    public bool sendDamage; // Determines if the bullet should send damage to its target
    [HideInInspector]
    public bool flyTarget; // It's not explicitly used in the provided script, but it might indicate if the target is a flying unit
    [HideInInspector]
    public float bias = 1; // A value to adjust the height (y-axis) of the target position
    [HideInInspector]
    public bool mortar; // Indicates if the bullet is from a mortar-type weapon
    [HideInInspector]
    public Vector3 targetPosSaved; // Stores the position of the target
    // Vector3 is a structure provided by Unity to represent a three-dimensional point or direction using three floating-point numbers (x, y, z).
    [HideInInspector]
    public float distance; // It's not explicitly used in the provided script, but it might represent the distance between the bullet and its target
    [HideInInspector]
    public float coef; // A coefficient, which in the script affects the vertical movement of the mortar bullet
    [HideInInspector]
    public StartAttack startAttack; // A reference to another script or component that manages the attack mechanics

    // EVERY frame update:

    void Update()
    {
        // IF there is a target and it's set to receive damage:

        if (target != null && sendDamage)
        {
            // DETERMINE the target's position with a bias on the y-axis.

            Vector3 targetPosition = new Vector3(target.position.x, target.position.y + bias, target.position.z);

            // IF the bullet is from a mortar:

            if (mortar)
            {
            
            // MOVE the bullet towards a saved target position.

                transform.position = Vector3.MoveTowards(transform.position, targetPosSaved, speed * Time.deltaTime);
                coef -= 0.02f;
                transform.Translate(Vector3.up * Time.deltaTime * coef);
                
            // IF bullet is close to the target position:

                if (Vector3.Distance(transform.position, targetPosSaved) < 0.1f)
                {
                
                // STOP all looping particle effects.

                    LoopSetFalse();

                
                // FOR each created warrior in the game:


                    for (int i = 0; i < startAttack.createdWarriors.Count; i++)
                    {
                // IF the warrior exists and isn't flying:

                        if(startAttack.createdWarriors[i] != null)
                            if(startAttack.createdWarriors[i].GetComponent<WarriorProperties>().fly == false)

                // IF the warrior is close to the bullet's explosion:

                                if(Vector3.Distance(transform.position, startAttack.createdWarriors[i].transform.position) < 2)
                
                // REDUCE the warrior's health by the bullet's damage amount.

                                    startAttack.createdWarriors[i].GetComponent<WarriorProperties>().HP -= damage;
                    }
                
                // TRIGGER an explosion particle effect (if available).


                    if (psTrigger != null)
                        psTrigger.Play(true);

                
                // STOP the bullet from causing further damage.


                    sendDamage = false;
                }
            }
            
            // ELSE (if the bullet isn't from a mortar):

            else
            {
    
            // MOVE the bullet towards its target.

                transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

            // IF bullet has reached its target:

                if (transform.position == targetPosition)
                {

                // STOP all looping particle effects.

                    LoopSetFalse();
        
                // IF the target is a building:

                // Deal damage to the target, differentiating between buildings and warriors

                    if (targetIsBuilding)
                        target.GetComponent<BuildingProperties>().HP -= damage;
                    else
                        target.GetComponent<WarriorProperties>().HP -= damage;

                    // Trigger a particle effect upon impact if available

                    if (psTrigger != null)
                        psTrigger.Play(true);
                    
                     // Prevent the bullet from dealing further damage

                    sendDamage = false;
                }
            }

        }
        else // If there is no target or the bullet is not set to deal damage
        {

     // Additional behavior when the bullet does not have an active target or is not set to deal damage


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

    // Method to stop all looping particle effects

    void LoopSetFalse()
    {
        // Iterate through each particle system in the array

        for (int i = 0; i < ps.Length; i++)
        {
        // Access the main module of the current particle system

            var main = ps[i].main;
        
        // Set the loop property to false, stopping the particle system from looping

            main.loop = false;
        }
    }


}
