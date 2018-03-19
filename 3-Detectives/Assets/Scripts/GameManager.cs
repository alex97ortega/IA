using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {


    public int filas, columnas;
    public GameObject casilla;
    public GameObject arma;
    public GameObject muerto;


    GameObject[,] casillas;
    Vector3[,] positions;

    GameObject tablero;

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
	
	public void Asesinar()
    {
        LimpiarTablero();
        int random;
        random = Random.Range(1, filas*columnas); // casilla 0,0 reservada para la casa
      

        //colocar sangre
        casillas[random % columnas, random / columnas].GetComponent<IdCasilla>().ChangeId(2);
        if (random % columnas > 0) casillas[random % columnas - 1, random / columnas].GetComponent<IdCasilla>().ChangeId(2);
        if (random % columnas < columnas-1) casillas[random % columnas + 1, random / columnas].GetComponent<IdCasilla>().ChangeId(2);
        if (random / columnas > 0) casillas[random % columnas, random / columnas - 1].GetComponent<IdCasilla>().ChangeId(2);
        if (random / columnas < filas-1) casillas[random % columnas, random / columnas + 1].GetComponent<IdCasilla>().ChangeId(2);


        // colocar muerto
        muerto.transform.position = new Vector3(random % columnas, random / columnas, 0);
        muerto.gameObject.SetActive(true);

        // colocar arma
        int rand, x, y;

        do
        {
            rand = Random.Range(0, 4);
            switch (rand)
            {
                case 0: // arriba izq
                    x = random % columnas - 1;
                    y = random / columnas + 1;
                    break;
                case 1: // arriba dcha
                    x = random % columnas + 1;
                    y = random / columnas + 1;
                    break;
                case 2: // abajo izq
                    x = random % columnas - 1;
                    y = random / columnas - 1;
                    break;
                default: // abajo dcha
                    x = random % columnas + 1;
                    y = random / columnas - 1;
                    break;
            }
        } while (x < 0 || x > columnas - 1 || y < 0 || y > filas - 1);
        arma.transform.position = new Vector3(x, y, 0);
        arma.gameObject.SetActive(true);
    }
    public void LimpiarTablero()
    {
        arma.gameObject.SetActive(false);
        muerto.gameObject.SetActive(false);
        for (int i = 0; i < columnas; i++)
        {
            for (int j = 0; j < filas; j++)
            {
                casillas[i, j].GetComponent<IdCasilla>().ChangeId(1);
            }
        }

    }
}
