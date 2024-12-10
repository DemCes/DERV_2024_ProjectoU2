using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OsciladorPlataforma : MonoBehaviour
{
    public Vector3 puntoA; // Punto inicial de oscilaci�n
    public Vector3 puntoB; // Punto final de oscilaci�n
    public float velocidad = 1f; // Velocidad de oscilaci�n
    private float tiempoTranscurrido;

    void Update()
    {
        // Calcular la posici�n actual en funci�n del tiempo
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