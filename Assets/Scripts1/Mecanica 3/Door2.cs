using UnityEngine;
using System.Collections;

public class Door2 : MonoBehaviour
{
    GameObject thedoor2;
    public bool isUnlocked = false;

    void OnTriggerEnter(Collider obj)
    {

        if (isUnlocked)
        {
            thedoor2 = GameObject.FindWithTag("SF_Door2");
            thedoor2.GetComponent<Animation>().Play("open2");   
        }
    }

    void OnTriggerExit(Collider obj)
    {

        if (isUnlocked)
        {
            thedoor2 = GameObject.FindWithTag("SF_Door2");
            thedoor2.GetComponent<Animation>().Play("close2");
        }
    }
}