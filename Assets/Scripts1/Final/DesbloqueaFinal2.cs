using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DesbloqueaFinal2 : MonoBehaviour
{
    public GameObject door; 
    public GameObject additionalDoor; 
    public TextMeshPro textMeshPro; 
    public Material glowWinDoorMaterial;
    public Color newTextColor = Color.green; 
    public Color defaultTextColor = Color.white; 

    public GameObject[] objectsToDisappear; 

    private Renderer doorRenderer; 
    private Renderer additionalDoorRenderer; 
    private bool doorUnlocked = false; 
    private Door2 doorScript; 

    void Start()

    {
        
        if (door != null)
        {
            doorRenderer = door.GetComponent<Renderer>();
            doorScript = door.GetComponent<Door2>();
            doorScript.enabled = false; 
        }

        if (additionalDoor != null)
        {
            additionalDoorRenderer = additionalDoor.GetComponent<Renderer>();
        }

       
        if (textMeshPro != null)
        {
            textMeshPro.color = defaultTextColor;
        }
    }

    public void UnlockDoor()
    {
        

        
        if (doorRenderer != null && glowWinDoorMaterial != null)
        {
            doorRenderer.material = glowWinDoorMaterial;
        }

        
        if (additionalDoorRenderer != null && glowWinDoorMaterial != null)
        {
            additionalDoorRenderer.material = glowWinDoorMaterial;
        }

        
        if (textMeshPro != null)
        {
            textMeshPro.color = newTextColor;
        }

       
        if (doorScript != null)
        {
            doorScript.isUnlocked = true;
            doorScript.enabled = true; 
        }

        
        foreach (GameObject obj in objectsToDisappear)
        {
            if (obj != null)
            {
                obj.SetActive(false);
            }
        }

             
        gameObject.SetActive(false); 
    }
}