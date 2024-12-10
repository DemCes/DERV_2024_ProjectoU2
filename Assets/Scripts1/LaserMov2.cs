using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserMov2 : MonoBehaviour
{
    public float speed = 2.0f; // Velocidad de movimiento
    public float angleRange = 45.0f; // �ngulo de oscilaci�n (45 grados en total)
    public bool reverseDirection = false; // Nuevo par�metro para invertir direcci�n

    private float startAngle; // �ngulo de inicio en el eje X
    private float angleOffset; // El valor del �ngulo en cada momento

    void Start()
    {
        // Asignamos el �ngulo inicial en el eje X
        startAngle = transform.rotation.eulerAngles.x;
    }

    void Update()
    {
        // Creamos un movimiento pendular basado en una funci�n senoidal
        angleOffset = Mathf.Sin(Time.time * speed) * angleRange / 2;

        // Si reverseDirection es verdadero, invertimos el �ngulo
        if (reverseDirection)
        {
            angleOffset = -angleOffset; // Invertir el movimiento
        }

        // Aplicamos el �ngulo resultante al l�ser en el eje X
        transform.rotation = Quaternion.Euler(startAngle + angleOffset, 0, 0);
    }
}