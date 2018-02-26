using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManger : MonoBehaviour {

    Vector3[] positions = new Vector3[9];
    public GameObject fichaP; // prefab de la ficha
    public GameObject[] fichas = new GameObject[9];
    public Vector2[] state = new Vector2[9];
    public List<Sprite> fichasImg = new List<Sprite>(); // lista de las fichas del tablero
    GameObject conjuntoFichas; // almacena todas las fichas

    GameObject hueco;

    public GameObject texto;


    public uint movements;
    //tiempo, cuando sepamos


    private void Awake()
    {
        conjuntoFichas = GameObject.Find("Fichas");
    }
    private void Start()
    {
        //Inicializar los positions a las posiciones que tienen que tener las fichas con ese índice
        int indx = 0;
        float tamF = 3.6f;
        for (float i = 0; i < 3; i++)
        {
            for (float j = 0; j < 3; j++)
            {
                positions[indx] = new Vector3(j * tamF - 3, -i * tamF + 3, 0);
                GameObject ficha = Instantiate(fichaP, positions[indx], Quaternion.identity);
                fichas[indx] = ficha;
                ficha.GetComponent<SpriteRenderer>().sprite = fichasImg[indx];
                ficha.GetComponent<Index>().index = (uint)indx;
                ficha.transform.parent = conjuntoFichas.transform;
                ficha.name = fichasImg[indx].name;
                if (indx == 8) hueco = ficha;
                indx++;
            }
        }
        /////////////////
        for (float i = 0; i < 6; i++)
        {
            if (i < 3)
            {
                Vector2 pos = new Vector2(-i * tamF + 4, +8);
                GameObject ficha = Instantiate(fichaP, pos, Quaternion.identity);
            }
            else if (i < 6)
            {
                Vector2 pos = new Vector2(-i * tamF + 14.5f, -8);
                GameObject ficha = Instantiate(fichaP, pos, Quaternion.identity);
            }
        }

        for (float i = 0; i < 6; i++)
        {
            if (i < 3)
            {
                Vector2 pos = new Vector2(-7, -i * tamF + 3.5f);
                GameObject ficha = Instantiate(fichaP, pos, Quaternion.identity);
            }
            else
            {
                Vector2 pos = new Vector2(+8, -i * tamF + 13.5f);
                GameObject ficha = Instantiate(fichaP, pos, Quaternion.identity);
            }
        }

        hueco.gameObject.SetActive(false); //ficha escondida o hueco
        fichas = GameObject.FindGameObjectsWithTag("Ficha"); // hay que usar el tag ficha


        // ahora ponemos posiciones aleatorias

        int random;
        for (int i = 1; i < fichas.Length; i++)
        {
            random = Random.Range(i, 8);
            Vector2 newPos = fichas[i].transform.position;
            fichas[i].transform.position = fichas[random].transform.position;
            fichas[random].transform.position = newPos;
        }



    }

    public void MueveFicha(uint ficha)
    {
        //Esto no acaba de funcionar y no sé why
        SensoresScript sensoresScript = fichas[ficha].GetComponentInChildren(typeof(SensoresScript)) as SensoresScript;
        Vector3 newPos;
        if (!sensoresScript.ocupadoArr) newPos = positions[ficha - 3];
        else
        if (!sensoresScript.ocupadoAb) newPos = positions[ficha + 3];
        else
        if (!sensoresScript.ocupadoDr) newPos = positions[ficha + 1];
        else
        if (!sensoresScript.ocupadoIzq) newPos = positions[ficha - 1];
        else return;
        fichas[ficha].transform.position = newPos;
        movements++;
    }

    public void Victoria()
    {
        //comprobar si se ha ganado
        for (int i = 0; i < positions.Length; i++)
        {
            if (positions[i] != fichas[i].transform.position)
            {
                return;
            }
        }

        hueco.gameObject.SetActive(true); //ficha escondida o hueco
        texto.gameObject.SetActive(true); //texto escondido o hueco
    }

}
