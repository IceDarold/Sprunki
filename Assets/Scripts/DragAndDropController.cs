using Assets.Scripts;
using System.Collections;
using UnityEngine;


public  class DragAndDropController : MonoBehaviour
{
    private static DragAndDropController instance;
    private SkinImageObject obj;
    private SkinSO skinSO;

    private Vector3 mousePosition;
    


    private void Awake()
    {
        instance = this;
        
    }

    private void Update()
    {
        if(obj != null && Input.GetMouseButtonUp(0))
        {
            obj.Return();
            obj = null;
            skinSO = null;
        }

        if(Input.mousePosition != mousePosition && obj != null)
        {
            Vector3 delta = Input.mousePosition - mousePosition;
            obj.transform.position += delta;
            mousePosition = Input.mousePosition;
        }
    }
    public static void AddObject(SkinImageObject o)
    {
        instance.obj = o;
        instance.skinSO = instance.obj.SkinSO;
        instance.mousePosition = Input.mousePosition;
    }

    public static SkinImageObject GetData()
    {
        return instance.obj;
    }
}