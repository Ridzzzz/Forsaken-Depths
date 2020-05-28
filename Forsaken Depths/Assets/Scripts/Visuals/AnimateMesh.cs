using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateMesh : MonoBehaviour
{
    public float speedX = 0.1f;
    public float speedY = 0.1f;
    float curX;
    float curY;
    int TileNum;

    void Start()
    {
        TileNum = transform.childCount;

        for (int i = 0; i < TileNum; i++)
        {
            curX = transform.GetChild(i).GetComponent<Renderer>().material.mainTextureOffset.x;
            curY = transform.GetChild(i).GetComponent<Renderer>().material.mainTextureOffset.y;
        }
    }

    // Update is called once per frame
    void Update()
    {
        curX += Time.deltaTime * speedX;
        curY += Time.deltaTime * speedY;

        for (int i = 0; i < TileNum; i++)
        {
            transform.GetChild(i).GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(curX,curY));
        }
    }
}
