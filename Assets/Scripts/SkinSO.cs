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
    public Vector2 ImageScale = new Vector2(1,1);

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
    public AudioClip AudioClip;
    public Sprite OffSkin;
}



public enum PauseType
{
    AfterAnimation,
    BeforeAnimation
}
    

