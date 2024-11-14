using System.Collections;
using System.Collections.Generic;
using System.IO;
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
            transform.localScale = cachedScale * skinData.ImageScale;
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
        if(skin == null || skin.GetData().Animation.Count == 0)
        {
            Debug.LogError("Add at least one sprite for animation");
            return;
        }

        skinData = skin;
        
        currentTime = 0f;
        cachedScale = transform.localScale;

        //spriteRenderer.sprite = skinData.GetData().Animation[currentFrame];

        string gifPath = Application.dataPath + skin.path;
        byte[] data = File.ReadAllBytes(gifPath);
        StartCoroutine(UniGif.GetTextureListCoroutine(data, (list, loopCount, width, height) =>
        {
            textures = new List<KeyValuePair<Sprite,float>>();
            foreach (var item in list)
            {
                var sprite = Sprite.Create(item.m_texture2d, new Rect(0, 0, item.m_texture2d.width, item.m_texture2d.height),
                new Vector2(0.5f, 0.5f));

                textures.Add(new KeyValuePair<Sprite, float>(sprite,item.m_delaySec));
                
            }
            
            
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
        }
        else
        {
            currentFrame = 0;
            spriteRenderer.sprite = skinData.GetData().Animation[currentFrame];
            MainAnim = StartCoroutine(MainAnimation());
        }
    }


    private IEnumerator MainAnimation()
    {
        
        
        while (true)
        {
            if (skinData.PauseType == PauseType.BeforeAnimation)
            {
                yield return new WaitForSeconds(skinData.PauseDuration);
            }

            foreach (var item in textures)
            {
                
                spriteRenderer.sprite = item.Key;
                transform.localScale = skinData.ImageScale * cachedScale;
                currentFrame = 0;
                yield return new WaitForSeconds(item.Value / skinData.AnimSpeed);

            }

            //spriteRenderer.sprite = skinData.GetData().Animation[0];

            if (skinData.PauseType == PauseType.AfterAnimation)
            {
                yield return new WaitForSeconds(skinData.PauseDuration);
            }
        }
        
    }

    private IEnumerator RemoveSkinAnimation()
    {
        float distance = 5f;
        Vector3 startPos = transform.position;
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
