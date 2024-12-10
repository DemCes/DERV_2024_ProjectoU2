using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColisionOrbe : MonoBehaviour
{
    [SerializeField] ManagerUI managerUI;
    private void OnCollisionEnter(Collision other)
    {

               managerUI.TocarOrbe();
                Destroy(gameObject);

    }
}
