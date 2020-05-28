using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowShader : MonoBehaviour
{
    Color currentColorGlow;
    float currentRimGlow;

    Color maxColorGlow = new Color(255f,0f,255f,255f);
    Color minColorGlow = new Color(150f,0f,255f,255f);

    float maxRimGlow = 1f;
    float minRimGlow = 3f;

    float effectTimer = 2f;

    void Start()
    {
        currentColorGlow = minColorGlow;
        currentRimGlow = minRimGlow;

        transform.GetComponent<Renderer>().sharedMaterial.SetColor("_ColorTint", new Color32((byte)currentColorGlow.r,0,255,255));
        transform.GetComponent<Renderer>().sharedMaterial.SetFloat("_RimPower", currentRimGlow);
    }

    void Update()
    {
        if (currentColorGlow.r == minColorGlow.r && currentRimGlow == minRimGlow)
        {
            StartCoroutine(changeColValueOverTime(minColorGlow.r, maxColorGlow.r, effectTimer));
            StartCoroutine(changeRimValueOverTime(minRimGlow, maxRimGlow, effectTimer));
        }

        if (currentColorGlow.r == maxColorGlow.r && currentRimGlow == maxRimGlow)
        {
            StartCoroutine(changeColValueOverTime(maxColorGlow.r, minColorGlow.r, effectTimer));
            StartCoroutine(changeRimValueOverTime(maxRimGlow, minRimGlow, effectTimer));
        }

        transform.GetComponent<Renderer>().sharedMaterial.SetColor("_ColorTint", new Color32((byte)currentColorGlow.r,0,255,255));
        transform.GetComponent<Renderer>().sharedMaterial.SetFloat("_RimPower", currentRimGlow);
    }

    IEnumerator changeColValueOverTime(float fromVal, float toVal, float duration)
    {
        float counter = 0f;

        while (counter < duration)
        {
            if (Time.timeScale == 0)
                counter += Time.unscaledDeltaTime;
            else
                counter += Time.deltaTime;

            float val = Mathf.Lerp(fromVal, toVal, counter / duration);
            currentColorGlow.r = val;
            //Debug.Log("Val: " + val);
            yield return null;
        }
    }

    IEnumerator changeRimValueOverTime(float fromVal, float toVal, float duration)
    {
        float counter = 0f;

        while (counter < duration)
        {
            if (Time.timeScale == 0)
                counter += Time.unscaledDeltaTime;
            else
                counter += Time.deltaTime;

            float val = Mathf.Lerp(fromVal, toVal, counter / duration);
            currentRimGlow = val;
            //Debug.Log("Val: " + val);
            yield return null;
        }
    }
}
