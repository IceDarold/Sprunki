using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts
{
    public class SkinImageObject : MonoBehaviour,IPointerDownHandler
    {
        [SerializeField] private SkinSO SkinSettings;

        public SkinSO SkinSO { get; private set; }

        private Vector3 startPos;

        private void Awake()
        {
            startPos = transform.position;
        }

        public void Return()
        {
            transform.position = startPos;
        }



        public void OnPointerDown(PointerEventData eventData)
        {
            DragAndDropController.AddObject(this);
        }
    }
}