using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Rendering;


public class CharacterAppearance : MonoBehaviour
{
    [SerializeField] private Sprite DefaultSprite;
    [SerializeField] private Sprite GlowSprite;
    [SerializeField] private float SkinRemoveAnimationSpeed;

    private SpriteRenderer spriteRenderer;
    private SkinSO skinData;

    private int currentFrame = -1;
    private float currentTime;

    private Vector2 cachedScale;
    private Vector3 cachedPosition;

    private float GifTime;

    private Coroutine MainAnim;
    private List<KeyValuePair<Sprite,float>> textures;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        
        if(currentFrame != -1)
        {
            transform.localScale = cachedScale * skinData.GetData().GifScale;
            transform.position = cachedPosition + skinData.GetData().Offset;
        }
    }

    public void SetSprite(bool isGlow)
    {
        if(isGlow)
        {
            spriteRenderer.sprite = GlowSprite;
        }
        else
        {
            spriteRenderer.sprite = DefaultSprite;
        }
    }

    public void AddSkin(SkinSO skin, Action callback )
    {
        if(skin == null)
        {
            Debug.LogError("Add at least one sprite for animation");
            return;
        }

        skinData = skin;
        
        currentTime = 0f;
        cachedScale = transform.localScale;
        cachedPosition = transform.position;


        

        //spriteRenderer.sprite = skinData.GetData().Animation[currentFrame];

        string gifPath = Application.dataPath + skin.GetData().Path;
        byte[] data = File.ReadAllBytes(gifPath);
        StartCoroutine(UniGif.GetTextureListCoroutine(data, (list, loopCount, width, height) =>
        {
            textures = new List<KeyValuePair<Sprite,float>>();
            foreach (var item in list)
            {
                var sprite = Sprite.Create(item.m_texture2d, new Rect(0, 0, item.m_texture2d.width, item.m_texture2d.height),
                new Vector2(0.5f, 0.5f),100,1,SpriteMeshType.Tight);

                
                textures.Add(new KeyValuePair<Sprite, float>(sprite,item.m_delaySec));
                GifTime += item.m_delaySec;
                
            }

            GifTime += skinData.GetData().PauseDuration;

            callback?.Invoke();
            transform.position += skinData.GetData().Offset;
            MainAnim = StartCoroutine(MainAnimation());
        }));

        
    }

    public void RemoveSkin()
    {
        
        currentFrame = -1;
        currentTime = 0f;
        Debug.Log("Remove Skin");
        if(MainAnim != null)
        {
            StopCoroutine(MainAnim);
            MainAnim = null;
        }
        
        StartCoroutine(RemoveSkinAnimation());
    }

    public void Mute(bool isMute)
    {
        if(isMute)
        {
            StopCoroutine(MainAnim);
            spriteRenderer.sprite = skinData.GetData().OffSkin;

            currentFrame = -1;
            currentTime = 0f;

            transform.localScale = cachedScale * skinData.GetData().OffSkinScale;
        }
        else
        {
            transform.localScale = cachedScale * skinData.GetData().GifScale;
            currentFrame = 0;
            Debug.Log("UnMute");
            //spriteRenderer.sprite = skinData.GetData().Animation[currentFrame];
            MainAnim = StartCoroutine(MainAnimation());
        }
    }


    private IEnumerator MainAnimation()
    {
        float delta = MusicSyncController.Time % GifTime;
        if(delta != 0)
        {
            Debug.Log(delta);
        }
        
        while (true)
        {
            if (skinData.PauseType == PauseType.BeforeAnimation)
            {
                float pause = delta - skinData.GetData().PauseDuration >= 0 ? 0f : skinData.GetData().PauseDuration - delta;
                delta = Mathf.Max(delta -  skinData.GetData().PauseDuration, 0f);
                yield return new WaitForSeconds(pause);
            }


            bool flag = false;
            foreach (var item in textures)
            {
                
                float a = 0f;

                if(delta - item.Value / skinData.GetData().AnimSpeed > 0)
                {
                    delta -= item.Value / skinData.GetData().AnimSpeed;
                    continue;
                }
                else if(delta != 0f)
                {
                    a = delta - item.Value / skinData.GetData().AnimSpeed;
                    delta = 0f;
                }

                //Debug.Log(i);
               
                spriteRenderer.sprite = item.Key;
                transform.localScale = skinData.GetData().GifScale * cachedScale;
                currentFrame = 0;
                yield return new WaitForSeconds(item.Value / skinData.GetData().AnimSpeed + a);
                flag = true;

            }

            if (!flag)
            {
                spriteRenderer.sprite = textures[textures.Count - 1].Key;
                transform.localScale = skinData.GetData().GifScale * cachedScale;
            }

            //spriteRenderer.sprite = skinData.GetData().Animation[0];

            if (skinData.PauseType == PauseType.AfterAnimation)
            {
                float pause = delta - skinData.GetData().PauseDuration >= 0 ? 0f : skinData.GetData().PauseDuration - delta;
                delta = Mathf.Max(delta - skinData.GetData().PauseDuration, 0f);
                yield return new WaitForSeconds(pause);
            }
        }
        
    }

    private IEnumerator RemoveSkinAnimation()
    {
        float distance = 5f;
        Vector3 startPos = cachedPosition;
        Vector3 goal = startPos - new Vector3(0, distance, 0);
        while (Vector3.Distance(transform.position, goal) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position,goal , SkinRemoveAnimationSpeed * Time.deltaTime);
            yield return null;
        }

        spriteRenderer.sprite = DefaultSprite;
        transform.localScale = cachedScale;
        

        while(Vector3.Distance(transform.position, startPos) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position,startPos,SkinRemoveAnimationSpeed * Time.deltaTime);
            yield return null;
        }

        yield break;
    }



}
