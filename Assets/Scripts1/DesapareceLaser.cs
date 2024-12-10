using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesapareceLaser : MonoBehaviour
{
    public float timeToDisappear = 5.0f;
    public float timeToReappear = 7.0f;

    private Renderer objectRenderer;
    private Collider objectCollider;
    private float timer;
    private bool isVisible = true;

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        objectCollider = GetComponent<Collider>();
        timer = 0.0f;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (isVisible && timer >= timeToDisappear)
        {
            objectRenderer.enabled = false;
            objectCollider.enabled = false;
            isVisible = false;
            timer = 0.0f;
        }
        else if (!isVisible && timer >= timeToReappear)
        {
            objectRenderer.enabled = true;
            objectCollider.enabled = true;
            isVisible = true;
            timer = 0.0f;
        }
    }
}
