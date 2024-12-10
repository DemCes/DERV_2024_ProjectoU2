        using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    bool canMove;

    void Start()
    {
        canMove = true;
    }

    void Update()
    {
        if (canMove)
        {
            if (Input.GetKey(KeyCode.Q))
            { //giro a la izquierda
                transform.Rotate(0, 6f * -1, 0);
            }
            else if (Input.GetKey(KeyCode.E)) //giro a la derecha
            {
                transform.Rotate(0, 6f, 0);
            }

            //arriba - abajo
            if (Input.GetKey(KeyCode.W))
            {
                transform.position += transform.forward * 5f * Time.deltaTime;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                transform.position += transform.forward * -1 * 5f * Time.deltaTime;
            }

            //izquierda - derecha
            if (Input.GetKey(KeyCode.A))
            {
                transform.position += transform.right * -1 * 5f * Time.deltaTime;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                transform.position += transform.right * 5f * Time.deltaTime;
            }
        }
    }

    public void DisableMovement()
    {
        canMove = false;
    }
}
