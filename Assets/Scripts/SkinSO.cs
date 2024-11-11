using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skin Settings",menuName = "Skin Settings")]
public class SkinSO : ScriptableObject
{
    [SerializeField] private List<Data> ModesSettings;
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
    public List<Sprite> Animation;
    public AudioClip AudioClip;
    public Sprite OffSkin;
}
    

