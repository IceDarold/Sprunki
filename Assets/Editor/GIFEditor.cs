using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Unity.EditorCoroutines;
using Unity.EditorCoroutines.Editor;
using UnityEngine.UIElements;

[CustomEditor(typeof(GIFAnimationClip))]
public class GIFEditor : Editor
{
    private static GIFEditor instance;
    private PreviewRenderUtility previewRenderUtility;
    private GameObject previewObject;
    private SpriteRenderer previewRenderer;
    private Sprite previewSprite;


    private bool isPlaying;
    private float currentTime = 0f;

    private float cachedTime = 0f;
  
    private EditorCoroutine coroutine;
    private float length = 0f;

    private void OnEnable()
    {
        previewRenderUtility = new PreviewRenderUtility();
        previewRenderUtility.camera.orthographic = true; 
        previewRenderUtility.camera.orthographicSize = 5f;
        

        previewObject = new GameObject("Preview Object");
        previewRenderer =  previewObject.AddComponent<SpriteRenderer>();
        previewRenderer.color = Color.red;

        previewRenderUtility.AddSingleGO(previewObject);

        currentTime = (float)EditorApplication.timeSinceStartup;

        //coroutine = EditorCoroutineUtility.StartCoroutineOwnerless(UpdateCoroutine());
        
    }

    private void OnDisable()
    {
        
        if (previewObject != null)
            DestroyImmediate(previewObject);
        previewRenderUtility.Cleanup();

        //EditorCoroutineUtility.StopCoroutine(coroutine);
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        GIFAnimationClip clip = target as GIFAnimationClip;

        var property = serializedObject.FindProperty("Animation");

        

        EditorGUILayout.PropertyField(property,true);
        serializedObject.ApplyModifiedProperties();

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


    private IEnumerator UpdateCoroutine()
    {
        while (true)
        {
            
            yield return null;
        }
        
        
    }



}
