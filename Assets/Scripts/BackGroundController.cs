using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackGroundController : MonoBehaviour
{
    [SerializeField] private Canvas parentCanvas;
    [SerializeField] private float AnimTime;
    [SerializeField] private List<ModeBackground > backgrounds = new List<ModeBackground>();
    private Image image;

    private void Start()
    {
        image = GetComponent<Image>();
        image.sprite = backgrounds.Find(e => e.ModeType == ModeController.Mode).Sprite;
        ModeController.OnModeChanged += ChangeBackground;
    }


    private void OnDestroy()
    {
        ModeController.OnModeChanged -= ChangeBackground;
    }



    private void ChangeBackground()
    {
        StartCoroutine(Anim());
        
    }


    private IEnumerator Anim()
    {
        parentCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        image.color = Color.black;
        yield return new WaitForSeconds(AnimTime);

        parentCanvas.renderMode = RenderMode.ScreenSpaceCamera;
        image.color = Color.white;
        var res = backgrounds.Find(e => e.ModeType == ModeController.Mode);
        Debug.Log(res);
        image.sprite = res.Sprite;

    }
}

[Serializable]
public class ModeBackground
{
    public ModeType ModeType;
    public Sprite Sprite;
}
