﻿using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts
{
    public class SkinImageObject : MonoBehaviour,IPointerDownHandler
    {
        [SerializeField] private SkinSO SkinSettings;

        public SkinSO SkinSO => SkinSettings;

        private Vector3 startPos;



        public void Return()
        {
            transform.position = startPos;
        }



        public void OnPointerDown(PointerEventData eventData)
        {
            startPos = transform.position;
            DragAndDropController.AddObject(this);
        }
    }
}