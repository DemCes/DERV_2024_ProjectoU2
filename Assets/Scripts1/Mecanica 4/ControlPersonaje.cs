using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPersonaje : MonoBehaviour
{
    private bool puedeMoverse;

    void Start()
    {
        puedeMoverse = false; 
    }

    void Update()
    {
        if (puedeMoverse)
        {
            
        }
    }

    public void ActivarMovimiento()
    {
        puedeMoverse = true; 
    }
}