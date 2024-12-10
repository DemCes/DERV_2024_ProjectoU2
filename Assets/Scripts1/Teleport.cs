using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public GameObject reset; // El objeto al que se teletransportar� el jugador

    void OnCollisionEnter(Collision other)
    {
        // Comprobamos si el objeto que colisiona es el jugador
        if (other.gameObject.CompareTag("Player") && reset != null)
        {
            // Teletransportamos al jugador a la posici�n del objeto "reset"
            other.transform.position = reset.transform.position;
        }
    }
}