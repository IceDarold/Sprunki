using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "GIF Animation Clip", menuName = "GIF Animation Clip")]
public class GIFAnimationClip : ScriptableObject
{
    public List<GifFrame> Animation;

    public float Length
    {
        get 
        {
            float res = 0f;
            for (int i = 0; i < Animation.Count; i++)
            {
                res += Animation[i].Duration;
            }
            return res;
        }
    }





}

[Serializable]
public class GifFrame
{
    public Sprite Sprite;
    public float Duration;
}
