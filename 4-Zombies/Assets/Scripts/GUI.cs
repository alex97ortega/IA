using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GUI : MonoBehaviour {

    public GameObject botonAtacar;
    public GameObject botonVolver;
    public GameObject botonEsperar;

    public void Comenzar()
    {

        botonAtacar.SetActive(true);
        botonVolver.SetActive(true);
        botonEsperar.SetActive(true);
    }
}
