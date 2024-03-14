using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageFlash : MonoBehaviour
{
    public float flashDuration = 0.3f;
    public float flashIntensity = 1.0f;

    public Material baseMaterial; // Assign the base material in the Inspector
    private Material flashMaterial; // Material instance for this object

    void Start()
    {
        // Create a new material instance for this object
        flashMaterial = new Material(baseMaterial);
        // Assign the new material instance to the object's renderer
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
