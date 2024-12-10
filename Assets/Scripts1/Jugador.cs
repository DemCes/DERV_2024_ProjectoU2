using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jugador : MonoBehaviour
{
    [SerializeField] float velocidad_movimiento = 10f;
    [SerializeField] float sensibilidadMouse = 100f;
    [SerializeField] float fuerzaSalto = 5f; // Fuerza del salto
    public Transform camara;   // Asigna la c�mara en el Inspector

    private Rigidbody rb;

    // Variables de rotaci�n
    private float rotacionX = 0f;
    private float rotacionY = 0f;

    // L�mites para la rotaci�n
    private float limiteGiroLateral = 5000f;  // 91 grados de rango total (�45.5 grados)
    private float limiteGiroArriba = 55f;
    private float limiteGiroAbajo = 75f;

    // Variable para verificar si el jugador est� en el suelo
    private bool estaEnElSuelo;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Congelar las rotaciones del Rigidbody en los ejes X y Z
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        // Ocultar y bloquear el cursor en el centro de la pantalla
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Control de la rotaci�n de la c�mara y el cuerpo del jugador
        RotarJugadorYCamara();

        // Movimiento del personaje
        MoverJugador();

        // Comprobar si el jugador puede saltar
        if (estaEnElSuelo && Input.GetButtonDown("Jump")) // "Jump" corresponde a la tecla Space
        {
            Saltar();
        }
    }

    // Funci�n para rotar el jugador y la c�mara
    void RotarJugadorYCamara()
    {
        // Obtener la entrada del mouse
        float mouseX = Input.GetAxis("Mouse X") * sensibilidadMouse * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensibilidadMouse * Time.deltaTime;

        // Rotaci�n horizontal del cuerpo (solo en el eje Y)
        rotacionY += mouseX;

        // Clampear la rotaci�n horizontal dentro de los 91 grados (�45.5 grados)
        rotacionY = Mathf.Clamp(rotacionY, -limiteGiroLateral, limiteGiroLateral);

        // Rotaci�n vertical de la c�mara (solo en el eje X)
        rotacionX -= mouseY;

        // Clampear la rotaci�n vertical para mirar hacia arriba y hacia abajo
        rotacionX = Mathf.Clamp(rotacionX, -limiteGiroAbajo, limiteGiroArriba);

        // Aplicar la rotaci�n al cuerpo del jugador (rotaci�n horizontal)
        transform.localRotation = Quaternion.Euler(0f, rotacionY, 0f);

        // Aplicar la rotaci�n a la c�mara (rotaci�n vertical)
        camara.localRotation = Quaternion.Euler(rotacionX, 0f, 0f);
    }

    // Funci�n para mover al jugador
    void MoverJugador()
    {
        // Obtener la entrada de las teclas de movimiento (W, A, S, D)
        float moverAdelanteAtras = Input.GetAxis("Vertical") * velocidad_movimiento * Time.deltaTime;
        float moverLados = Input.GetAxis("Horizontal") * velocidad_movimiento * Time.deltaTime;

        // Mover el cuerpo del jugador en la direcci�n hacia adelante/atr�s y hacia los lados
        Vector3 movimiento = transform.forward * moverAdelanteAtras + transform.right * moverLados;

        // Aplicar el movimiento al Rigidbody del cuerpo del jugador
        rb.MovePosition(rb.position + movimiento);
    }

    // Funci�n para saltar
    void Saltar()
    {
        rb.AddForce(Vector3.up * fuerzaSalto, ForceMode.Impulse);
        estaEnElSuelo = false; // El jugador ya no est� en el suelo despu�s de saltar
    }

    void OnCollisionEnter(Collision collision)
    {
        // Verifica si el jugador colisiona con el suelo
        if (collision.gameObject.CompareTag("IsGround"))
        {
            estaEnElSuelo = true; // El jugador est� en el suelo
        }
    }
}
