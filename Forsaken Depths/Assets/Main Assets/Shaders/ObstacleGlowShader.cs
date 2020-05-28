using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleGlowShader : MonoBehaviour
{
    Color currentColorGlow;
    float currentRimGlow;

    Color maxColorGlow = new Color(0f,255f,255f,255f);
    Color minColorGlow = new Color(0f,150f,150f,255f);

    float maxRimGlow = 1f;
    float minRimGlow = 3f;

    float effectTimer = 2f;

    void Start()
    {
        currentColorGlow = minColorGlow;
        currentRimGlow = minRimGlow;

        transform.GetComponent<Renderer>().sharedMaterial.SetColor("_ColorTint", new Color32(0,(byte)currentColorGlow.g,(byte)currentColorGlow.b,255));
        transform.GetComponent<Renderer>().sharedMaterial.SetFloat("_RimPower", currentRimGlow);
    }

    void Update()
    {
        if (currentColorGlow.g == minColorGlow.g && currentRimGlow == minRimGlow)
        {
            StartCoroutine(changeColValueOverTime(minColorGlow.g, maxColorGlow.g, effectTimer));
            StartCoroutine(changeRimValueOverTime(minRimGlow, maxRimGlow, effectTimer));
        }

        if (currentColorGlow.g == maxColorGlow.g && currentRimGlow == maxRimGlow)
        {
            StartCoroutine(changeColValueOverTime(maxColorGlow.g, minColorGlow.g, effectTimer));
            StartCoroutine(changeRimValueOverTime(maxRimGlow, minRimGlow, effectTimer));
        }

        transform.GetComponent<Renderer>().sharedMaterial.SetColor("_ColorTint", new Color32(0,(byte)currentColorGlow.g,(byte)currentColorGlow.b,255));
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
            currentColorGlow.g = val;
            currentColorGlow.b = val;
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
