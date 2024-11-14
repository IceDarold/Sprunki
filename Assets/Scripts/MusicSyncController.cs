using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class MusicSyncController : MonoBehaviour
{
    [SerializeField] private bool useSync = true;
    [SerializeField] private bool useSelfSync = true;
    [SerializeField] private Text TestTimeText;
    
    public static float Time {  get; private set; }

    public static bool UseSync => instance.useSync;
    public static bool UseSelfSync => instance.useSelfSync;

    private static MusicSyncController instance;
    
    private int character = 0;
    private int playing = 0;

    private void Awake()
    {
        instance = this;

    }

    private void Update()
    {

        if(TestTimeText != null)
        {
            TestTimeText.text = Time.ToString();
        }
        if(instance.character == 0)
        {
            Time = 0f;
            return;
        }

        if(instance.playing > 0)
        {
            Time += UnityEngine.Time.deltaTime;
            
        }

        
    }


    public static void Add()
    {
        instance.character++;
        instance.playing++;
        

        
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
