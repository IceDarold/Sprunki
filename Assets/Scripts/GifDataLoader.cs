using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Assets.Scripts
{
    public class GifDataLoader : MonoBehaviour
    {
        public static float Loaded => (float)loading / count;
        private static GifDataLoader instance;
        private static int loading = 0;
        private static int count = 0;

        private void Awake()
        {
            instance = this;
        }

        public static void LoadGifData(string path, Action<List<UniGif.GifTexture>> callback)
        {
            string gifPath = Application.dataPath + path;
            Debug.Log(gifPath);
            byte[] data = File.ReadAllBytes(gifPath);
            Debug.Log("Starting Data Loading");
            loading += 1;
            count += 1;
            instance.StartCoroutine(UniGif.GetTextureListCoroutine(data, (list, loopCount, width, height) =>
            {
                callback?.Invoke(list);
                Debug.Log("Loading Data Ended");
                loading -= 1;

            }));

            
        }

    }
}