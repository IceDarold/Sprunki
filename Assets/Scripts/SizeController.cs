using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeController : MonoBehaviour
{
    private Camera _cam;
    private int currentScreenWidth;
    private int currentScreenHeight;

    // Укажите здесь базовое разрешение и соответствующее поле зрения
    private float baseResolutionAspectRatio;
    private float baseFOV = 60f;
    public float maxFOV = 80f;
    public float minFOV = 53f;
    void Start()
    {
        _cam = GetComponent<Camera>();
        currentScreenWidth = Screen.width;
        currentScreenHeight = Screen.height;
        baseResolutionAspectRatio = (float)currentScreenWidth / currentScreenHeight;

        baseFOV = _cam.fieldOfView;
        Debug.Log(currentScreenHeight.ToString() + " " + currentScreenWidth.ToString());
        UpdateFOV();
    }

    // Update is called once per frame
    void Update()
    {
        if (Screen.width != currentScreenWidth || Screen.height != currentScreenHeight)
        {
            currentScreenWidth = Screen.width;
            currentScreenHeight = Screen.height;

            Debug.Log(currentScreenHeight.ToString() + " " + currentScreenWidth.ToString());

            UpdateFOV();
        }
    }

    private void UpdateFOV()
    {
        // Текущее соотношение сторон
        float currentAspectRatio = (float)Screen.width / Screen.height;
        Debug.Log(currentAspectRatio.ToString() + " " + baseResolutionAspectRatio.ToString());
        // Корректируем поле зрения пропорционально изменению соотношения сторон
        float newFov = baseFOV * (baseResolutionAspectRatio / currentAspectRatio);
        _cam.fieldOfView = Mathf.Clamp(newFov, minFOV, maxFOV);
    }
}
