using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimientoEnemigo1 : MonoBehaviour
{
    public Transform Jugador; // El transform del jugador
    float velociad = 2f; // Velocidad de movimiento de los enemigos
    private Vector3 ultimaPosPlayer; // Última posición del jugador

    void Start()
    {
        // Inicializa la última posición del jugador
        ultimaPosPlayer = Jugador.position;
    }

    void Update()
    {
        if (Jugador.position != ultimaPosPlayer)
        {
            Debug.Log("El jugador se está moviendo.");
            MoveTowardsPlayer();
        }
        else
        {
            Debug.Log("El jugador está quieto.");
        }
        ultimaPosPlayer = Jugador.position;
    }

    void MoveTowardsPlayer()
    {
        Vector3 direction = (Jugador.position - transform.position).normalized;
        transform.position += direction * velociad * Time.deltaTime;
    }
}