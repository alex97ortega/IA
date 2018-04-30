
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public int filas, columnas;
    public GameObject casilla;
    public Camera cam;
    
    public GameObject[,] casillas;
    
    
    Vector3[,] positions;
    
    
    GameObject tablero;
    public GameObject[] alys;
    public GameObject[] enemies;

    public bool noche = false;
    bool comenzado = false;

    public int numeroAliados;
    public int numeroEnemigos;

    public int maxA;
    public int maxE;
    public GameObject cobete;
    public GameObject protas;
    public GameObject aliado;
    public GameObject enemigo;
    public GameObject botonComenzar;
    public GameObject textofin;
    public GUI gui;

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
        maxA = numeroAliados;
        maxE = numeroEnemigos;
        alys = new GameObject[maxA];
        enemies = new GameObject[maxE];
    }
    
	public void CambiarNoche()
    {
        noche = !noche;

        foreach (GameObject x in alys)
        {
            if (x != null)
            {
                if (noche) x.GetComponent<SpriteRenderer>().color = Color.grey;
                else x.GetComponent<SpriteRenderer>().color = Color.white;
            }
        }
        foreach (GameObject x in enemies)
        {
            if (x != null)
            {
                if (noche) x.GetComponent<SpriteRenderer>().color = Color.grey;
                else x.GetComponent<SpriteRenderer>().color = Color.white;
            }
        }
        for (int i = 0; i < columnas; i++)
        {
            for (int j = 0; j < filas; j++)
            {
                if (noche) casillas[i, j].GetComponent<SpriteRenderer>().color = Color.grey;
                else casillas[i, j].GetComponent<SpriteRenderer>().color = Color.white;
            }
        }
        if (noche)
        {
            protas.GetComponent<SpriteRenderer>().color = Color.grey;
            cobete.GetComponent<SpriteRenderer>().color = Color.grey;
            cam.backgroundColor = Color.black;
        }
        else
        {
            protas.GetComponent<SpriteRenderer>().color = Color.white;
            cobete.GetComponent<SpriteRenderer>().color = Color.white;
            cam.backgroundColor = Color.grey;
        }        
    }
    public void Comenzar()
    {
        comenzado = true;
        botonComenzar.SetActive(false);
        gui.Comenzar();
    }
    public void Reiniciar()
    {
        foreach (GameObject x in alys)
        {
            Destroy(x);
        }
        foreach (GameObject x in enemies)
        {
            Destroy(x);
        }

        numeroEnemigos = maxE;
        numeroAliados = maxA;
        alys = new GameObject[maxA];
        enemies = new GameObject[maxE];
        protas.transform.position = new Vector2(1,0);
        comenzado = false;
        gui.Reinciar();
        botonComenzar.SetActive(false);
        textofin.SetActive(false);
        protas.SetActive(true);
    }

    public void CreateAly(Vector3 pos)
    {
        if (!comenzado)
        {
            GameObject al = Instantiate(aliado, pos, Quaternion.identity);
            if (noche) al.GetComponent<SpriteRenderer>().color = Color.grey;
            al.gameObject.SetActive(true);
            alys[maxA - numeroAliados] = al;
            numeroAliados--;
        }
    }
    public void CreateEnemy(Vector3 pos)
    {
        if (!comenzado)
        {
            botonComenzar.SetActive(true);
            GameObject en = Instantiate(enemigo, pos, Quaternion.identity);
            if (noche) en.GetComponent<SpriteRenderer>().color = Color.grey;
            en.gameObject.SetActive(true);
            enemies[maxE - numeroEnemigos] = en;
            numeroEnemigos--;
        }
    }
    public bool Comenzado() { return comenzado; }
   
}
