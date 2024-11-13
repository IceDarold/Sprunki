using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class UpdatedSpriteRenderer : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Vector3 currentSize;
    private Vector3 currentScale;

    
    
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentSize = spriteRenderer.sprite.bounds.size;
        currentScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newSize = spriteRenderer.sprite.bounds.size;
        if ( newSize != currentSize)
        {
            /*Vector3 newScaling = new Vector3(newSize.x /currentSize.x ,newSize.y /currentSize.y,1);
            transform.localScale = new Vector3(currentScale.x / newScaling.x, currentScale.y / newScaling.y, 1);
            Debug.Log(currentScale.ToString() + "         " + transform.localScale.ToString());*/

            float aspectRatio = (currentSize.x / currentSize.y) * (currentScale.x / currentScale.y) / (newSize.x / newSize.y);
            //float aspectRatio = (newSize.x / newSize.y) * (currentScale.x / currentScale.y) / (currentSize.x/currentSize.y);
            float newScaling = currentScale.y * currentSize.y / newSize.y;
            transform.localScale = new Vector3(newScaling * aspectRatio  , newScaling, 1);
            Debug.Log(aspectRatio.ToString() + "    " + (transform.localScale.x / transform.localScale.y).ToString());


            currentScale = transform.localScale;
            currentSize = newSize;
        }
    }
}
