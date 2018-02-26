using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour {

    Text textoMov;
    int numMov;

    void Awake()
    {
        textoMov = GameObject.Find("TextoMovimientos").GetComponent(typeof(Text)) as Text;
    }

    public void SumaMov () {

        numMov++;
        textoMov.text = "Movimientos: " + numMov.ToString();
    }
	
	
}
