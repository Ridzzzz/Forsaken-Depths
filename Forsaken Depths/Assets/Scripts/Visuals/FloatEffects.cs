using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatEffects : MonoBehaviour
{
    public bool Singular = true;
    public bool animPos = true;
    public Vector3 posAmplitude = Vector3.one;
    public Vector3 posSpeed = Vector3.one;

    public bool animRot = false;
    public Vector3 rotAmplitude = Vector3.one*20;
    public Vector3 rotSpeed = Vector3.one;

    public bool animScale = false;
    public Vector3 scaleAmplitude = Vector3.one*0.1f;
    public Vector3 scaleSpeed = Vector3.one;

    private Vector3 origPos;
    private Vector3 origRot;
    private Vector3 origScale;

    private Vector3 [] _origPos;
    private Vector3 [] _origRot;
    private Vector3 [] _origScale;

    private float startAnimOffset = 0;

    int AlterNum;

    void Start() 
    {
        if (Singular)
        {
            origPos = transform.localPosition;
            origRot = transform.localEulerAngles;
            origScale = transform.localScale;            
            startAnimOffset = 0;
            //startAnimOffset = Random.Range(0f, 540f);        // so that the xyz anims are already offset from each other since the start
        }         

        else
        {
            AlterNum = transform.childCount;
            _origPos = new Vector3[AlterNum];
            _origRot = new Vector3[AlterNum];
            _origScale = new Vector3[AlterNum];

            for (int i = 0; i < AlterNum; i++)
            {
                _origPos[i] = transform.GetChild(i).localPosition;
                _origRot[i] = transform.GetChild(i).localEulerAngles;
                _origScale[i] = transform.GetChild(i).localScale;
            }
        }
    }
     
     /**
      * Update
      */
    void Update() 
    {
        if (Singular)
        {
            /* position */
            if(animPos) 
            {
                Vector3 pos;
                pos.x = origPos.x + posAmplitude.x*Mathf.Sin(posSpeed.x*Time.time + startAnimOffset);
                pos.y = origPos.y + posAmplitude.y*Mathf.Sin(posSpeed.y*Time.time + startAnimOffset);
                pos.z = origPos.z + posAmplitude.z*Mathf.Sin(posSpeed.z*Time.time + startAnimOffset);
                transform.localPosition = pos;
            }
    
            /* rotation */
            if(animRot) 
            {
                Vector3 rot;
                rot.x = origRot.x + rotAmplitude.x*Mathf.Sin(rotSpeed.x*Time.time + startAnimOffset);
                rot.y = origRot.y + rotAmplitude.y*Mathf.Sin(rotSpeed.y*Time.time + startAnimOffset);
                rot.z = origRot.z + rotAmplitude.z*Mathf.Sin(rotSpeed.z*Time.time + startAnimOffset);
                transform.localEulerAngles = rot;
            }
    
            /* scale */
            if(animScale) 
            {
                Vector3 scale;
                scale.x = origScale.x * (1+scaleAmplitude.x*Mathf.Sin(scaleSpeed.x*Time.time + startAnimOffset));
                scale.y = origScale.y * (1+scaleAmplitude.y*Mathf.Sin(scaleSpeed.y*Time.time + startAnimOffset));
                scale.z = origScale.z * (1+scaleAmplitude.z*Mathf.Sin(scaleSpeed.z*Time.time + startAnimOffset));
                transform.localScale = scale;
            }
        }

        else
        {
            for (int i = 0; i < AlterNum; i++)
            {
                /* position */
                if(animPos) 
                {
                    Vector3 pos;
                    pos.x = _origPos[i].x + posAmplitude.x*Mathf.Sin(posSpeed.x*Time.time + startAnimOffset);
                    pos.y = _origPos[i].y + posAmplitude.y*Mathf.Sin(posSpeed.y*Time.time + startAnimOffset);
                    pos.z = _origPos[i].z + posAmplitude.z*Mathf.Sin(posSpeed.z*Time.time + startAnimOffset);
                    transform.GetChild(i).localPosition = pos;
                }
        
                /* rotation */
                if(animRot) 
                {
                    Vector3 rot;
                    rot.x = _origRot[i].x + rotAmplitude.x*Mathf.Sin(rotSpeed.x*Time.time + startAnimOffset);
                    rot.y = _origRot[i].y + rotAmplitude.y*Mathf.Sin(rotSpeed.y*Time.time + startAnimOffset);
                    rot.z = _origRot[i].z + rotAmplitude.z*Mathf.Sin(rotSpeed.z*Time.time + startAnimOffset);
                    transform.GetChild(i).localEulerAngles = rot;
                }
        
                /* scale */
                if(animScale) 
                {
                    Vector3 scale;
                    scale.x = _origScale[i].x * (1+scaleAmplitude.x*Mathf.Sin(scaleSpeed.x*Time.time + startAnimOffset));
                    scale.y = _origScale[i].y * (1+scaleAmplitude.y*Mathf.Sin(scaleSpeed.y*Time.time + startAnimOffset));
                    scale.z = _origScale[i].z * (1+scaleAmplitude.z*Mathf.Sin(scaleSpeed.z*Time.time + startAnimOffset));
                    transform.GetChild(i).localScale = scale;
                }
            }
        }
    }
}
