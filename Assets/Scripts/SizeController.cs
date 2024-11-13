using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeController : MonoBehaviour
{
    [SerializeField] private Vector2 referenceSize;
    [SerializeField] private float referenceFOV;

    private Camera _cam;
    private int currentScreenWidth;
    private int currentScreenHeight;

    // Укажите здесь базовое разрешение и соответствующее поле зрения
    private float baseResolutionAspectRatio;
    
    public float maxFOV = 80f;
    public float minFOV = 53f;
    void Start()
    {
        _cam = GetComponent<Camera>();
        currentScreenWidth = Screen.width;
        currentScreenHeight = Screen.height;
        baseResolutionAspectRatio = (float)referenceSize.x / referenceSize.y;

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
        float newFov = referenceFOV * (baseResolutionAspectRatio / currentAspectRatio);
        _cam.fieldOfView = Mathf.Clamp(newFov, minFOV, maxFOV);
    }
}
