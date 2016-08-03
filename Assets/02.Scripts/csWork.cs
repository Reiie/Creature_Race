using UnityEngine;
using System;
using System.Collections;
//using System.Collections.Generic;

public class csWork : MonoBehaviour {

    public enum Work
    {
        Nowork,
        Kangnam,
        Kangbuk,
        Alba,
        Repair,
        Labor,
        Plant,
        CheonHo,
        Rest,
        Race
    }


    private GameObject clone;

    public UISprite icon;
    public UILabel r_Time_Label;
    public int r_Time;
    public Work WorkName;
    

    private UIRoot mRoot;
    private static bool isDragOk = true;

    // Use this for initialization
    void Start () {
        
        mRoot = NGUITools.FindInParents<UIRoot>(transform.parent);
        r_Time = System.Convert.ToInt32(r_Time_Label.text);
    }
	
    public void OnDragStart()
    {
       
       if (!isDragOk) return;
       // isDragOk = true;
        
      
     //   GameObject child = gameObject.transform.FindChild("Color").gameObject;
        clone = NGUITools.AddChild(transform.parent.gameObject, gameObject);
        clone.transform.localPosition = transform.localPosition;
        clone.transform.localRotation = transform.localRotation;
        clone.transform.localScale = transform.localScale;
        clone.GetComponent<BoxCollider>().enabled = false;
        clone.transform.SetParent(UIDragDropRoot.root);

    }

    public void OnDrag(Vector2 delta)
    {
        if (!isDragOk) return;

        clone.transform.localPosition += (Vector3)(delta * mRoot.pixelSizeAdjustment);
    }

    void OnDragEnd()
    {
        if (!isDragOk) return;

        NGUITools.Destroy(clone.gameObject);
    }
}
