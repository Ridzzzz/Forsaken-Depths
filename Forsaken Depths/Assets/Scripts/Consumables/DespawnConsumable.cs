using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DespawnConsumable : MonoBehaviour
{
    public ParticleSystem particleSystemToStop;

    void OnEnable()
    {
        gameObject.GetComponent<FloatEffects>().enabled = false;
        Material _myMaterial = GetComponent<Renderer>().material;
        StartCoroutine(FadeTo(_myMaterial, 0f, 2f));
        particleSystemToStop.Stop();
    }

    IEnumerator FadeTo(Material material, float targetOpacity, float duration) 
    {
        Color color = material.color;
        float startOpacity = color.a;
        float t = 0;

        while(t < duration) 
        {
            t += Time.deltaTime;

            float blend = Mathf.Clamp01(t / duration);

            color.a = Mathf.Lerp(startOpacity, targetOpacity, blend);

            material.color = color;

            yield return null;
        }

        ObjectPooler.Instance.ReturnToPool(transform.parent.gameObject, transform.parent.gameObject.tag);
    }
}
