using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jugador : MonoBehaviour
{
    [SerializeField] float velocidad_movimiento = 10f;
    [SerializeField] float sensibilidadMouse = 100f;
    [SerializeField] float fuerzaSalto = 5f; // Fuerza del salto
    public Transform camara;   // Asigna la cámara en el Inspector

    private Rigidbody rb;

    // Variables de rotación
    private float rotacionX = 0f;
    private float rotacionY = 0f;

    // Límites para la rotación
    private float limiteGiroLateral = 5000f;  // 91 grados de rango total (±45.5 grados)
    private float limiteGiroArriba = 55f;
    private float limiteGiroAbajo = 75f;

    // Variable para verificar si el jugador está en el suelo
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
        // Control de la rotación de la cámara y el cuerpo del jugador
        RotarJugadorYCamara();

        // Movimiento del personaje
        MoverJugador();

        // Comprobar si el jugador puede saltar
        if (estaEnElSuelo && Input.GetButtonDown("Jump")) // "Jump" corresponde a la tecla Space
        {
            Saltar();
        }
    }

    // Función para rotar el jugador y la cámara
    void RotarJugadorYCamara()
    {
        // Obtener la entrada del mouse
        float mouseX = Input.GetAxis("Mouse X") * sensibilidadMouse * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensibilidadMouse * Time.deltaTime;

        // Rotación horizontal del cuerpo (solo en el eje Y)
        rotacionY += mouseX;

        // Clampear la rotación horizontal dentro de los 91 grados (±45.5 grados)
        rotacionY = Mathf.Clamp(rotacionY, -limiteGiroLateral, limiteGiroLateral);

        // Rotación vertical de la cámara (solo en el eje X)
        rotacionX -= mouseY;

        // Clampear la rotación vertical para mirar hacia arriba y hacia abajo
        rotacionX = Mathf.Clamp(rotacionX, -limiteGiroAbajo, limiteGiroArriba);

        // Aplicar la rotación al cuerpo del jugador (rotación horizontal)
        transform.localRotation = Quaternion.Euler(0f, rotacionY, 0f);

        // Aplicar la rotación a la cámara (rotación vertical)
        camara.localRotation = Quaternion.Euler(rotacionX, 0f, 0f);
    }

    // Función para mover al jugador
    void MoverJugador()
    {
        // Obtener la entrada de las teclas de movimiento (W, A, S, D)
        float moverAdelanteAtras = Input.GetAxis("Vertical") * velocidad_movimiento * Time.deltaTime;
        float moverLados = Input.GetAxis("Horizontal") * velocidad_movimiento * Time.deltaTime;

        // Mover el cuerpo del jugador en la dirección hacia adelante/atrás y hacia los lados
        Vector3 movimiento = transform.forward * moverAdelanteAtras + transform.right * moverLados;

        // Aplicar el movimiento al Rigidbody del cuerpo del jugador
        rb.MovePosition(rb.position + movimiento);
    }

    // Función para saltar
    void Saltar()
    {
        rb.AddForce(Vector3.up * fuerzaSalto, ForceMode.Impulse);
        estaEnElSuelo = false; // El jugador ya no está en el suelo después de saltar
    }

    void OnCollisionEnter(Collision collision)
    {
        // Verifica si el jugador colisiona con el suelo
        if (collision.gameObject.CompareTag("IsGround"))
        {
            estaEnElSuelo = true; // El jugador está en el suelo
        }
    }
}
