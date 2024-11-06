using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skin Settings",menuName = "Skin Settings")]
public class SkinSO : ScriptableObject
{
    public List<Sprite> Animation;
    public AudioClip  AudioClip;
    public Sprite OffSkin;
}
    

