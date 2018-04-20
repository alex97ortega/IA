using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GUI : MonoBehaviour {

    public GameObject botonAtacar;
    public GameObject botonVolver;
    public GameObject botonEsperar;

    public GameObject textoPuntos;
    public GameObject textoTurnos;
    public void Comenzar()
    {

        botonAtacar.SetActive(true);
        botonVolver.SetActive(true);
        botonEsperar.SetActive(true);
        
        textoTurnos.SetActive(true);
    }

    public void Reinciar()
    {
        botonAtacar.SetActive(false);
        botonVolver.SetActive(false);
        botonEsperar.SetActive(false);
        
        textoTurnos.SetActive(false);
    }
}
