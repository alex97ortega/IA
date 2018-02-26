using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateGame : MonoBehaviour {

    //public List<Sprite> fichasImg = new List<Sprite>(); // lista de las fichas del tablero
    //public Sprite huecoImg; // casilla del tablero que va a estar vacía
    //public GameObject texto;
   // public GameObject fichaP; // prefab de la ficha

    //GameObject[] fichas;

    //Vector3[] posicionesCorrectas = new Vector3[9]; //para comparar con las posiciones actuales
    //const int tam = 3; // tamaño del cuadrado
    //GameObject hueco;
    //GameObject conjuntoFichas; // almacena todas las fichas
    

    //private void Awake() {
    //    conjuntoFichas = GameObject.Find("Fichas");
    //}
    // Use this for initialization
    void Start () {
        //int cont = 0;//LA PUTA VARIABLE MÁS IMPORTANTE!!!!
        //float n = 3.6f;
        // bucle que genera el tablero
        //for (float i = 0; i < tam; i++)
        //{
        //    for (float j = 0; j < tam; j++)
        //    {
        //        Vector2 pos = new Vector2(j*n-3, -i*n+3);
        //        GameObject ficha = Instantiate(fichaP, pos, Quaternion.identity);
        //        ficha.GetComponent<SpriteRenderer>().sprite = fichasImg[cont];
        //        ficha.GetComponent<Index>().index = (uint)cont;
        //        ficha.transform.parent = conjuntoFichas.transform;
        //        ficha.name = fichasImg[cont].name;
        //        if (cont == 8) hueco = ficha;
        //        cont++;
        //    }
        //}

        //// fichas bordes invisibles, CUTRÍSIMO
        //for (float i = 0; i < 6; i++)
        //{
        //    if (i < 3)
        //    {
        //        Vector2 pos = new Vector2(-i*n + 4,  + 8);
        //        GameObject ficha = Instantiate(fichaP, pos, Quaternion.identity);
        //    }
        //    else if (i < 6)
        //    {
        //        Vector2 pos = new Vector2(-i*n + 14.5f, -8);
        //        GameObject ficha = Instantiate(fichaP, pos, Quaternion.identity);
        //    }
        //}

        //for (float i = 0; i < 6; i++)
        //{
        //    if (i < 3)
        //    {
        //        Vector2 pos = new Vector2(-7, -i*n+3.5f);
        //        GameObject ficha = Instantiate(fichaP, pos, Quaternion.identity);
        //    }
        //    else
        //    {
        //        Vector2 pos = new Vector2(+8, -i * n + 13.5f);
        //        GameObject ficha = Instantiate(fichaP, pos, Quaternion.identity);
        //    }
        //}

        //hueco.gameObject.SetActive(false); //ficha escondida o hueco
        //fichas = GameObject.FindGameObjectsWithTag("Ficha"); // hay que usar el tag ficha
        //for (int x = 0; x < posicionesCorrectas.Length; x++)
        //    posicionesCorrectas[x] = (fichas[x].transform.position); //asignar posiciones


        //// ahora ponemos posiciones aleatorias

        // int random;
        // for (int i = 1; i < fichas.Length; i++)
        // {
        //     random = Random.Range(i, tam*tam -1);
        //     Vector2 newPos = fichas[i].transform.position;
        //     fichas[i].transform.position = fichas[random].transform.position;
        //     fichas[random].transform.position = newPos;
        // }
    }

    //public void Victoria () {
    //    //comprobar si se ha ganado
    //    for (int i = 0; i < posicionesCorrectas.Length; i++)
    //    {
    //        if (posicionesCorrectas[i] != fichas[i].transform.position)
    //        {
    //            return;
    //        }
    //    }

    //    hueco.gameObject.SetActive(true); //ficha escondida o hueco
    //    texto.gameObject.SetActive(true); //texto escondido o hueco
    //}
}
