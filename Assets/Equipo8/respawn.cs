using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class respawn : MonoBehaviour
{
    [SerializeField] Handler_Game8 handler;

    private void OnTriggerEnter(Collider other)
    {
        other.transform.position = handler.spawn.position;
        other.transform.rotation = handler.spawn.rotation;
    }
}
