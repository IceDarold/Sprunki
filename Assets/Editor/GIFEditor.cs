using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Unity.EditorCoroutines;
using Unity.EditorCoroutines.Editor;
using UnityEngine.UIElements;
using System;
using System.IO;

[CustomEditor(typeof(GIFAnimationClip))]
public class GIFEditor : Editor
{
    private static GIFEditor instance;
    private PreviewRenderUtility previewRenderUtility;
    private GameObject previewObject;
    private SpriteRenderer previewRenderer;
    private Sprite previewSprite;

    private GIFAnimationClip clip;

    private bool isPlaying;
    private float currentTime = 0f;

    private float cachedTime = 0f;

    private float length = 0f;

    private void OnEnable()
    {
        previewRenderUtility = new PreviewRenderUtility();
        previewRenderUtility.camera.orthographic = true; 
        previewRenderUtility.camera.orthographicSize = 5f;
        

        previewObject = new GameObject("Preview Object");
        previewRenderer =  previewObject.AddComponent<SpriteRenderer>();
        previewRenderer.color = Color.white;

        previewRenderUtility.AddSingleGO(previewObject);

        currentTime = (float)EditorApplication.timeSinceStartup;

        
        
    }

    private void OnDisable()
    {
        
        if (previewObject != null)
            DestroyImmediate(previewObject);
        previewRenderUtility.Cleanup();

        
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

         clip = target as GIFAnimationClip;

        var property = serializedObject.FindProperty("Animation");

        

        EditorGUILayout.PropertyField(property,true);
        serializedObject.ApplyModifiedProperties();

        if(GUILayout.Button("Export GIF"))
        {
            ExportGif();
        }

        EditorGUILayout.Space(80);


        
        length = clip.Length;


        if(length == 0)
        {
            return;
        }

        EditorGUILayout.BeginHorizontal();
        string buttonContent = isPlaying ? "Stop" : "Play";
        if (GUILayout.Button(buttonContent))
        {
            isPlaying = !isPlaying;
        }

        
        
        currentTime = currentTime % length;
        currentTime = EditorGUILayout.Slider(currentTime, 0f, length);

        EditorGUILayout.EndHorizontal();


        
        previewRenderer.sprite = clip.GetFrame(currentTime).Sprite;



        Rect previewRect = GUILayoutUtility.GetRect(300, 300);

        previewRenderUtility.BeginPreview(previewRect, GUIStyle.none);
        previewRenderUtility.camera.transform.position = new Vector3(0, 0, -10);
        previewRenderUtility.camera.backgroundColor = Color.gray;
        previewRenderUtility.camera.clearFlags = CameraClearFlags.SolidColor;
        previewRenderUtility.camera.transform.LookAt(Vector3.zero);
        previewRenderUtility.camera.Render();

        
        Texture result = previewRenderUtility.EndPreview();
        GUI.DrawTexture(previewRect, result);

        float delta = (float)EditorApplication.timeSinceStartup - cachedTime;
        if (isPlaying)
        {
            currentTime += delta;

        }
        cachedTime = (float)EditorApplication.timeSinceStartup;

        Repaint();

    }

    private void ExportGif()
    {
        string path = EditorUtility.OpenFilePanel("GIF Exporting", Application.dataPath, "gif");

        if (path == "") return;

        if (Path.GetExtension(path) != ".gif") return;
        string fileName = Path.GetFileNameWithoutExtension(path); 
        

        byte[] bytes = File.ReadAllBytes(path);
        EditorCoroutineUtility.StartCoroutineOwnerless(UniGif.GetTextureListCoroutine(bytes,(list,height,width,a)=>
        {
            clip.Clear();
            int count = list.Count;

            float[] durations = new float[count];
            Sprite[] sprites = new Sprite[count];

            string relativePath = "/Sprites/Generated/" + fileName + "/";
            string spriteFolder = Application.dataPath + relativePath;

            if(!Directory.Exists(spriteFolder))
            {
                Directory.CreateDirectory(spriteFolder);
            }

            int i = 0;
            foreach(var item in list)
            {
                durations[i] = item.m_delaySec;
                byte[] pngData = item.m_texture2d.EncodeToPNG();
                File.WriteAllBytes(spriteFolder + i.ToString() + ".png", pngData);
                i++;
                
            }

            AssetDatabase.Refresh();

            for (int j = 0; j < count; j++)
            {
                /*TextureImporter textureImporter = AssetImporter.GetAtPath("Assets" + relativePath + j.ToString() + ".png") as TextureImporter;
                if(textureImporter != null)
                {
                    textureImporter.textureType = TextureImporterType.Sprite;
                    textureImporter.SaveAndReimport();
                }*/
                Debug.Log("Assets" + relativePath + j.ToString() + ".png");
                Sprite newSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets" + relativePath + j.ToString() + ".png");
                sprites[j] = newSprite;
                Debug.Log(newSprite);
            }
            
            clip.AddAnimation(sprites,durations);

        }));
    }
}
