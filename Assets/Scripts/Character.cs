using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;


[RequireComponent(typeof(SpriteRenderer),typeof(AudioSource),typeof(UpdatedSpriteRenderer))]
public class Character : MonoBehaviour
{
    
    [SerializeField] private RuntimeAnimatorController Template;

    private AudioSource audioSource;

    private bool isSkinned = false;
    private bool isMute = false;
    private SkinImageObject obj;

    private CharacterAppearance characterAppearance;

    private float cachedTime = 0f;


    private const string CLIP_NAME = "TestAnim";

    private void Awake()
    {
        characterAppearance = GetComponent<CharacterAppearance>();
        audioSource = GetComponent<AudioSource>();
        
        ModeController.OnModeChanged += OnModeChanged;
    }

    private void OnDestroy()
    {
        ModeController.OnModeChanged -= OnModeChanged;
    }


    private void OnMouseEnter()
    {
        if (!isSkinned && DragAndDropController.GetData() != null)
        {
            characterAppearance.SetSprite(true);
        }
        
    }

    private void OnMouseExit()
    {
        if(!isSkinned && DragAndDropController.GetData() != null)
        {
            characterAppearance.SetSprite(false);
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

            characterAppearance.AddSkin(obj.SkinSO);
            SetupAudioSource();

            if (obj.SkinSO.ChangeToMode != ModeType.None && obj.SkinSO.ChangeToMode != ModeController.Mode)
            {
                ModeController.SetMode(obj.SkinSO.ChangeToMode);
                return;
            }
            

            
            
            
        }
    }

    private void OnMouseDown()
    {
        if (isSkinned)
        {
            isSkinned =false;
  

            obj.gameObject.SetActive(true);

            StopAnimAndAudio();

            characterAppearance.RemoveSkin();
        }
    }

    public void ChangeMuteState()
    {
        if(!isSkinned) return;

        float time = 0f;
        if (!isMute)
        {
            if(MusicSyncController.UseSelfSync)
            {
                cachedTime = audioSource.time;
            }
            
            audioSource.Stop();
            isMute = true;
            
        }
        else
        {
            if (MusicSyncController.UseSelfSync)
            {
                audioSource.time = cachedTime;
            }

            if (MusicSyncController.UseSync)
            {
                audioSource.time = MusicSyncController.Time % obj.SkinSO.GetData().AudioClip.length;
                time = audioSource.time;

                
            }

            audioSource.Play();
            isMute = false;
        }

        
        characterAppearance.Mute(isMute);
        MusicSyncController.MuteState(isMute);

    }

    private void StopAnimAndAudio()
    {
        MusicSyncController.Remove(isMute);
        audioSource.Stop();
        isMute = false;
    }

    private void SetupAudioSource()
    {
        AudioClip clip = obj.SkinSO.GetData().AudioClip;


        if(clip == null)
        {
            return;
        }
        audioSource.clip = clip;

        if (MusicSyncController.UseSync)
        {
            MusicSyncController.Add();
            audioSource.time = MusicSyncController.Time % clip.length;

        }
        
        audioSource.Play();
    }

    private void OnModeChanged()
    {
        if (isSkinned)
        {
            isSkinned = false;


            obj.gameObject.SetActive(true);

            StopAnimAndAudio();

            characterAppearance.RemoveSkin();
        }


    }




}
