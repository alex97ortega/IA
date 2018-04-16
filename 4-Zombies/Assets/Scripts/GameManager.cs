using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public int filas, columnas;
    public GameObject casilla;
    
    public GameObject[,] casillas;
    Vector3[,] positions;
    

    GameObject tablero;
    bool noche = false;
    bool comenzado = false;

    // Use this for initialization
    void Start () {
        tablero = GameObject.Find("Tablero");
        casillas = new GameObject[columnas, filas];
        positions = new Vector3[columnas, filas];
        
        for (int i = 0; i < columnas; i++)
        {
            for (int j = 0; j < filas; j++)
            {
                positions[i, j] = new Vector3(i, j, 0);
                GameObject ficha = Instantiate(casilla, positions[i, j], Quaternion.identity);
                casillas[i, j] = ficha;

                ficha.transform.parent = tablero.transform;
                ficha.name = casillas[i, j].name;
            }
        }
    }
	
	public void CambiarNoche()
    {
        noche = !noche;
    }
    public void Comenzar()
    {
        comenzado = true;
    }
    public void Reiniciar()
    {
        comenzado = false;
    }
}
