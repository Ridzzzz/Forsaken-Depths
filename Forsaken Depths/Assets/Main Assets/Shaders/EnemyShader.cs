using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShader : MonoBehaviour
{
    public GameObject enemyEyeL, enemyEyeR;

    Color currentColorGlow;
    float currentRimGlow;
    float currentDissolveValue;

    Color maxColorGlow = new Color(100f,100f,255f,255f);
    Color minColorGlow = new Color(0f,0f,255f,255f);

    float maxRimGlow = 2f;
    float minRimGlow = 5f;

    float minDissolveValue = 1.25f;
    float maxDissolveValue = -1f;

    float effectTimer = 2f;

    bool enemyIsDead;

    void OnEnable()
    {
        currentColorGlow = minColorGlow;
        currentRimGlow = minRimGlow;

        currentDissolveValue = minDissolveValue;

        transform.GetComponent<Renderer>().material.SetColor("_Color", new Color32((byte)currentColorGlow.r,(byte)currentColorGlow.g,255,255));
        transform.GetComponent<Renderer>().material.SetFloat("_RimPower", currentRimGlow);

        transform.GetComponent<Renderer>().material.SetFloat("_DisAmount", currentDissolveValue);
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

        transform.GetComponent<Renderer>().material.SetColor("_Color", new Color32((byte)currentColorGlow.r,(byte)currentColorGlow.g,255,255));
        transform.GetComponent<Renderer>().material.SetFloat("_RimPower", currentRimGlow);
        
        if (enemyIsDead)
        {
            transform.GetComponent<Renderer>().material.SetFloat("_DisAmount", currentDissolveValue);
        }
    }

    public void ExecuteDeathSequence()
    {
        enemyIsDead = true;
        enemyEyeL.SetActive(false);
        enemyEyeR.SetActive(false);
        StartCoroutine(changeDissolveValueOverTime(minDissolveValue, maxDissolveValue, effectTimer));
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
            currentColorGlow.g = val;
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

    IEnumerator changeDissolveValueOverTime(float fromVal, float toVal, float duration)
    {
        yield return new WaitForSeconds(1.5f);
        float counter = 0f;

        while (counter < duration)
        {
            if (Time.timeScale == 0)
                counter += Time.unscaledDeltaTime;
            else
                counter += Time.deltaTime;

            float val = Mathf.Lerp(fromVal, toVal, counter / duration);
            currentDissolveValue = val;
            //Debug.Log("Val: " + val);
            yield return null;
        }
    }
}
