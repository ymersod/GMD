using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageFlash : MonoBehaviour
{
    public float flashDuration = 0.3f;
    public float flashIntensity = 1.0f;

    public Material baseMaterial;
    private Material flashMaterial;

    void Start()
    {
        flashMaterial = new Material(baseMaterial);
        GetComponent<Renderer>().material = flashMaterial;
    }

    public void TakeDamage()
    {
        StartCoroutine(Flash());
    }

    IEnumerator Flash()
    {
        flashMaterial.SetFloat("_FlashAmount", flashIntensity);
        flashMaterial.SetFloat("_SelfIllum", flashIntensity);
        yield return new WaitForSeconds(flashDuration);
        flashMaterial.SetFloat("_FlashAmount", 0);
        flashMaterial.SetFloat("_SelfIllum", 1f);
    }
}
