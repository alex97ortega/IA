using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fondo : MonoBehaviour {

    public GameManager gm;
    void OnMouseDown()
    {
        gm.SeleccionarVacio();
    }
}
