using UnityEngine;

public class LaserMov1 : MonoBehaviour
{
    public float minAngle = 0f;  // Ángulo mínimo (0 grados)
    public float maxAngle = 90f;  // Ángulo máximo (90 grados)
    public float rotationSpeed = 50f;  // Velocidad de rotación
    public Vector3 rotationAxis = Vector3.up;  // Eje de rotación (por defecto Y)

    private float currentAngle;
    private bool rotatingForward = true;  // Controla la dirección de la rotación

    void Start()
    {
        currentAngle = minAngle;  // Inicia en el ángulo mínimo
        transform.rotation = Quaternion.AngleAxis(currentAngle, rotationAxis);  // Aplicar la rotación inicial
    }

    void Update()
    {
        // Rotar en una dirección hasta alcanzar el ángulo máximo
        if (rotatingForward)
        {
            currentAngle += rotationSpeed * Time.deltaTime;
            if (currentAngle >= maxAngle)
            {
                currentAngle = maxAngle;  // Asegurarse de no pasar el ángulo máximo
                rotatingForward = false;  // Cambiar la dirección de la rotación
            }
        }
        // Rotar en la dirección opuesta hasta alcanzar el ángulo mínimo
        else
        {
            currentAngle -= rotationSpeed * Time.deltaTime;
            if (currentAngle <= minAngle)
            {
                currentAngle = minAngle;  // Asegurarse de no pasar el ángulo mínimo
                rotatingForward = true;  // Cambiar la dirección de la rotación
            }
        }

        // Aplicar la rotación en base al ángulo actual
        transform.rotation = Quaternion.AngleAxis(currentAngle, rotationAxis);
    }
}
