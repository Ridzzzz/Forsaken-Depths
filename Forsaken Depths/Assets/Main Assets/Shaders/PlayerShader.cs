using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShader : MonoBehaviour
{
    public ParticleSystem effect1, effect2, effect3, effect4, effect5;
    float currentDissolveValue;

    float minDissolveValue = 0.5f;
    float maxDissolveValue = -0.25f;

    float effectTimer = 2f;

    bool playerIsDead;

    void Start()
    {
        playerIsDead = false;

        effect2.Stop();
        effect4.Stop();
        effect5.Stop();

        currentDissolveValue = minDissolveValue;

        transform.GetComponent<Renderer>().material.SetFloat("_DisAmount", currentDissolveValue);
    }

    void Update()
    {
        if (playerIsDead)
        {
            transform.GetComponent<Renderer>().material.SetFloat("_DisAmount", currentDissolveValue);
        }        
    }

    public void ExecuteDeathSequence()
    {
        playerIsDead = true;
        StartCoroutine(changeDissolveValueOverTime(minDissolveValue, maxDissolveValue, effectTimer));
    }

    IEnumerator changeDissolveValueOverTime(float fromVal, float toVal, float duration)
    {
        effect1.Stop();        
        effect3.Stop();

        effect2.Play();
        effect4.Play();

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
        
        yield return new WaitForSeconds(1f);

        effect2.Stop();
        effect4.Stop();
    }
}
