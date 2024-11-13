using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAppearance : MonoBehaviour
{
    [SerializeField] private Sprite DefaultSprite;
    [SerializeField] private Sprite GlowSprite;
    [SerializeField] private float SkinRemoveAnimationSpeed;

    private SpriteRenderer spriteRenderer;
    private SkinSO skinData;

    private int currentFrame = -1;
    private float currentTime;

    private Coroutine MainAnim;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        /*if(currentFrame != -1)
        {
            currentTime += Time.deltaTime;
            if(currentTime >= skinData.AnimFrameTime)
            {
                currentTime = 0f;
                currentFrame = (currentFrame + 1) % skinData.GetData().Animation.Count;
                spriteRenderer.sprite = skinData.GetData().Animation[currentFrame];
            }
        }*/

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
        currentFrame = 0;
        currentTime = 0f;

        spriteRenderer.sprite = skinData.GetData().Animation[currentFrame];

        MainAnim =  StartCoroutine(MainAnimation());
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

            foreach (var item in skinData.GetData().Animation)
            {
                spriteRenderer.sprite = item;
                yield return new WaitForSeconds(skinData.AnimFrameTime);

            }

            spriteRenderer.sprite = skinData.GetData().Animation[0];
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

        while(Vector3.Distance(transform.position, startPos) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position,startPos,SkinRemoveAnimationSpeed * Time.deltaTime);
            yield return null;
        }

        yield break;
    }



}
