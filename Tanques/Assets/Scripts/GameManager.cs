using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    Vector3[,] positions = new Vector3[10,10];
    GameObject[,] casillas = new GameObject[10,10];
    GameObject tablero;
    public GameObject casilla;
    public GameObject tA;
    public GameObject tV;
    public GameObject tR;

    public Sprite arena;
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

        //poner el tablero a 0 
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                casillas[i, j].GetComponent<Index>().index = 1;
                casillas[i, j].GetComponent<SpriteRenderer>().sprite = arena;
            }
        }
    }
}
