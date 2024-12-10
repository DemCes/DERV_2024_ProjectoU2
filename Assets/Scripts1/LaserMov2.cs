using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserMov2 : MonoBehaviour
{
    public float speed = 2.0f; // Velocidad de movimiento
    public float angleRange = 45.0f; // Ángulo de oscilación (45 grados en total)
    public bool reverseDirection = false; // Nuevo parámetro para invertir dirección

    private float startAngle; // Ángulo de inicio en el eje X
    private float angleOffset; // El valor del ángulo en cada momento

    void Start()
    {
        // Asignamos el ángulo inicial en el eje X
        startAngle = transform.rotation.eulerAngles.x;
    }

    void Update()
    {
        // Creamos un movimiento pendular basado en una función senoidal
        angleOffset = Mathf.Sin(Time.time * speed) * angleRange / 2;

        // Si reverseDirection es verdadero, invertimos el ángulo
        if (reverseDirection)
        {
            angleOffset = -angleOffset; // Invertir el movimiento
        }

        // Aplicamos el ángulo resultante al láser en el eje X
        transform.rotation = Quaternion.Euler(startAngle + angleOffset, 0, 0);
    }
}