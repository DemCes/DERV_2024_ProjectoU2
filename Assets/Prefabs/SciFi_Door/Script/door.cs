using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour
{
    GameObject thedoor;
    public bool isUnlocked = false; 

    void OnTriggerEnter(Collider obj)
    {
        
        if (isUnlocked)
        {
            thedoor = GameObject.FindWithTag("SF_Door");
            thedoor.GetComponent<Animation>().Play("open");
        }
    }

    void OnTriggerExit(Collider obj)
    {
        
        if (isUnlocked)
        {
            thedoor = GameObject.FindWithTag("SF_Door");
            thedoor.GetComponent<Animation>().Play("close");
        }
    }
}