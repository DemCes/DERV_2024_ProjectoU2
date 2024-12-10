using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneradorOrb : MonoBehaviour
{
    public GameObject orbePrefab; // El prefab del orbe a generar
    public Transform[] spawnPoints; // Array de puntos de spawn
    public int maxOrbesSimultaneos = 6; // Máximo de orbes activos a la vez
    public float intervaloDeGeneracion = 6f; // Intervalo de generación de orbes

    private float temporizador;

    void Update()
    {
        temporizador += Time.deltaTime;

        if (temporizador >= intervaloDeGeneracion)
        {
            GenerarOrbes();
            temporizador = 0f;
        }
    }

    void GenerarOrbes()
    {
        // Genera un número aleatorio de orbes en puntos aleatorios
        int orbesAGenerar = Random.Range(1, maxOrbesSimultaneos + 1);
        for (int i = 0; i < orbesAGenerar; i++)
        {
            // Seleccionar un punto de spawn aleatorio
            int indiceSpawn = Random.Range(0, spawnPoints.Length);
            Transform spawnPoint = spawnPoints[indiceSpawn];

            // Crear el orbe en la posición del punto de spawn
            Instantiate(orbePrefab, spawnPoint.position, Quaternion.identity);
        }
    }
}