using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]
public class GunAttackRadius : MonoBehaviour
{
    [Range(0, 100)]
    public int segments = 50;
    [Range(0, 100)]
    public float radius = 5;
    LineRenderer line;

    public Color color;

    void Start()
    {
        line = gameObject.GetComponent<LineRenderer>();

        line.positionCount = segments + 1;
        line.useWorldSpace = false;

        line.material = new Material(Shader.Find("Sprites/Default"));
        line.sharedMaterial.SetColor("_Color", color);

        CreatePoints();

        gameObject.SetActive(false);
    }

    public void CreatePoints()
    {
        float x;
        float y;

        float angle = 20f;

        for (int i = 0; i < (segments + 1); i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
            y = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;

            line.SetPosition(i, new Vector3(x, y, 0));

            angle += (360f / segments);
        }
    }
}