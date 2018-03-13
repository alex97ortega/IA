using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManger : MonoBehaviour {

    Vector3[] positions = new Vector3[9];
    public GameObject fichaP; // prefab de la ficha
    public GameObject bordeP; // prefab de bordes
    public GameObject[] fichas = new GameObject[9];
    public GameObject[] bordes = new GameObject[12];
    public Vector2[] state = new Vector2[9];
    public List<Sprite> fichasImg = new List<Sprite>(); // lista de las fichas del tablero

    public GameObject botonReinicio;
    public GameObject botonResolver;


    GameObject conjuntoFichas; // almacena todas las fichas
    GameObject conjuntoBordes; // almacena los bordes
    GameObject hueco;

    UI ui;
    float tamF = 3.6f;
    public GameObject texto;
    public GameObject stats;
    public GameObject movs;


    public uint movements;
    //tiempo, cuando sepamos

    private void Awake()
    {
        conjuntoFichas = GameObject.Find("Fichas");
        conjuntoBordes = GameObject.Find("Bordes");
        ui = GameObject.Find("Scripts").GetComponent(typeof(UI)) as UI;
        //Inicializar los positions a las posiciones que tienen que tener las fichas con ese índice
        int indx = 0;
        
        for (float i = 0; i < 3; i++)
        {
            for (float j = 0; j < 3; j++)
            {
                positions[indx] = new Vector3(j * tamF - 3, -i * tamF + 3, 0);
                GameObject ficha = Instantiate(fichaP, positions[indx], Quaternion.identity);
                fichas[indx] = ficha;
                ficha.GetComponent<SpriteRenderer>().sprite = fichasImg[indx];
                ficha.GetComponent<Index>().index = (uint)indx;
                ficha.GetComponent<BoardPosition>().boardPosition = new Vector2Int((int) j, (int) i);
                ficha.transform.parent = conjuntoFichas.transform;
                ficha.name = fichasImg[indx].name;
                if (indx == 8) hueco = ficha;
                indx++;
            }
        }
        /////////////////
        Vector2 pos;
        for (float i = 0; i < 6; i++)
        {
            if (i < 3)
            {
                 pos = new Vector2(-i * tamF + 4, +8);                
            }
            else 
            {
                 pos = new Vector2(-i * tamF + 14.5f, -8);              
            }
            GameObject borde = Instantiate(bordeP, pos, Quaternion.identity);
            borde.transform.parent = conjuntoBordes.transform;
            //borde.GetComponent<SpriteRenderer>().sprite = fichasImg[0];
        }

        for (float i = 0; i < 6; i++)
        {
            if (i < 3)
            {
                 pos = new Vector2(-7, -i * tamF + 3.5f);                
            }
            else
            {
                 pos = new Vector2(+8, -i * tamF + 13.5f);
               
            }
            GameObject borde = Instantiate(bordeP, pos, Quaternion.identity);
            borde.transform.parent = conjuntoBordes.transform;
            //borde.GetComponent<SpriteRenderer>().sprite = fichasImg[0];
        }

        hueco.gameObject.SetActive(false); //ficha escondida o hueco
        fichas = GameObject.FindGameObjectsWithTag("Ficha"); // hay que usar el tag ficha
        bordes = GameObject.FindGameObjectsWithTag("Borde");


        Baraja();
        
    }
    public void MueveFicha(uint ficha)
    {
        ficha++;
        SensoresScript sensoresScript = fichas[ficha].GetComponentInChildren(typeof(SensoresScript)) as SensoresScript;
        Vector3 newPos;

        if (!sensoresScript.ocupadoArr)
        {
            newPos = fichas[ficha].transform.position + new Vector3(0, 3.6f, 0);
            fichas[ficha].GetComponent<BoardPosition>().MoveFicha(0, -1);
        }
        else if (!sensoresScript.ocupadoAb)
        {
            newPos = fichas[ficha].transform.position + new Vector3(0, -3.6f, 0);
            fichas[ficha].GetComponent<BoardPosition>().MoveFicha(0, 1);
        }
        else if (!sensoresScript.ocupadoDr)
        {
            newPos = fichas[ficha].transform.position + new Vector3(3.6f, 0, 0);
            fichas[ficha].GetComponent<BoardPosition>().MoveFicha(1, 0);
        }
        else if (!sensoresScript.ocupadoIzq)
        {
            newPos = fichas[ficha].transform.position + new Vector3(-3.6f, 0, 0);
            fichas[ficha].GetComponent<BoardPosition>().MoveFicha(-1, 0);
        }
        else return;
       
        
        fichas[ficha].transform.position = newPos;
        movements++;
        ui.SumaMov();
        Victoria();
    }

    
    public void Victoria()
    {
        //comprobar si se ha ganado
        for (int i = 0; i < fichas.Length - 1; i++)
        {
           
            if (positions[i] != fichas[i+1].transform.position)
            {
                return;//si no ha ganado la partida el método termina aquí
            }
        }

        hueco.gameObject.SetActive(true); //ficha escondida o hueco

        ui.Estadisticas();

        botonResolver.gameObject.SetActive(false);

        texto.gameObject.SetActive(true); 
        stats.gameObject.SetActive(true);
        movs.gameObject.SetActive(false);
    }
    public void Baraja()
    {
        movements = 0;
        ui.Reiniciando();
        // la magia
        for (int i = 0; i < fichas.Length - 1; i++)
        {
            fichas[i + 1].transform.position = positions[i];
        }

        // ahora ponemos posiciones aleatorias
        int random;
        for (int i = 1; i < fichas.Length; i++)
        {            
            random = Random.Range(i, 8);
            Vector2 newPos = fichas[i].transform.position;
            fichas[i].transform.position = fichas[random].transform.position;
            fichas[random].transform.position = newPos;

            Vector2Int aux = fichas[i].GetComponent<BoardPosition>().boardPosition;
            fichas[i].GetComponent<BoardPosition>().boardPosition = fichas[random].GetComponent<BoardPosition>().boardPosition;
            fichas[random].GetComponent<BoardPosition>().boardPosition = aux;
        }
        // Activar y desactivar todos los gameObjects que se necesitan

        hueco.gameObject.SetActive(false); //ficha escondida o hueco
        stats.gameObject.SetActive(false);
        movs.gameObject.SetActive(true);
        texto.gameObject.SetActive(false);
        botonResolver.gameObject.SetActive(true);
    }
    
}
