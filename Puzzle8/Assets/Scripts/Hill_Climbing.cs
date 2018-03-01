using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hill_Climbing : MonoBehaviour
{

    GameManger gameManager;

    public GameObject[] fichas;

    int[,] solvedState = new int[3, 3];
    struct Node
    {
        public int[,] state;
        public int fichaMovida;
        public Vector2Int huecoPos;

        public Node(int[,] state_)
        {
            state = state_;
            fichaMovida = -1;
            huecoPos = new Vector2Int(-1, -1);
            //Localizamos el hueco
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    if (state[i, j] == -1)
                        huecoPos = new Vector2Int(i, j);
        }
    }

    List<Node> list = new List<Node>();
    List<int[,]> seen = new List<int[,]>();

    Vector2Int[] directions;


    private void Start()
    {
        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
                solvedState[i, j] = i + j;

        fichas = GameObject.FindGameObjectsWithTag("Ficha"); // hay que usar el tag ficha
        gameManager = GameObject.Find("Scripts").GetComponent(typeof(GameManger)) as GameManger;

        directions = new Vector2Int[4];
        directions[0] = new Vector2Int(-1, 0);
        directions[1] = new Vector2Int(1, 0);
        directions[2] = new Vector2Int(0, -1);
        directions[3] = new Vector2Int(0, 1);
    }

    public void Resolve()
    { 
        list.Add(new Node(TranslateCurrentState(fichas)));
        Debug.Log("Tablero Original:");
        PrintStado(list[0].state);

        int cont = 0;
        bool nuevoEstado;
        while (list.Count > 0 && !IsSolution(list[0].state))
        {
            Debug.Log("Vuelta :" + cont);
            nuevoEstado = false;
            //Probar todos los movimientos posibles
            for (int i = 0; i < 4; i++)
            {
                if (!nuevoEstado)
                {
                    if (IsValidMove(list[0], directions[i])) // is valid es correcto
                    {
                       
                        Node newNode = MueveFicha(list[0], directions[i]);
                        // Debug.Log("El hueco está en :" + newNode.huecoPos);
                        //Debug.Log("Muevo la ficha: "+ (newNode.fichaMovida + 1));
                        //eliminar los estados por los que ya hemos pasado ( !seen.Constains(state)  )
                        bool contenido = seen.Contains(newNode.state);
                        if (!contenido)
                        {
                            //meter los nuevos estados en la lista de estados
                            Debug.Log("Estado Nuevo");
                            list.Add(newNode);
                            PrintStado(newNode.state);
                            Debug.Log("Tamaño de la lista: " + list.Count);
                            nuevoEstado = true;
                        }
                        else Debug.Log("Estado ya visto"); // en la segunda vuelta solo entra aqui
                    }
                }
                
            }
            //quitar list[0] y meterlo en seen
            seen.Add(list[0].state);
            list.Remove(list[0]);
            //ordenar la lista de acuerdo a la heurística
            Debug.Log("Tamaño de la lista final: " + list.Count);
            SortByPotential(ref list);
            cont++;
        }//Solver Algorithm


        // List<int> solution = new List<int>();
        //Debug.Log("He intentado movir las fichas:");
        foreach (Node n in list)
        {
            //solution.Add(n.fichaMovida);
            Debug.Log(n.fichaMovida);
            gameManager.MueveFicha((uint)n.fichaMovida);

        }
        //return solution;
    }

    //Done
    bool IsSolution(int[,] state)
    {
        bool ret = true;
        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
                if (state[i, j] != solvedState[i, j])
                    ret = false;
        return ret;
    }

    //Done
    int[,] TranslateCurrentState(GameObject[] fichas)
    {
        //Inicializamos el estado a -1
        int[,] currentState = new int[3, 3];
        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
                currentState[i, j] = -1;

        //rellenamos el estado con los índices de las fichas
        for(int i = 1; i < fichas.Length; i++)
        {
            Vector2Int pos = fichas[i].GetComponent<BoardPosition>().boardPosition;
            int index = (int)fichas[i].GetComponent<Index>().GetIndex();
            currentState[pos.y, pos.x] = index;
            //Debug.Log("La ficha " + index + " está en la posicion: " + pos);
        }



        //Devolvemos el estado formateado
       /* Debug.Log("Tablero traducido a:\n"
            + currentState[0, 0] + " " + currentState[0, 1] + " " + currentState[0, 2] + "\n"
            + currentState[1, 0] + " " + currentState[1, 1] + " " + currentState[1, 2] + "\n" 
            + currentState[2, 0] + " " + currentState[2, 1] + "  " + currentState[2, 2]);*/
        return currentState;
    }

    Node MueveFicha(Node oldState, Vector2Int dir)
    {
        int[,] newState = oldState.state;

        Vector2Int newHuecoPos = oldState.huecoPos + dir;
        Node newNode = new Node
        {
            fichaMovida = oldState.state[newHuecoPos.x, newHuecoPos.y]
        };
        newState[oldState.huecoPos.x, oldState.huecoPos.y] = oldState.state[newHuecoPos.x, newHuecoPos.y];
        newState[newHuecoPos.x, newHuecoPos.y] = -1;

        newNode.huecoPos = newHuecoPos;
        newNode.state = newState;

        return newNode;
    }
    bool IsValidMove(Node n, Vector2Int dir)
    {
        Vector2Int newPos = n.huecoPos + dir;
        bool resultado = !(newPos.x > 2 || newPos.x < 0 || newPos.y > 2 || newPos.y < 0);
        return resultado;
    }


    struct WeightedNode { public int weigth; public Node node; }
    void SortByPotential(ref List<Node> list)
    {
        List<WeightedNode> wList = new List<WeightedNode>();
        foreach (Node n in list)
        {
            WeightedNode weightedNode;
            weightedNode.node = n;
            weightedNode.weigth = GetWeight(n);
            wList.Add(weightedNode);
        }
        //Sort wList
        wList.Sort((x, y) => x.weigth.CompareTo(y.weigth));
        //Copy WList.node -> list
        for (int i = 0; i < list.Count; i++)
        {
            list[i] = wList[i].node;
        }
    }

    int GetWeight(Node node)
    {
        //Contar la distancia entre la pieza movida y su posición correcta
        //Encontrar la posicion de la ficha movida
        bool found = false;
        int i = -1; int j = -1;
        for (i = 0; !found && i < 3; i++)
            for (j = 0; !found && j < 3; j++)
                found = node.state[i, j] == node.fichaMovida;

        Vector2Int fichaMovidaPos = new Vector2Int(i, j);

        Vector2Int posDestino = new Vector2Int
        {
            x = node.fichaMovida / 3,
            y = node.fichaMovida - (node.fichaMovida / 3)
        };

        //calcular la distancia entre la ficha movida y su lugar
        int distancia = (fichaMovidaPos.x - posDestino.x) + (fichaMovidaPos.y - posDestino.y); ;
        return distancia;
    }

    static void PrintStado(int[,] state)
    {
        Debug.Log("Tablero:\n"
           + state[0, 0] + " " + state[0, 1] + " " + state[0, 2] + "\n"
           + state[1, 0] + " " + state[1, 1] + " " + state[1, 2] + "\n"
           + state[2, 0] + " " + state[2, 1] + "  " + state[2, 2]);
    }
    static void Swap<T>(ref T lhs, ref T rhs)
    {
        T temp;
        temp = lhs;
        lhs = rhs;
        rhs = temp;
    }

}