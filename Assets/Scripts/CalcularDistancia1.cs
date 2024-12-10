using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalcularDistancia1 : MonoBehaviour
{
    [SerializeField] private Transform ubi_obj_cal_dist; // Referencia al jugador
    private float distance;

    void Start()
    {
        // Si no está asignado en el Inspector, intentar encontrarlo
        if (ubi_obj_cal_dist == null)
        {
            GameObject jugador = GameObject.Find("Jugador");
            if (jugador != null)
            {
                ubi_obj_cal_dist = jugador.transform;
            }
            else
            {
                Debug.LogError("No se encuentra el objeto 'Jugador' en la escena!");
            }
        }
    }

    void Update()
    {
        if (ubi_obj_cal_dist != null)
        {
            distance = Vector3.Distance(transform.position, ubi_obj_cal_dist.position);
        }
    }

    public float getDistance()
    {
        return distance;
    }
}