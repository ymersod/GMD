using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fix_material : MonoBehaviour
{
    public Texture m_MainTexture, m_Normal, m_Metal;
    Renderer m_Renderer;
    void Start()
    {
        m_Renderer = GetComponent<Renderer> ();

        m_Renderer.material.EnableKeyword ("_NORMALMAP");
        m_Renderer.material.EnableKeyword ("_METALLICGLOSSMAP");

        m_Renderer.material.SetTexture("_MainTex", m_MainTexture);
        m_Renderer.material.SetTexture("_BumpMap", m_Normal);
        m_Renderer.material.SetTexture ("_MetallicGlossMap", m_Metal);
    }

}
