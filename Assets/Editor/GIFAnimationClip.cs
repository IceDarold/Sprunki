using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "GIF Animation Clip", menuName = "GIF Animation Clip")]
public class GIFAnimationClip : ScriptableObject
{
    [SerializeField] private List<GifFrame> Animation;


    private void OnEnable()
    {
#if UNITY_EDITOR

        string guid = "e67d532371448c74da39196fff87acf5";
        string iconPath = AssetDatabase.GUIDToAssetPath(guid);
        Texture2D icon = AssetDatabase.LoadAssetAtPath<Texture2D>(iconPath);
        if(icon != null)
        {
            EditorGUIUtility.SetIconForObject(this, icon);
        }

#endif
    }

    public float Length
    {
        get 
        {
            if(Animation == null)
            {
                Animation = new List<GifFrame>();
            }

            float res = 0f;
            for (int i = 0; i < Animation.Count; i++)
            {
                res += Animation[i].Duration;
            }
            return res;
        }
    }

    public int GetFrameIndex(float time)
    {
       int frame = 0;
        time = time % Length;

        for (int i = 0; i < Animation.Count;i++)
        {
            if(time - Animation[i].Duration > 0)
            {
                frame += 1;
                time -= Animation[i].Duration;
            }
            else
            {
                return frame;
            }
            
        }

        return frame;


    }

    public GifFrame GetFrame(float time)
    {
        int frame = GetFrameIndex(time);
        return Animation[frame];
    }


    public void Clear()
    {
        Animation.Clear();
    }

    public void AddAnimation(Sprite[] sprites, float[] durations)
    {
        if(sprites.Length != durations.Length)
        {
            return;
        }

        if(Animation == null)
        {
            Animation = new List<GifFrame> ();
        }

        for (int i = 0;i < sprites.Length;i++)
        {
            GifFrame frame = new GifFrame()
            {
                Sprite = sprites[i],
                Duration = durations[i]
            };

            Animation.Add(frame);
        }
    }


}

[Serializable]
public class GifFrame
{
    public Sprite Sprite;
    public float Duration;

    
}
