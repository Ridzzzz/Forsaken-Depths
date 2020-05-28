using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponShader : MonoBehaviour
{
    public GameObject PS_weaponTrail;
    Color currentColorGlow;
    float currentRimGlow;

    float pCurrentDissolveValue;
    float sCurrentDissolveValue;

    Color maxColorGlow = new Color(255f,100f,255f,255f);
    Color minColorGlow = new Color(255f,0f,255f,255f);

    float maxRimGlow = 2f;
    float minRimGlow = 5f;

    float pMinDissolveValue = 1.25f;
    float pMaxDissolveValue = -1f;

    float sMinDissolveValue = 0.75f;
    float sMaxDissolveValue = -0.50f;

    float effectTimer = 2f;

    bool enemyIsDead;

    void OnEnable()
    {
        currentColorGlow = minColorGlow;
        currentRimGlow = minRimGlow;

        pCurrentDissolveValue = pMinDissolveValue;
        sCurrentDissolveValue = sMinDissolveValue;

        transform.GetComponent<Renderer>().material.SetColor("_ColorTint", new Color32(255,(byte)currentColorGlow.g,255,255));
        transform.GetComponent<Renderer>().material.SetFloat("_RimPower", currentRimGlow);

        transform.GetComponent<Renderer>().material.SetFloat("_DisAmount", pCurrentDissolveValue);

        transform.GetComponent<Renderer>().materials[1].SetFloat("_DisAmount", sCurrentDissolveValue);
        transform.GetComponent<Renderer>().materials[2].SetFloat("_DisAmount", sCurrentDissolveValue);
        transform.GetComponent<Renderer>().materials[3].SetFloat("_DisAmount", sCurrentDissolveValue);
        transform.GetComponent<Renderer>().materials[4].SetFloat("_DisAmount", sCurrentDissolveValue);
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

        transform.GetComponent<Renderer>().material.SetColor("_ColorTint", new Color32(255,(byte)currentColorGlow.g,255,255));
        transform.GetComponent<Renderer>().material.SetFloat("_RimPower", currentRimGlow);

        if (enemyIsDead)
        {
            transform.GetComponent<Renderer>().materials[0].SetFloat("_DisAmount", pCurrentDissolveValue);
            transform.GetComponent<Renderer>().materials[1].SetFloat("_DisAmount", sCurrentDissolveValue);
            transform.GetComponent<Renderer>().materials[2].SetFloat("_DisAmount", sCurrentDissolveValue);
            transform.GetComponent<Renderer>().materials[3].SetFloat("_DisAmount", sCurrentDissolveValue);
            transform.GetComponent<Renderer>().materials[4].SetFloat("_DisAmount", sCurrentDissolveValue);
        }        
    }

    public void ExecuteDeathSequence()
    {
        enemyIsDead = true;
        StartCoroutine(changePDissolveValueOverTime(pMinDissolveValue, pMaxDissolveValue, effectTimer));
        StartCoroutine(changeSDissolveValueOverTime(sMinDissolveValue, sMaxDissolveValue, effectTimer));
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

    IEnumerator changePDissolveValueOverTime(float fromVal, float toVal, float duration)
    {
        yield return new WaitForSeconds(1.5f);
        PS_weaponTrail.SetActive(false);

        float counter = 0f;

        while (counter < duration)
        {
            if (Time.timeScale == 0)
                counter += Time.unscaledDeltaTime;
            else
                counter += Time.deltaTime;

            float val = Mathf.Lerp(fromVal, toVal, counter / duration);
            pCurrentDissolveValue = val;
            //Debug.Log("Val: " + val);
            yield return null;
        }
    }

    IEnumerator changeSDissolveValueOverTime(float fromVal, float toVal, float duration)
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
            sCurrentDissolveValue = val;
            //Debug.Log("Val: " + val);
            yield return null;
        }
    }
}
