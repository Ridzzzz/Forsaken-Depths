using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiBarText : MonoBehaviour
{
    Transform cam;

    Vector3 posOffset = new Vector3(0f, 0.75f, 0f);

    Vector3 randomIntensityFactor = new Vector3(0.2f, 0f, 0.1f);

    void OnEnable()
    {
        cam = Camera.main.transform;

        transform.localPosition += posOffset;
        transform.localPosition += new Vector3(Random.Range(-randomIntensityFactor.x, randomIntensityFactor.x), 0f, Random.Range(-randomIntensityFactor.z, randomIntensityFactor.z));
        
        StartCoroutine(ScaleUpValueOverTime(0.45f, 0.60f, 0.5f));
        StartCoroutine(ScaleDownValueOverTime(0.60f, 0f, 0.5f));
    }

    void LateUpdate()
    {
        transform.forward = cam.forward;
    }

    IEnumerator ScaleUpValueOverTime(float fromVal, float toVal, float duration)
    {
        float counter = 0f;

        while (counter < duration)
        {
            if (Time.timeScale == 0)
                counter += Time.unscaledDeltaTime;
            else
                counter += Time.deltaTime;

            float val = Mathf.Lerp(fromVal, toVal, counter / duration);
            transform.localScale = new Vector3(val, val, val);
            //Debug.Log("Val: " + val);
            yield return null;
        }
    }

    IEnumerator ScaleDownValueOverTime(float fromVal, float toVal, float duration)
    {
        yield return new WaitForSeconds(0.5f);

        float counter = 0f;

        while (counter < duration)
        {
            if (Time.timeScale == 0)
                counter += Time.unscaledDeltaTime;
            else
                counter += Time.deltaTime;

            float val = Mathf.Lerp(fromVal, toVal, counter / duration);
            transform.localScale = new Vector3(val, val, val);
            //Debug.Log("Val: " + val);
            yield return null;
        }

        ObjectPooler.Instance.ReturnToPool(gameObject.transform.parent.gameObject, gameObject.transform.parent.gameObject.tag);
    }
}
