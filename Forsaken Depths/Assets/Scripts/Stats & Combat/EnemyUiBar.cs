using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterStat))]
public class EnemyUiBar : MonoBehaviour
{
    Transform target;
    Transform ui;
    Transform cam;
    CanvasGroup hpBarBackground;
    Image healthSlider;
    
	void OnEnable () 
    {
        cam = Camera.main.transform;
        ui = transform.GetChild(0).transform;
        target = ui.parent.transform;
        hpBarBackground = ui.GetChild(0).GetComponent<CanvasGroup>();
        healthSlider = ui.GetChild(0).GetChild(0).GetComponent<Image>();        

        GetComponent<CharacterStat>().OnHealthChanged += OnHealthChanged;
	}

    void OnHealthChanged(Stat health, float currentHealth) 
    {       
        if (ui != null)
        {
            float healthPercent = currentHealth / health.GetValue();
            StartCoroutine(changeHealthValueOverTime(healthSlider.fillAmount, healthPercent, 0.25f));
            
            if (currentHealth <= 0)
            {
                StartCoroutine(changeAlphaValueOverTime(1f, 0f, 1f));
            }
        }
    }

    void LateUpdate () 
    {
        if (ui != null)
        {
            ui.position = target.position + new Vector3(0f, 2.25f, 0f);
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
}
