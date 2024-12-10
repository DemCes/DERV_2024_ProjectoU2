using UnityEngine;

public class LaserMov1 : MonoBehaviour
{
    public float minAngle = 0f;  // �ngulo m�nimo (0 grados)
    public float maxAngle = 90f;  // �ngulo m�ximo (90 grados)
    public float rotationSpeed = 50f;  // Velocidad de rotaci�n
    public Vector3 rotationAxis = Vector3.up;  // Eje de rotaci�n (por defecto Y)

    private float currentAngle;
    private bool rotatingForward = true;  // Controla la direcci�n de la rotaci�n

    void Start()
    {
        currentAngle = minAngle;  // Inicia en el �ngulo m�nimo
        transform.rotation = Quaternion.AngleAxis(currentAngle, rotationAxis);  // Aplicar la rotaci�n inicial
    }

    void Update()
    {
        // Rotar en una direcci�n hasta alcanzar el �ngulo m�ximo
        if (rotatingForward)
        {
            currentAngle += rotationSpeed * Time.deltaTime;
            if (currentAngle >= maxAngle)
            {
                currentAngle = maxAngle;  // Asegurarse de no pasar el �ngulo m�ximo
                rotatingForward = false;  // Cambiar la direcci�n de la rotaci�n
            }
        }
        // Rotar en la direcci�n opuesta hasta alcanzar el �ngulo m�nimo
        else
        {
            currentAngle -= rotationSpeed * Time.deltaTime;
            if (currentAngle <= minAngle)
            {
                currentAngle = minAngle;  // Asegurarse de no pasar el �ngulo m�nimo
                rotatingForward = true;  // Cambiar la direcci�n de la rotaci�n
            }
        }

        // Aplicar la rotaci�n en base al �ngulo actual
        transform.rotation = Quaternion.AngleAxis(currentAngle, rotationAxis);
    }
}
