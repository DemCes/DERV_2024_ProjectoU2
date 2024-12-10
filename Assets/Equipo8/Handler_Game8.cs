using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Handler_Game8 : MonoBehaviour
{
    [SerializeField] public Transform spawn;
    [SerializeField] TextMeshProUGUI txtTimer;
    [SerializeField] PlayerController player;
    int segundos;

    void Start()
    {
        segundos = 15;
        player.transform.position = spawn.position;
        player.transform.rotation = spawn.rotation;
        StartCoroutine(timer());
    }

    IEnumerator timer()
    {
        while (segundos > -1)
        {
            txtTimer.text = formatTimer(segundos);
            segundos--;
            yield return new WaitForSeconds(1.0f);
        }
        Debug.Log("Se termino el juego");
        player.DisableMovement();
    }

    string formatTimer(int seconds)
    {
        int min, sec;
        string minutos, segundos;
        min = seconds / 60;
        sec = seconds % 60;


        if (min < 10)
        {
            minutos = "0" + min.ToString();
        }
        else
        {
            minutos = min.ToString();
        }

        if (sec < 10)
        {
            segundos = "0" + sec.ToString();
        }
        else
        {
            segundos = sec.ToString();
        }

        return minutos + ":" + segundos;
    }
}
