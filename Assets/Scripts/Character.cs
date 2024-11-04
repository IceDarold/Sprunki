using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer),typeof(AudioSource),typeof(Animator))]
public class Character : MonoBehaviour
{
    [SerializeField] private Sprite DefaultSprite;
    [SerializeField] private Sprite GlowSprite;
    [SerializeField] private RuntimeAnimatorController Template;

    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;
    private Animator animator;
    private AnimatorOverrideController overrideController;

    private bool isSkinned = false;
    private bool isMute = false;
    private SkinImageObject obj;


    private const string CLIP_NAME = "TestAnim";
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();

        overrideController = new AnimatorOverrideController(Template);
        animator.runtimeAnimatorController = overrideController;
        
    }


    private void OnMouseEnter()
    {
        if (!isSkinned && DragAndDropController.GetData() != null)
        {
            spriteRenderer.sprite = GlowSprite;
        }
        
    }

    private void OnMouseExit()
    {
        if(!isSkinned && DragAndDropController.GetData() != null)
        {
            spriteRenderer.sprite = DefaultSprite;
        }
        
    }

    private void OnMouseOver()
    {
        var data = DragAndDropController.GetData();
        if (Input.GetMouseButtonUp(0) && data != null && !isSkinned)
        {

              isSkinned = true;
              obj = data;
              obj.gameObject.SetActive(false);
            
            SetupAudioSource();
            SetupAnimation();
            
        }
    }

    private void OnMouseDown()
    {
        if (isSkinned)
        {
            isSkinned =false;
            obj.gameObject.SetActive(true);

            StopAnimAndAudio();

            spriteRenderer.sprite = DefaultSprite;
        }
    }

    public void ChangeMuteState()
    {
        if(!isSkinned) return;

        if (!isMute)
        {
            audioSource.Stop();
            animator.SetTrigger("Mute");
            spriteRenderer.sprite = obj.SkinSO.OffSkin;
            isMute = true;
        }
        else
        {
            animator.SetTrigger("AddSkin");
            audioSource.Play();
            isMute = false;
        }
        
    }

    private void StopAnimAndAudio()
    {
        animator.SetTrigger("RemoveSkin");
        audioSource.Stop();
    }

    private void SetupAudioSource()
    {
        audioSource.clip = obj.SkinSO.AudioClip;
        audioSource.loop = true;
        audioSource.Play();
    }

    private void SetupAnimation()
    {
        overrideController[CLIP_NAME] = obj.SkinSO.AnimationClip;
        animator.SetTrigger("AddSkin");
    }



}
