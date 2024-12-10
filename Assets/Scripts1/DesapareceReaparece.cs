using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesapareceReaparece : MonoBehaviour
{
    public float tiempoParaDesaparecer = 2.0f; // Tiempo desde que el jugador toca la plataforma hasta que desaparece
    public float tiempoParaReaparecer = 5.0f; // Tiempo desde que desaparece hasta que vuelve a aparecer

    private Renderer puenteRenderer;
    private Collider puenteCollider;
    private Color colorOriginal;
    private bool puenteDesaparecido = false;

    void Start()
    {
        puenteRenderer = GetComponent<Renderer>();
        puenteCollider = GetComponent<Collider>();
        colorOriginal = puenteRenderer.material.color;
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player") && !puenteDesaparecido)
        {
            // Iniciar el proceso de desaparición después del tiempo especificado
            Invoke("DesaparecerPuente", tiempoParaDesaparecer);
        }
    }

    void DesaparecerPuente()
    {
        puenteRenderer.enabled = false;
        puenteCollider.enabled = false;
        puenteDesaparecido = true;

        // Programar la reaparición de la plataforma después de tiempoParaReaparecer
        Invoke("ReaparecerPuente", tiempoParaReaparecer);
    }

    void ReaparecerPuente()
    {
        puenteRenderer.enabled = true;
        puenteCollider.enabled = true;
        puenteRenderer.material.color = colorOriginal;
        puenteDesaparecido = false;
    }
}

