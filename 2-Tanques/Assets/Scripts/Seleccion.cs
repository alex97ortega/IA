using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seleccion : MonoBehaviour {

    public GameManager gm;
    public GameObject tanque;
    
    void OnMouseDown()
    {
        gm.nadaSeleccionado = false;
        gm.Seleccionar(tanque);
    }
    
}
