using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MusicSyncController : MonoBehaviour
{
    [SerializeField] private bool useSync = true;
    [SerializeField] private bool useSelfSync = true;
    
    public static float Time {  get; private set; }

    public static bool UseSync => instance.useSync;
    public static bool UseSelfSync => instance.useSelfSync;

    private static MusicSyncController instance;
    
    private int character = 0;
    private int playing = 0;
    private float _duration;
    private void Awake()
    {
        instance = this;

    }

    private void Update()
    {
        if(instance.character == 0)
        {
            Time = 0f;
            return;
        }

        if(instance.playing > 0)
        {
            Time += UnityEngine.Time.deltaTime;
            Time = Time % _duration;
            Debug.Log(Time);
        }
    }


    public static void Add(float duration)
    {
        instance.character++;
        instance.playing++;
        
        instance._duration = duration;

        
    }
    public static void Remove(bool isMute)
    {
        instance.character--;

        if(!isMute)
        {
            instance.playing--;
        }
    }

    public static void MuteState(bool isMute)
    {
        if (isMute)
        {
            instance.playing--;
        }
        else
        {
            instance.playing++;
        }
    }

}
