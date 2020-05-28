using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterStat))]
public class PlayerUiBar : MonoBehaviour
{
    Transform target;
    Transform ui;
    Transform cam;
    CanvasGroup hpBarBackground, expBarBackground;
    Image healthSlider, expSlider;
    
	void OnEnable () 
    {
        cam = Camera.main.transform;
        ui = transform.GetChild(0).transform;
        target = ui.parent.transform;
        hpBarBackground = ui.GetChild(0).GetComponent<CanvasGroup>();
        expBarBackground = ui.GetChild(1).GetComponent<CanvasGroup>();
        healthSlider = ui.GetChild(0).GetChild(0).GetComponent<Image>();
        expSlider = ui.GetChild(1).GetChild(0).GetComponent<Image>();    

        GetComponent<CharacterStat>().OnHealthChanged += OnHealthChanged;
        GetComponent<PlayerStat>().OnExpChanged += OnExpChanged;
	}

    void OnHealthChanged(Stat health, float currentHealth) 
    {       
        if (ui != null)
        {
            float healthPercent = currentHealth / health.GetValue();
            StartCoroutine(changeHealthValueOverTime(healthSlider.fillAmount, healthPercent, 0.2f));
            
            if (currentHealth <= 0)
            {
                StartCoroutine(changeAlphaValueOverTime(1f, 0f, 0.5f));
            }
        }
    }

    void OnExpChanged(float expMaxCapacity ,float expCounter)
    {
        if (ui != null)
        {           
            float expPercent = expCounter / expMaxCapacity;

            if (expSlider.fillAmount > expPercent)
            {
                expSlider.fillAmount = expPercent;
            }

            else
            {
                StartCoroutine(changeExpValueOverTime(expSlider.fillAmount, expPercent, 0.2f));
            }            
        }
    }

    void LateUpdate () 
    {
        if (ui != null)
        {
            ui.position = target.position + new Vector3(0f, 1f, 0f);
            ui.forward = -cam.forward;
        }
	}

    IEnumerator changeAlphaValueOverTime(float fromVal, float toVal, float duration)
    {
        float counter = 0f;

        while (counter < duration)
        {
            if (Time.timeScale == 0)
                counter += Time.unscaledDeltaTime;
            else
                counter += Time.deltaTime;

            float val = Mathf.Lerp(fromVal, toVal, counter / duration);
            hpBarBackground.alpha = val;
            expBarBackground.alpha = val;
            //Debug.Log("Val: " + val);
            yield return null;
        }
    }

    IEnumerator changeHealthValueOverTime(float fromVal, float toVal, float duration)
    {
        float counter = 0f;

        while (counter < duration)
        {
            if (Time.timeScale == 0)
                counter += Time.unscaledDeltaTime;
            else
                counter += Time.deltaTime;

            float val = Mathf.Lerp(fromVal, toVal, counter / duration);
            healthSlider.fillAmount = val;
            //Debug.Log("Val: " + val);
            yield return null;
        }
    }

    IEnumerator changeExpValueOverTime(float fromVal, float toVal, float duration)
    {
        float counter = 0f;

        while (counter < duration)
        {
            if (Time.timeScale == 0)
                counter += Time.unscaledDeltaTime;
            else
                counter += Time.deltaTime;

            float val = Mathf.Lerp(fromVal, toVal, counter / duration);
            expSlider.fillAmount = val;
            //Debug.Log("Val: " + val);
            yield return null;
        }
    }
}
