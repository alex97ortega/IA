﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {


    public int filas, columnas;
    public int maxHuecos;
    public GameObject casilla;
    public GameObject arma;
    public GameObject muerto;
    public GameObject detective;

    public GameObject boton;
    public GameObject botonVel;

    public Camera cam;

    public GameObject[,] casillas;
    Vector3[,] positions;

    GameObject tablero;
    bool muere = true;
    bool siniestro = false;
    // Use this for initialization
    void Start () {
        tablero = GameObject.Find("Tablero");
        casillas = new GameObject[columnas, filas];
        positions = new Vector3[columnas, filas];

        if (filas * columnas <= 50) cam.orthographicSize = 3;
        else
        {
            cam.orthographicSize = 3 + ((filas * columnas - 50) / 20);
        }
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
        muere = true;
        LimpiarTablero();

        boton.gameObject.SetActive(true);

        int random;
        random = Random.Range(1, filas*columnas); // casilla 0,0 reservada para la casa
      

        //colocar sangre
        casillas[random % columnas, random / columnas].GetComponent<IdCasilla>().ChangeId(IdCasilla.Tipo.sangre);
        if (random % columnas > 0) casillas[random % columnas - 1, random / columnas].GetComponent<IdCasilla>().ChangeId(IdCasilla.Tipo.sangre);
        if (random % columnas < columnas-1) casillas[random % columnas + 1, random / columnas].GetComponent<IdCasilla>().ChangeId(IdCasilla.Tipo.sangre);
        if (random / columnas > 0) casillas[random % columnas, random / columnas - 1].GetComponent<IdCasilla>().ChangeId(IdCasilla.Tipo.sangre);
        if (random / columnas < filas-1) casillas[random % columnas, random / columnas + 1].GetComponent<IdCasilla>().ChangeId(IdCasilla.Tipo.sangre);


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

        detective.GetComponent<Patrulla>().CreaCamino();
        siniestro = !siniestro;
        Siniestro();
        CrearHuecos(random, x + y*columnas);
    }

    public void LimpiarTablero()
    {
        arma.gameObject.SetActive(false);
        muerto.gameObject.SetActive(false);
        boton.gameObject.SetActive(false);
        detective.gameObject.SetActive(false);

        for (int i = 0; i < columnas; i++)
        {
            for (int j = 0; j < filas; j++)
            {
                casillas[i, j].GetComponent<IdCasilla>().ChangeId(IdCasilla.Tipo.normal);
            }
        }
    }
    public void CrearHuecos(int posCadaver, int posArma)
    {
        int numHuecos = Random.Range(1, maxHuecos+1);

        int pos;

        for (int i = 0; i < numHuecos; i++)
        {
            do
            {
                pos = Random.Range(1, filas * columnas);
            } while (pos == posCadaver || pos == posArma);

            ColocaHueco(pos % columnas, pos / columnas);
        }
    }
    public void ColocaHueco(int x, int y)
    {
        if((x != muerto.transform.position.x || y != muerto.transform.position.y) && (x != arma.transform.position.x || y != arma.transform.position.y) && (x!=0 || y!=0))
        {
            casillas[x, y].GetComponent<IdCasilla>().ChangeId(IdCasilla.Tipo.hueco);
            IdCasilla.Tipo aux;
            //colocar barro
            if (x > 0 && casillas[x - 1, y].GetComponent<IdCasilla>().GetTipo() != IdCasilla.Tipo.hueco)
            {
                if (casillas[x - 1, y].GetComponent<IdCasilla>().GetTipo() == IdCasilla.Tipo.sangre || casillas[x - 1, y].GetComponent<IdCasilla>().GetTipo() == IdCasilla.Tipo.barroSangre)
                    aux = IdCasilla.Tipo.barroSangre;
                else aux = IdCasilla.Tipo.barro;
                casillas[x - 1, y].GetComponent<IdCasilla>().ChangeId(aux);
            }

            if (x < columnas - 1 && casillas[x + 1, y].GetComponent<IdCasilla>().GetTipo() != IdCasilla.Tipo.hueco)
            {
                if (casillas[x + 1, y].GetComponent<IdCasilla>().GetTipo() == IdCasilla.Tipo.sangre || casillas[x + 1, y].GetComponent<IdCasilla>().GetTipo() == IdCasilla.Tipo.barroSangre)
                    aux = IdCasilla.Tipo.barroSangre;
                else aux = IdCasilla.Tipo.barro;
                casillas[x + 1, y].GetComponent<IdCasilla>().ChangeId(aux);
            }
            if (y > 0 && casillas[x, y - 1].GetComponent<IdCasilla>().GetTipo() != IdCasilla.Tipo.hueco)
            {
                if (casillas[x, y - 1].GetComponent<IdCasilla>().GetTipo() == IdCasilla.Tipo.sangre || casillas[x , y-1].GetComponent<IdCasilla>().GetTipo() == IdCasilla.Tipo.barroSangre)
                    aux = IdCasilla.Tipo.barroSangre;
                else aux = IdCasilla.Tipo.barro;
                casillas[x, y - 1].GetComponent<IdCasilla>().ChangeId(aux);
            }
            if (y < filas - 1 && casillas[x, y + 1].GetComponent<IdCasilla>().GetTipo() != IdCasilla.Tipo.hueco)
            {
                if (casillas[x, y + 1].GetComponent<IdCasilla>().GetTipo() == IdCasilla.Tipo.sangre || casillas[x, y +1].GetComponent<IdCasilla>().GetTipo() == IdCasilla.Tipo.barroSangre)
                    aux = IdCasilla.Tipo.barroSangre;
                else aux = IdCasilla.Tipo.barro;
                casillas[x, y + 1].GetComponent<IdCasilla>().ChangeId(aux);
            }
        }
    }
    public void Patrullar()
    {
        if (muere)
        {
            botonVel.gameObject.SetActive(true);
            detective.gameObject.SetActive(true);
            detective.transform.position = new Vector3(0, 0, 0);
            detective.GetComponent<Patrulla>().Patrullar();
            muere = false;
        }               
    }
    public void Muere()
    {
        detective.gameObject.SetActive(false);
        muere = true;
    }
    public void Siniestro()
    {
        siniestro = !siniestro;

        for (int i = 0; i < columnas; i++)
            {
                for (int j = 0; j < filas; j++)
                {
                    if(siniestro) casillas[i, j].GetComponent<SpriteRenderer>().color = Color.black;
                    else casillas[i, j].GetComponent<SpriteRenderer>().color = Color.white;
                }
            }
        if (siniestro)
        {
            arma.GetComponent<SpriteRenderer>().color = Color.black;
            muerto.GetComponent<SpriteRenderer>().color = Color.black;
            detective.GetComponent<Patrulla>().Noche();
        }
        else
        {
            arma.GetComponent<SpriteRenderer>().color = Color.white;
            muerto.GetComponent<SpriteRenderer>().color = Color.white;
        }           
    }
    public void Salir()
    {
        Debug.Log("Saliendo");
        Application.Quit();
    }
}
