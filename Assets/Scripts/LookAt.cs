using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    [SerializeField] private Transform ubi_obj_a_mirar;
    private Vector3 posicionInicial;

    private void Start()
    {
        if (ubi_obj_a_mirar == null)
        {
            GameObject jugador = GameObject.Find("Jugador");
            if (jugador != null)
            {
                ubi_obj_a_mirar = jugador.transform;
            }
            else
            {
                Debug.LogError("No se encuentra el objeto 'Jugador' en la escena!");
                return;
            }
        }

        posicionInicial = transform.position;
    }

    void Update()
    {
        if (ubi_obj_a_mirar == null) return;

        // Hace que el enemigo mire constantemente al jugador
        transform.LookAt(ubi_obj_a_mirar.position);
    }

    public void RegresarAlPuntoInicial()
    {
        transform.position = posicionInicial;
    }
}