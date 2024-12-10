using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class finish_Trigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Finish");
    }
}
