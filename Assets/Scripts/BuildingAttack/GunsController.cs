using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunsController : MonoBehaviour
{

    public List<Transform> landEnemies = new List<Transform>();
    public List<Transform> flyEnemies = new List<Transform>();
    // public Vector2[] landEnemiesPosition;

    public List<Transform> guns = new List<Transform>();
    Vector2[] gunsPositions;
    int[] gunsRange;


    //compute shader
    public ComputeShader _shader;
    ComputeBuffer enemiesPositions, enemyIndexFromShader, gunPositionBuffer;
    private int kiCalc;
    float[] enemyDistance;
    float minValue;

    //other
    int enemyIndex;

}
