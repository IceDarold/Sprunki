using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer),typeof(AudioSource))]
public class Character : MonoBehaviour
{
    [SerializeField] private Sprite DefaultSprite;
    [SerializeField] private Sprite GlowSprite;

    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;
    private bool isSkinned = false;
    private SkinImageObject obj;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
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
              spriteRenderer.color = Color.red;
              obj = data;
            obj.gameObject.SetActive(false);
              Debug.Log("Set Skin");
        }
    }

    private void OnMouseDown()
    {
        if (isSkinned)
        {
            isSkinned =false;
            spriteRenderer.color = Color.white;
            obj.gameObject.SetActive(true);
        }
    }



}
