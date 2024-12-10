using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OsciladorPlataforma : MonoBehaviour
{
    public Vector3 puntoA; // Punto inicial de oscilación
    public Vector3 puntoB; // Punto final de oscilación
    public float velocidad = 1f; // Velocidad de oscilación
    private float tiempoTranscurrido;

    void Update()
    {
        // Calcular la posición actual en función del tiempo
        tiempoTranscurrido += Time.deltaTime * velocidad;
        float t = Mathf.PingPong(tiempoTranscurrido, 1);
        transform.position = Vector3.Lerp(puntoA, puntoB, t);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Hacer al jugador hijo de la plataforma
            collision.transform.SetParent(transform);
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Eliminar al jugador como hijo de la plataforma
            collision.transform.SetParent(null);
        }
    }
}