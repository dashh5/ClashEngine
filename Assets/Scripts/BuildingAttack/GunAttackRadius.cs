using UnityEngine;
using System.Collections;


// This script is to visually represent the attack or 
// effect radius of an item in a game, providing clear feedback to 
// the player.By toggling the active state of the GameObject, the circle 
// can be shown or hidden as needed, such as when a player selects or 
// deselects a weapon. The color and size of the circle are customizable, 
// providing flexibility for different use cases or game aesthetics.





// Require a LineRenderer component to be attached to the same GameObject
[RequireComponent(typeof(LineRenderer))]
public class GunAttackRadius : MonoBehaviour
{
    [Range(0, 100)]
    public int segments = 50;  // Number of segments to create the circle
    [Range(0, 100)]
    public float radius = 5;   // Radius of the circle
    LineRenderer line;         // Reference to the LineRenderer component

    public Color color;        // Color of the LineRenderer
    
    // Start is called before the first frame update
    void Start()
    {
        // Get the LineRenderer component attached to this GameObject
        line = gameObject.GetComponent<LineRenderer>();

        // Set the number of positions to be one more than the number of segments
        line.positionCount = segments + 1;
        // Use local space rather than world space
        line.useWorldSpace = false;

        // Create a new material with a default shader and set it as the LineRenderer's material
        line.material = new Material(Shader.Find("Sprites/Default"));
        // Set the color of the material
        line.sharedMaterial.SetColor("_Color", color);

        // Call the CreatePoints function to calculate and set the positions of the LineRenderer
        CreatePoints();

        // Initially set the GameObject to inactive
        gameObject.SetActive(false);
    }

    // Calculate and set the positions for the LineRenderer to create a circle
    public void CreatePoints()
    {
        // Variables to store the x and y positions of each point
        float x;
        float y;

        // Starting angle for the first point
        float angle = 20f;

        // Loop through each segment to calculate and set the positions
        for (int i = 0; i < (segments + 1); i++)
        {
            // Calculate the x and y positions based on the current angle
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
            y = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;

            // Set the position of the current point in the LineRenderer
            line.SetPosition(i, new Vector3(x, y, 0));

            // Increment the angle for the next point
            angle += (360f / segments);
        }
    }
}
