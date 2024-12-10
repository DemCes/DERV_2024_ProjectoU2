using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MovimientoRecto : MonoBehaviour
{
    [SerializeField] private AdvancedMovements advancedMovements;
    [SerializeField] private Transform puntoFinal;
    [SerializeField] private float velocidadMovimiento = 5f;
    //[SerializeField] private TextMeshProUGUI Explicacion;

    // Referencias a los puntos de teletransporte
    [SerializeField] private Transform puntoTeletransporteA;
    [SerializeField] private Transform puntoTeletransporteG;
    [SerializeField] private Transform puntoTeletransporteL;

    private bool moviendoseEnLineaRecta = false;

    //[SerializeField] private Transform teleportTargetLaser;
    private void Update()
    {
        if (moviendoseEnLineaRecta)
        {
            // Mover continuamente hacia el punto final
            float movimientoZ = velocidadMovimiento * Time.deltaTime;
            transform.position += new Vector3(0, 0, -movimientoZ);

            // Verificar si hemos llegado al punto final
            if (transform.position.z <= puntoFinal.position.z)
            {
                DetenerMovimientoLineal();
            }

            // Manejo de teletransportes usando los puntos de referencia
            if (Input.GetKeyDown(KeyCode.L) && puntoTeletransporteA != null)
            {
                TeletransportarseAPunto(puntoTeletransporteA);
            }
            else if (Input.GetKeyDown(KeyCode.G) && puntoTeletransporteG != null)
            {
                TeletransportarseAPunto(puntoTeletransporteG);
            }
            else if (Input.GetKeyDown(KeyCode.A) && puntoTeletransporteL != null)
            {
                TeletransportarseAPunto(puntoTeletransporteL);
            }
        }
    }


    private void TeletransportarseAPunto(Transform punto)
    {
        Vector3 nuevaPosicion = new Vector3(
            punto.position.x,
            transform.position.y,
            transform.position.z
        );

        transform.position = nuevaPosicion;
      
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ActivadorLineaRecta"))
        {
            //Explicacion.SetActive = true;
            ActivarMovimientoLineal();
        }
        else if (other.CompareTag("DetenerLineaRecta"))
        {
            DetenerMovimientoLineal();
        }
    }

    private void ActivarMovimientoLineal()
    {
        if (advancedMovements != null)
        {
            advancedMovements.enabled = false;
        }

        moviendoseEnLineaRecta = true;
        Debug.Log("Movimiento lineal activado");
    }

    private void DetenerMovimientoLineal()
    {
        if (advancedMovements != null)
        {
            //Explicacion.setActive = false;
            advancedMovements.enabled = true;
        }

        moviendoseEnLineaRecta = false;
        Debug.Log("Movimiento lineal desactivado");
    }
}