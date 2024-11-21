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


    private Coroutine MainAnim;
    //private List<KeyValuePair<Sprite,float>> textures;
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

    public void AddSkin(SkinSO skin)
    {
        if(skin == null)
        {
            Debug.LogError("Add at least one sprite for animation");
            return;
        }

        skinData = skin;
        
        currentTime = 0f;
        
        cachedScale = transform.localScale;
        Debug.Log(cachedScale);
        cachedPosition = transform.position;




        //spriteRenderer.sprite = skinData.GetData().Animation[currentFrame];

        

        transform.position += skinData.GetData().Offset;
        if(skinData.GetData().Animation != null) 
        {
            MainAnim = StartCoroutine(MainAnimation());
        }
        else
        {
            spriteRenderer.sprite = skinData.GetData().DefaultSprite;
            transform.localScale = skinData.GetData().GifScale * cachedScale;
            currentFrame = 0;
        }
        


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
            if(MainAnim != null)
            {
                StopCoroutine(MainAnim);
            }
            
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
            if (skinData.GetData().Animation != null)
            {
                MainAnim = StartCoroutine(MainAnimation());
            }
            else
            {
                spriteRenderer.sprite = skinData.GetData().DefaultSprite;
            }
            
        }
    }


    private IEnumerator MainAnimation()
    {
        float delta = MusicSyncController.Time % skinData.GetData().GifTime;
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
            foreach (var item in skinData.GetData().Animation)
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
                var textures = skinData.GetData().Animation;
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
        Debug.Log(cachedScale);

        while(Vector3.Distance(transform.position, startPos) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position,startPos,SkinRemoveAnimationSpeed * Time.deltaTime);
            yield return null;
        }

        yield break;
    }



}
