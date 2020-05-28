using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnScaledFloatEffects : MonoBehaviour
{
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

    private float startAnimOffset = 0;

    Transform cam;

    void Start()
    {
        origPos = transform.localPosition;
        origRot = transform.localEulerAngles;
        origScale = transform.localScale;            
        startAnimOffset = 0;

        cam = Camera.main.transform;
    }

    
    void Update()
    {
        /* position */
        if(animPos) 
        {
            Vector3 pos;
            pos.x = origPos.x + posAmplitude.x*Mathf.Sin(posSpeed.x*Time.unscaledTime + startAnimOffset);
            pos.y = origPos.y + posAmplitude.y*Mathf.Sin(posSpeed.y*Time.unscaledTime + startAnimOffset);
            pos.z = origPos.z + posAmplitude.z*Mathf.Sin(posSpeed.z*Time.unscaledTime + startAnimOffset);
            transform.localPosition = pos;
        }

        /* rotation */
        if(animRot) 
        {
            Vector3 rot;
            rot.x = origRot.x + rotAmplitude.x*Mathf.Sin(rotSpeed.x*Time.unscaledTime + startAnimOffset);
            rot.y = origRot.y + rotAmplitude.y*Mathf.Sin(rotSpeed.y*Time.unscaledTime + startAnimOffset);
            rot.z = origRot.z + rotAmplitude.z*Mathf.Sin(rotSpeed.z*Time.unscaledTime + startAnimOffset);
            transform.localEulerAngles = rot;
        }

        /* scale */
        if(animScale) 
        {
            Vector3 scale;
            scale.x = origScale.x * (1+scaleAmplitude.x*Mathf.Sin(scaleSpeed.x*Time.unscaledTime + startAnimOffset));
            scale.y = origScale.y * (1+scaleAmplitude.y*Mathf.Sin(scaleSpeed.y*Time.unscaledTime + startAnimOffset));
            scale.z = origScale.z * (1+scaleAmplitude.z*Mathf.Sin(scaleSpeed.z*Time.unscaledTime + startAnimOffset));
            transform.localScale = scale;
        }
    }

    void LateUpdate()
    {
        transform.forward = cam.forward;
    }
}
