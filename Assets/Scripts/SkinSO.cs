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
    public ModeType ChangeToMode;


    public void LoadSkinAnims()
    {
        foreach (var mode in ModesSettings)
        {
            if(mode.Path != "")
            {
                GifDataLoader.LoadGifData(mode.Path, mode.LoadData);
            }
            
        }
    }

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
    public Sprite DefaultSprite;
    public float AnimSpeed;
    public Vector2 GifScale = new Vector2 (1,1);
    public AudioClip AudioClip;
    public float PauseDuration;
    public Sprite OffSkin;
    public Vector2 OffSkinScale = new Vector2(1,1);
    public Vector3 Offset;

    [NonSerialized]
    public List<KeyValuePair<Sprite, float>> Animation;

    [NonSerialized]
    public float GifTime;

    public void LoadData(List<UniGif.GifTexture> textures)
    {
        Animation = new List<KeyValuePair<Sprite, float>>();

        foreach (var item in textures)
        {
            var sprite = Sprite.Create(item.m_texture2d, new Rect(0, 0, item.m_texture2d.width, item.m_texture2d.height),
            new Vector2(0.5f, 0.5f), 100, 1, SpriteMeshType.Tight);


            Animation.Add(new KeyValuePair<Sprite, float>(sprite, item.m_delaySec));
            GifTime += item.m_delaySec;

        }

        GifTime += PauseDuration;
    }

}



public enum PauseType
{
    AfterAnimation,
    BeforeAnimation
}
    

