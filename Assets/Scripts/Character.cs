using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer),typeof(AudioSource),typeof(Animator))]
public class Character : MonoBehaviour
{
    
    [SerializeField] private RuntimeAnimatorController Template;

    private AudioSource audioSource;

    private bool isSkinned = false;
    private bool isMute = false;
    private SkinImageObject obj;

    private CharacterAppearance characterAppearance;


    private const string CLIP_NAME = "TestAnim";
    private void Awake()
    {
        characterAppearance = GetComponent<CharacterAppearance>();
        audioSource = GetComponent<AudioSource>();
        
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
            
            SetupAudioSource();
            characterAppearance.AddSkin(obj.SkinSO);
            
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

        if (!isMute)
        {
            audioSource.Stop();
            isMute = true;
            
        }
        else
        {
            
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
        audioSource.clip = obj.SkinSO.AudioClip;
        MusicSyncController.Add(obj.SkinSO.AudioClip.length);
        audioSource.time = MusicSyncController.Time;
        audioSource.Play();
    }




}
