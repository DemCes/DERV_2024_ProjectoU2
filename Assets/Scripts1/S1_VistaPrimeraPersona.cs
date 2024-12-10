using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S1_VistaPrimeraPersona : MonoBehaviour
{
    [SerializeField] float velocidad_movimiento = 10f;
    [SerializeField] float sensibilidadMouse = 100f;
    [SerializeField] float fuerzaGravedadExtra = 5f;
    public Transform Vista;  

    private Rigidbody rb;

    private float rotacionX = 0f;
    private float rotacionY = 0f;

    private float limiteGiroLateral = 10000f; 
    private float limiteGiroArriba = 55f;
    private float limiteGiroAbajo = 75f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

     
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        MoverJugador();
    }

    void LateUpdate()
    {
        RotarJugadorYCamara();
    }

    void RotarJugadorYCamara()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensibilidadMouse * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensibilidadMouse * Time.deltaTime;

        rotacionY += mouseX;


        rotacionY = Mathf.Clamp(rotacionY, -limiteGiroLateral, limiteGiroLateral);

        rotacionX -= mouseY;

        rotacionX = Mathf.Clamp(rotacionX, -limiteGiroAbajo, limiteGiroArriba);


        transform.localRotation = Quaternion.Euler(0f, rotacionY, 0f);

        
        Vista.localRotation = Quaternion.Euler(rotacionX, 0f, 0f);
    }


    void MoverJugador()
    {

        float moverAdelanteAtras = Input.GetAxis("Vertical") * velocidad_movimiento * Time.deltaTime;
        float moverLados = Input.GetAxis("Horizontal") * velocidad_movimiento * Time.deltaTime;

        Vector3 movimiento = transform.forward * moverAdelanteAtras + transform.right * moverLados;

        rb.MovePosition(rb.position + movimiento);
        rb.AddForce(Physics.gravity * fuerzaGravedadExtra, ForceMode.Acceleration);
    }
}