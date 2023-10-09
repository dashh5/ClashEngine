using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilderUIController : MonoBehaviour
{

    void Update()
    {
        Vector3 relativePos = Camera.main.transform.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(relativePos);
        transform.rotation = rotation;

    }

}
