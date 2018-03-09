﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    Vector3[,] positions = new Vector3[10,10];
    GameObject[,] casillas = new GameObject[10,10];
    GameObject tablero;
    public GameObject casilla;

    public GameObject tA;
    public GameObject tV;
    public GameObject tR;

    public GameObject xAzul;
    public GameObject xVerde;
    public GameObject xRojo;

    public Text sel;

    public bool nadaSeleccionado;
    GameObject tanqueSeleccionado;

    public Sprite arena;
    public Sprite barro;
    public Sprite roca;

    public Algoritmo a1;


    bool moviendoseAzul = false;
    bool moviendoseVerde = false;
    bool moviendoseRojo = false;
    // Use this for initialization
    void Start () {

        tablero = GameObject.Find("Tablero");

        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                positions[i, j] = new Vector3(i, j, 0);
                GameObject ficha = Instantiate(casilla, positions[i, j], Quaternion.identity);
                casillas[i, j] = ficha;
				//ficha.GetComponent<Collision2D>;
                // ficha.GetComponent<SpriteRenderer>().sprite = arena; // habrá que ponerlo aleatorio
                // ficha.GetComponent<Index>().index = (uint)indx;
                // ficha.GetComponent<BoardPosition>().boardPosition = new Vector2Int((int)j, (int)i);
                 ficha.transform.parent = tablero.transform;
                ficha.name = casillas[i, j].name;
            }
        }
        CreateGame();

    }
    
    
	// Update is called once per frame
	void Update () {

        // texto
		if(tanqueSeleccionado == tA)
        {
            sel.text = "Seleccionado tanque Azul";
            sel.color = Color.cyan;
        }
        else if (tanqueSeleccionado == tV)
        {
            sel.text = "Seleccionado tanque Verde";
            sel.color = Color.green;
        }
        else if (tanqueSeleccionado == tR)
        {
            sel.text = "Seleccionado tanque Rojo";
            sel.color = Color.red;
        }
        else
        {
            sel.text = "Ninguno seleccionado";
            sel.color = Color.white;
        }

        //detectar si hay que parar tanque

        if (moviendoseAzul)
        {

        }
        if (moviendoseVerde)
        {

        }
        if (moviendoseRojo)
        {

        }
    }
    public void CreateGame()
    {
        
        // crear posiciones aleatorias de los tanques
        int randomA, randomV, randomR;
        randomA = Random.Range(0, 99);
        do
        {
            randomV = Random.Range(0, 99);
        } while (randomA == randomV);

        do
        {
            randomR = Random.Range(0, 99);
        } while (randomA == randomR || randomV == randomR);

        tA.transform.position = new Vector3(randomA / 10, randomA % 10, 0);
        tV.transform.position = new Vector3(randomV / 10, randomV % 10, 0);
        tR.transform.position = new Vector3(randomR / 10, randomR % 10, 0);

       
        Limpiar();
    }
    public void Limpiar()
    {
        //poner el tablero a 0 
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                casillas[i, j].GetComponent<Index>().index = 1;
                casillas[i, j].GetComponent<SpriteRenderer>().sprite = arena;
            }
        }

        xAzul.gameObject.SetActive(false);
        xVerde.gameObject.SetActive(false);
        xRojo.gameObject.SetActive(false);


        SeleccionarVacio();
    }
    public void mapaAleatorio()
    {
        int rand;
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                if((tA.transform.position.x == i && tA.transform.position.y == j) ||
                    (tV.transform.position.x == i && tV.transform.position.y == j) ||
                    (tR.transform.position.x == i && tR.transform.position.y == j))
                {
                    casillas[i, j].GetComponent<Index>().index = 1;
                    casillas[i, j].GetComponent<SpriteRenderer>().sprite = arena;
                }
                else
                {
                    rand = Random.Range(0, 8);
                    switch(rand)
                    {
                        case 0:
                            casillas[i, j].GetComponent<Index>().index = 0;
                            casillas[i, j].GetComponent<SpriteRenderer>().sprite = roca;
                            break;
                        case 1:
                            casillas[i, j].GetComponent<Index>().index = 2;
                            casillas[i, j].GetComponent<SpriteRenderer>().sprite = barro;
                            break;
                        default:
                            casillas[i, j].GetComponent<Index>().index = 1;
                            casillas[i, j].GetComponent<SpriteRenderer>().sprite = arena;
                            break;
                    }
                }
            }
        }

        xAzul.gameObject.SetActive(false);
        xVerde.gameObject.SetActive(false);
        xRojo.gameObject.SetActive(false);


        SeleccionarVacio();
    }
    public void Seleccionar(GameObject g)
    {
        tanqueSeleccionado = g;
    }
    public void SeleccionarVacio()
    {
        nadaSeleccionado = true;
        tanqueSeleccionado = casilla;
    }

    public void PonerCruz(Vector3 pos)
    {
        if(tanqueSeleccionado == tA)
        {
           
                xAzul.gameObject.SetActive(true);
                xAzul.transform.position = pos;
                moviendoseAzul = true;
                //if(!a1.calcularRuta(tA, pos,casillas, tV.transform.position, tR.transform.position))
                //    Debug.Log("Ruta imposible");
            
        }
        else if (tanqueSeleccionado == tV)
        {
            
                xVerde.gameObject.SetActive(true);
                xVerde.transform.position = pos;
                moviendoseVerde = true;
                //if(!a1.calcularRuta(tV, pos, casillas, tA.transform.position, tR.transform.position))
                //    Debug.Log("Ruta imposible");
                      
        }
        else if (tanqueSeleccionado == tR)
        {
            
                xRojo.gameObject.SetActive(true);
                xRojo.transform.position = pos;
                moviendoseRojo = true;
                //if(!a1.calcularRuta(tR, pos, casillas, tV.transform.position, tA.transform.position))
                //    Debug.Log("Ruta imposible");
                       
        }
        SeleccionarVacio();
    }

	public void PararTanque(GameObject tq){
		Debug.Log ("parar");
	}
}
