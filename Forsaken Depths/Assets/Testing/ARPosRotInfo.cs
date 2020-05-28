using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARPosRotInfo : MonoBehaviour
{
    Vector3 currentPosition;
    Vector3 currentRotation;

    // Update is called once per frame
    void Update()
    {
        currentPosition = gameObject.transform.position;
    }

    void OnGUI() 
    {
        int w = Screen.width, h = Screen.height;

        GUIStyle style = new GUIStyle();

        Rect rect = new Rect(0, 0, w, h * 2 / 100);
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = h * 2 / 100;
        style.normal.textColor = new Color(0.0f, 0.0f, 0.5f, 1.0f);
        string text = currentPosition.ToString();
        GUI.Label(rect, text, style);
    }
}
