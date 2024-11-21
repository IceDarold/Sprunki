using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class SkinImageObject : MonoBehaviour,IPointerDownHandler
    {
        [SerializeField] private SkinSO SkinSettings;

        public SkinSO SkinSO => SkinSettings;

        private Vector3 startPos;
        private Image image;

        private void Awake()
        {
            image = GetComponent<Image>();

            if (SkinSettings == null) return;
            SkinSettings.LoadSkinAnims();


        }

        private void Update()
        {
            image.fillAmount = 1 - GifDataLoader.Loaded;
           
            image.color = GifDataLoader.Loaded > 0 ? Color.gray : Color.white;
        }

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