using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skin Settings",menuName = "Skin Settings")]
public class SkinSO : ScriptableObject
{
    [SerializeField] private List<Data> ModesSettings;
    public PauseType PauseType;
    public float PauseDuration;
    public ModeType ChangeToMode;


    public Data GetData()
    {
        return ModesSettings.Find(d => d.Mode == ModeController.Mode);
    }
}

[Serializable]
public class Data
{
    public ModeType Mode;
    public string Path;
    public float AnimSpeed;
    public Vector2 GifScale = new Vector2 (1,1);
    public AudioClip AudioClip;
    public Sprite OffSkin;
    public Vector2 OffSkinScale = new Vector2(1,1);
}



public enum PauseType
{
    AfterAnimation,
    BeforeAnimation
}
    

