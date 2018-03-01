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
        public int[,] state;//almacena el tablero
        public int fichaMovida;//el índice de la pieza que seha movido en esa jugada
        public Vector2Int huecoPos;//la posición del hueco en esa jugada

        //La constructora simplemente busca el hueco y copia el stado por parámetro
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
    //lista de los estados candidatos
    List<Node> list = new List<Node>();
    //lista de estados por los que ya hemos pasado
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
    { //Metemos a la lista de candidatos el estado actual del juego
        list.Add(new Node(TranslateCurrentState(fichas)));
        //PrintStado(list[0].state, "Tablero Original:");

        int cont = 1;//Debug
        //Mientras no encontremos la solucion, seguir buscando
        while (list.Count > 0 && !IsSolution(list[0].state))
        {
            Node currentState = list[0];
            Debug.Log("--------------------------------");
            Debug.Log("Vuelta: " + cont + " el tamaño de la lista es:" + list.Count);
            //Probar todos los movimientos posibles
            foreach (Vector2Int dir in directions)
            {
                if (IsValidMove(currentState, dir)) // is valid es correcto
                {
                    PrintStado(currentState.state, "Estado original");
                    //El problema esta en el mueve ficha, el estado se le pasa correctamente
                    //Pero lo devuelve mal, como si recordase el tabelro anterior dentro del método
                    Node newNode = MueveFicha(currentState, dir);
                    // Debug.Log("El hueco está en :" + newNode.huecoPos);
                    //Debug.Log("Muevo la ficha: "+ (newNode.fichaMovida + 1));
                    //descartar los estados por los que ya hemos pasado ( !seen.Constains(state)  )
                    PrintStado(newNode.state, "Estado tras mover Ficha");
                    bool contenido = seen.Contains(newNode.state);
                    if (!contenido)
                    {
                        //meter los nuevos estados en la lista de estados
                       // Debug.Log("Estado nuevo");
                        list.Add(newNode);
                    }
                   // else Debug.Log("Estado ya visto"); // en la segunda vuelta solo entra aqui
                }
            }
            //quitar list[0] y meterlo en seen
            seen.Add(currentState.state);
            list.Remove(currentState);
            //ordenar la lista de acuerdo a la heurística
          //  Debug.Log("Tamaño de la lista final: " + list.Count);
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

    //Comprueba si el estado que se le pasa es el resuelto
    bool IsSolution(int[,] state)
    {
        bool ret = true;
        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
                if (state[i, j] != solvedState[i, j])
                    ret = false;
        return ret;
    }

    //Taduce el array de GameObjects donde están las fichas en un estado de int[,]
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
        return currentState;
    }

    //Devuelve el nuevo estado resultado de mover la ficha en la dirección que se le pasa
    Node MueveFicha(Node oldState, Vector2Int dir)
    {
        //copiamos el estado del que venimos
        PrintStado(oldState.state, "Old state recibido en Mueve Ficha");
        int[,] newState = oldState.state;
        //calculamos la posición del huevo en el nuevo estado
        Vector2Int newHuecoPos = oldState.huecoPos + dir;
        //Creamos un nuevo Nodo, inicializando la ficha movida al valor que tenía 
        //la ficha del estado anterior en la posición en la que ahora está el hueco
        //porque se han intercambiado
        Node newNode = new Node
        {
            fichaMovida = oldState.state[newHuecoPos.x, newHuecoPos.y]
        };
        //Ponemos en la posicion del hueco antiguo el valor de la casilla que ahora va a ser el hueco
        Debug.Log("He movido la ficha: " + newNode.fichaMovida);
        newState[oldState.huecoPos.x, oldState.huecoPos.y] = oldState.state[newHuecoPos.x, newHuecoPos.y];
        //Ponemos el hueco a -1 para representarlo
        Debug.Log(message: "Valor del nuevo hueco antes de ponerlo a -1 " +newState[newHuecoPos.x, newHuecoPos.y]);
        newState[newHuecoPos.x, newHuecoPos.y] = -1;
        //Inicializamos los valores del nuevo Node con la información calculada
        newNode.huecoPos = newHuecoPos;
        newNode.state = newState;

        return newNode;
    }
    bool IsValidMove(Node n, Vector2Int dir)
    {
        //Calculamos la posición del hueco si se moviese en la direccion que se pasa por parámetro
        //devolvemos si la posición resultante es válida
        Vector2Int newPos = n.huecoPos + dir;
        bool resultado = !(newPos.x > 2 || newPos.x < 0 || newPos.y > 2 || newPos.y < 0);
        return resultado;
    }

    //Estructura para poder darle un valor a cada nodo
    struct WeightedNode { public int weigth; public Node node; }
    //Reordenamos la lista de candidatos para que el más prometedor sea el primero
    void SortByPotential(ref List<Node> list)
    {
        //Lista con los penos
        List<WeightedNode> wList = new List<WeightedNode>();
        foreach (Node n in list)
        {
            WeightedNode weightedNode;
            weightedNode.node = n;
            weightedNode.weigth = GetWeight(n);
            wList.Add(weightedNode);
        }
        //Sort wList
        //de menor a mayor (<)
        wList.Sort((x, y) => {
            if (x.weigth >= y.weigth)
                return -1;
            else
                return 1;
            });
        //Copy WList.node -> list
        for (int i = 0; i < list.Count; i++)
        {
            list[i] = wList[i].node;
        }
    }
    //Devuelve la distancia de la última ficha movida en ese estado 
    //hacia la que debería ser su posición
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
    //Esto simplemente imprime el tablero en la consola debug de Unity
    static void PrintStado(int[,] state, string text)
    {
        Debug.Log(text + "\n"
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