using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAppearance : MonoBehaviour
{
    [SerializeField] private Sprite DefaultSprite;
    [SerializeField] private Sprite GlowSprite;
    [SerializeField] private float AnimationFrameTime;
    [SerializeField] private float SkinRemoveAnimationSpeed;

    private SpriteRenderer spriteRenderer;
    private SkinSO skinData;

    private int currentFrame = -1;
    private float currentTime;


    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if(currentFrame != -1)
        {
            currentTime += Time.deltaTime;
            if(currentTime >= AnimationFrameTime)
            {
                currentTime = 0f;
                currentFrame = (currentFrame + 1) % skinData.Animation.Count;
                spriteRenderer.sprite = skinData.Animation[currentFrame];
            }
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
        if(skin == null || skin.Animation.Count == 0)
        {
            Debug.LogError("Add at least one sprite for animation");
            return;
        }

        skinData = skin;
        currentFrame = 0;
        currentTime = 0f;

        spriteRenderer.sprite = skinData.Animation[currentFrame];

    }

    public void RemoveSkin()
    {
        
        currentFrame = -1;
        currentTime = 0f;

        StartCoroutine(RemoveSkinAnimation());
    }

    public void Mute(bool isMute)
    {
        if(isMute)
        {
            spriteRenderer.sprite = skinData.OffSkin;
            currentFrame = -1;
            currentTime = 0f;
        }
        else
        {
            currentFrame = 0;
            spriteRenderer.sprite = skinData.Animation[currentFrame];
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
