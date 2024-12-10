using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rota : MonoBehaviour
{
    public float rotationSpeed = 50.0f; // Velocidad de rotación en grados por segundo

    void Update()
    {
        // Rotar el objeto alrededor de su eje Y
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }
}
    