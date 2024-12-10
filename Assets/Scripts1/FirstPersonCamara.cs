using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCamara : MonoBehaviour
{
    public float sensX,sensY;

    public Transform orientacion;

     float rotacionX,rotacionY;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X")*Time.deltaTime*sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y")*Time.deltaTime*sensY;

        rotacionY += mouseX;
        rotacionX -= mouseY;

        rotacionX = Mathf.Clamp(rotacionX,-90f,90f);

        transform.rotation = Quaternion.Euler(rotacionX, rotacionY,0);
        orientacion.rotation = Quaternion.Euler(0,rotacionY,0);
    }
}
