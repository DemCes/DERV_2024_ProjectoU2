using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class platform_controller1 : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Real")
        {
            collision.gameObject.GetComponent<Renderer>().material.color = Color.green;
        } 
        else if (collision.gameObject.tag == "Fake")
        {
            StartCoroutine(fake(collision.gameObject));
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Real")
        {
            collision.gameObject.GetComponent<Renderer>().material.color = Color.white;
        }
    }

    IEnumerator fake(GameObject other)
    {
        yield return new WaitForSeconds(0.15f);
        other.SetActive(false);
        yield return new WaitForSeconds(1.0f);
        other.SetActive(true);
    }
}
