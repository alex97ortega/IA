using System.Collections;
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

    public Camera cam;

    public GameObject[,] casillas;
    Vector3[,] positions;

    GameObject tablero;

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

        for(int i = 0; i < numHuecos; i++)
        {
            do
            {
                pos = Random.Range(1, filas * columnas);
            } while (pos == posCadaver || pos == posArma);

            casillas[pos % columnas, pos / columnas].GetComponent<IdCasilla>().ChangeId(IdCasilla.Tipo.hueco);
            IdCasilla.Tipo aux;
            //colocar barro
            if (pos % columnas > 0 && casillas[pos % columnas - 1, pos / columnas].GetComponent<IdCasilla>().GetTipo() != IdCasilla.Tipo.hueco)
            {
                if (casillas[pos % columnas - 1, pos / columnas].GetComponent<IdCasilla>().GetTipo() == IdCasilla.Tipo.sangre)
                    aux = IdCasilla.Tipo.barroSangre;
                else aux = IdCasilla.Tipo.barro;
                casillas[pos % columnas - 1, pos / columnas].GetComponent<IdCasilla>().ChangeId(aux);
            }

            if (pos % columnas < columnas - 1 && casillas[pos % columnas + 1, pos / columnas].GetComponent<IdCasilla>().GetTipo() != IdCasilla.Tipo.hueco)
            {
                if (casillas[pos % columnas + 1, pos / columnas].GetComponent<IdCasilla>().GetTipo() == IdCasilla.Tipo.sangre)
                    aux = IdCasilla.Tipo.barroSangre;
                else aux = IdCasilla.Tipo.barro;
                casillas[pos % columnas + 1, pos / columnas].GetComponent<IdCasilla>().ChangeId(aux);
            }
            if (pos / columnas > 0 && casillas[pos % columnas, pos / columnas-1].GetComponent<IdCasilla>().GetTipo() != IdCasilla.Tipo.hueco)
            {
                if (casillas[pos % columnas, pos / columnas - 1].GetComponent<IdCasilla>().GetTipo() == IdCasilla.Tipo.sangre)
                    aux = IdCasilla.Tipo.barroSangre;
                else aux = IdCasilla.Tipo.barro;
                casillas[pos % columnas, pos / columnas - 1].GetComponent<IdCasilla>().ChangeId(aux);
            }
            if (pos / columnas < filas - 1 && casillas[pos % columnas, pos / columnas+1].GetComponent<IdCasilla>().GetTipo() != IdCasilla.Tipo.hueco)
            {
                if (casillas[pos % columnas, pos / columnas + 1].GetComponent<IdCasilla>().GetTipo() == IdCasilla.Tipo.sangre)
                    aux = IdCasilla.Tipo.barroSangre;
                else aux = IdCasilla.Tipo.barro;
                casillas[pos % columnas, pos / columnas + 1].GetComponent<IdCasilla>().ChangeId(aux);
            }
        }
    }

    public void Patrullar()
    {
        detective.gameObject.SetActive(true);
        detective.transform.position = new Vector3(0, 0, 0);
        detective.GetComponent<Patrulla>().Patrullar();
    }
    public void Muere()
    {
        detective.gameObject.SetActive(false);
    }
}
