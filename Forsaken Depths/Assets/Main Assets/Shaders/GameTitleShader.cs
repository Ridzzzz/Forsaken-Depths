using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameTitleShader : MonoBehaviour
{
    float minSoftness = 0f;
    float maxSoftness = 0.5f;

    float minDilate = -0.25f;
    float maxDilate = 0.25f;

    float minThickness = 0.25f;
    float maxThickness = 0.25f;

    float currentSoftness;
    float currentDilate;
    float currentThickness;

    float effectTimer = 2f;

    void Start()
    {
        currentSoftness = maxSoftness;
        currentDilate = maxDilate;
        currentThickness = maxThickness;

        transform.GetChild(0).GetComponent<TMP_Text>().fontSharedMaterial.SetFloat(ShaderUtilities.ID_OutlineSoftness, currentSoftness);
        transform.GetChild(0).GetComponent<TMP_Text>().fontSharedMaterial.SetFloat(ShaderUtilities.ID_FaceDilate, currentDilate);
        transform.GetChild(0).GetComponent<TMP_Text>().fontSharedMaterial.SetFloat(ShaderUtilities.ID_OutlineWidth, currentThickness);
    }

    void Update()
    {
        if (currentSoftness == maxSoftness && currentDilate == maxDilate)
        {
            StartCoroutine(changeSoftnessValueOverTime(maxSoftness, minSoftness, effectTimer));
            StartCoroutine(changeDilateValueOverTime(maxDilate, minDilate, effectTimer));
        }

        if (currentSoftness == minSoftness && currentDilate == minDilate)
        {
            StartCoroutine(changeSoftnessValueOverTime(minSoftness, maxSoftness, effectTimer));
            StartCoroutine(changeDilateValueOverTime(minDilate, maxDilate, effectTimer));
        }

        transform.GetChild(0).GetComponent<TMP_Text>().fontSharedMaterial.SetFloat(ShaderUtilities.ID_OutlineSoftness, currentSoftness);
        transform.GetChild(0).GetComponent<TMP_Text>().fontSharedMaterial.SetFloat(ShaderUtilities.ID_FaceDilate, currentDilate);
    }

    IEnumerator changeSoftnessValueOverTime(float fromVal, float toVal, float duration)
    {
        float counter = 0f;

        while (counter < duration)
        {
            if (Time.timeScale == 0)
                counter += Time.unscaledDeltaTime;
            else
                counter += Time.deltaTime;

            float val = Mathf.Lerp(fromVal, toVal, counter / duration);
            currentSoftness = val;
            //Debug.Log("Val: " + val);
            yield return null;
        }
    }

    IEnumerator changeDilateValueOverTime(float fromVal, float toVal, float duration)
    {
        float counter = 0f;

        while (counter < duration)
        {
            if (Time.timeScale == 0)
                counter += Time.unscaledDeltaTime;
            else
                counter += Time.deltaTime;

            float val = Mathf.Lerp(fromVal, toVal, counter / duration);
            currentDilate = val;
            //Debug.Log("Val: " + val);
            yield return null;
        }
    }

    IEnumerator changeThicknessValueOverTime(float fromVal, float toVal, float duration)
    {
        float counter = 0f;

        while (counter < duration)
        {
            if (Time.timeScale == 0)
                counter += Time.unscaledDeltaTime;
            else
                counter += Time.deltaTime;

            float val = Mathf.Lerp(fromVal, toVal, counter / duration);
            currentThickness = val;
            //Debug.Log("Val: " + val);
            yield return null;
        }
    }
}
