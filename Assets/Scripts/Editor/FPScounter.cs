using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FPScounter : MonoBehaviour
{
    [SerializeField] private Text fpsText;
    [SerializeField] private float hudRefreshRate = 1f;

    private float timer;

    private void Update()
    {
        if (Time.unscaledTime > timer)
        {
            int fps = (int)(1f / Time.unscaledDeltaTime);
            fpsText.text = fps + " FPS";
            timer = Time.unscaledTime + hudRefreshRate;
        }
    }
}
