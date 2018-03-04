using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour {

    public Text textoMov;
    public Text textoEst;
    public int numMov;
    public float tiempo=0.0f;

    bool finished = false;
    private void Update()
    {
        if (!finished)
        {
            tiempo += Time.deltaTime;
            textoMov.text = "     Movimientos: " + numMov.ToString() + "\n\nTiempo: "
                + tiempo.ToString("0") + " segundos";
        }  
    }
    public void SumaMov () {

        numMov++;
        
    }
	public void Reiniciando()
    {
        numMov=0;
        tiempo = 0.0f;
        finished = false;
        textoMov.text = "     Movimientos: " + numMov.ToString() + "\n\nTiempo: " 
            + tiempo.ToString("0") + " segundos";
    }
	public void Estadisticas()
    {
        finished = true;
        textoEst.text = "ESTADÍSTICAS\n\n\nMovimientos totales: " + numMov + "\n\nTiempo total: "
            + tiempo.ToString("0") + " segundos\n\n" + numMov / tiempo + " teclas por segundo";
    }
}
