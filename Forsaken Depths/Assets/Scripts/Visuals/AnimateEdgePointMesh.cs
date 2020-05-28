using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateEdgePointMesh : MonoBehaviour
{
    public float speedX = 0.1f;
    public float speedY = 0.1f;
    float curX;
    float curY;
    int TileNum;

    void Start()
    {
        TileNum = transform.childCount;

        curX = transform.GetChild(0).GetComponent<Renderer>().material.mainTextureOffset.x;
        curY = transform.GetChild(0).GetComponent<Renderer>().material.mainTextureOffset.y;
    }

    // Update is called once per frame
    void Update()
    {
        curX += Time.deltaTime * speedX;
        curY += Time.deltaTime * speedY;

        transform.GetChild(0).GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(curX,curY));
    }
}
