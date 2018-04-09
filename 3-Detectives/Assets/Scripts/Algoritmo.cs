﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// algoritmo A* (el mismo que el de la práctica de los tanques)
// al algoritmo le llega la posición actual del detective y la posición origen (siempre va a ser su casa: la 0,0)
// y el tablero de casillas descubiertas
// el algoritmo se encargará de buscar el camino más corto desde su posición hasta su casa,
// de entre las casillas que haya descubiertas
public class Algoritmo : MonoBehaviour {
    int[,] tablero;

    Vector2Int start;
    Vector2Int goal;

    const int worldWidth = 10;
    const int worldHeight = 10;
    const int worldSize = 100;

    class Node
    {
        public Node(Node parent_, Vector2Int point)
        {
            parent = parent_;
            x = point.x;
            y = point.y;
            indice = point.x + (point.y * worldWidth);
            f = 0;
            g = 0;
        }

        //public 
        //referencia al padre
        public Node parent;
        //indice en el array de marcados
        public int indice;
        //posición en el tablero
        public int x;
        public int y;
        //coste estimado del nodo al final
        public int f;
        //coste real hasta ahora
        public int g;

    }

    public List<Vector2Int> CalcularRuta(Vector2Int actual, int[,] estadoTablero)
    {
        //Get the current state into the local format
        start = new Vector2Int(actual.x,actual.y);
        goal = new Vector2Int(0,0);
        tablero = TraduceTablero(estadoTablero);

        // PrintTablero(tablero); 

        //INIT LOCAL VARIABLES
        Node myPathStart = new Node(null, new Vector2Int(start.x, start.y));
        Node myPathEnd = new Node(null, new Vector2Int(goal.x, goal.y));
        //Constains all the world cells
        bool[] AStar = new bool[worldSize];
        //list of currently open Nodes
        List<Node> open = new List<Node> { myPathStart };//cola de prioridad
        //list of closed Nodes
        List<Node> closed = new List<Node>();
        //list of the final output array
        List<Vector2Int> result = new List<Vector2Int>();
        //reference to Nodes pos (that is nearby)
        List<Vector2Int> myNeightbours = new List<Vector2Int>();
        //reference to a Node (that we are considering now)
        Node myNode;
        //reference to a Node (that starts a path in question)
        Node myPath;
        //temp variables
        int lenght, max, min, i, j;

        //Starts the algorithm!
        while ((lenght = open.Count) != 0)
        {
            max = worldSize;
            min = -1;

            for (i = 0; i < lenght; i++)
            {
                if (open[i].f < max)
                {
                    max = open[i].f;
                    min = i;
                }
            }
            //grab the next node and remove it from open
            myNode = open[min];
            open.RemoveAt(min);

            //is it the destination node?
            if (myNode.indice == myPathEnd.indice)
            {
                // myPath = Closed[Closed.push(myNode) - 1];
                closed.Add(myNode);
                myPath = myNode;

                do
                {
                    result.Add(new Vector2Int(myPath.x, myPath.y));

                } while ((myPath = myPath.parent) != null);

                AStar = new bool[0];
                closed = new List<Node>();
                open = new List<Node>();

                result.Reverse();
            }
            else//not the destination
            {
                //find which nearby nodes are walkeable
                myNeightbours = Neightbours(new Vector2Int(myNode.x, myNode.y));
                //test each one that hasn't been tried already
                for (i = 0, j = myNeightbours.Count; i < j; i++)
                {
                    myPath = new Node(myNode, new Vector2Int(myNeightbours[i].x, myNeightbours[i].y));
                    //estimated cost of this particular route so far
                    myPath.g = myNode.g + tablero[myNode.x, myNode.y]; /*ManhattanDistance(myNeightbours[i], new Vector2Int(myNode.x, myNode.y))*/
                    //estimated cost of entire guessed route to the destination
                    myPath.f = myPath.g + ManhattanDistance(myNeightbours[i], new Vector2Int(myPathEnd.x, myPathEnd.y));

                    if (!AStar[myPath.indice])
                    {//Nodo estudiado por primera vez
                        //remember this new path for testing above
                        open.Add(myPath);
                        //mark this node int the world graph as visited
                        AStar[myPath.indice] = true;
                    }
                    else
                    {
                        for (int n = 0; n < open.Count; n++)
                        {
                            if (open[n].x == myPath.x && open[n].y == myPath.y && open[n].f > myPath.f)
                            {
                                open[n] = myPath;
                                break;
                            }
                        }
                    }
                }
                //remember this route as having no more untested options
                closed.Add(myNode);
            }
        }//keep iterating until the open list is empty

        /* string s = "Ruta: ";
         foreach (var n in result)
             s += "(" + n.x + ", " + n.y + ") ";
         Debug.Log(s);*/

        return result;
    }

    List<Vector2Int> Neightbours(Vector2Int pos)
    {
        List<Vector2Int> lResult = new List<Vector2Int>();
        //Neightbours
        int N = pos.y - 1;
        int S = pos.y + 1;
        int W = pos.x - 1;
        int E = pos.x + 1;

        bool myN = N > -1 && CanWalkHere(pos.x, N);
        bool myS = S < worldHeight && CanWalkHere(pos.x, S);
        bool myE = E < worldWidth && CanWalkHere(E, pos.y);
        bool myW = W > -1 && CanWalkHere(W, pos.y);

        if (myN)
            lResult.Add(new Vector2Int(pos.x, N));
        if (myE)
            lResult.Add(new Vector2Int(E, pos.y));
        if (myS)
            lResult.Add(new Vector2Int(pos.x, S));
        if (myW)
            lResult.Add(new Vector2Int(W, pos.y));

        return lResult;

    }

    int ManhattanDistance(Vector2Int point, Vector2Int goal)
    {
        //Falta calcular el coste de las casillas embarradas
        int dist = Mathf.Abs(point.x - goal.x) + Mathf.Abs(point.y - goal.y);
        return dist;
    }

    bool CanWalkHere(int x, int y)
    {
        if (x < 0 || x >= 10) return false;
        if (y < 0 || y >= 5) return false;
        if (tablero[x, y] == 0) return false;
        return true;
    }
    //Imprime en un mismo debug todo el tablero (done)
    void PrintTablero(int[,] t)
    {
        Debug.Log(//Menuda inmundicia
            t[0, 4] + " " + t[1, 4] + " " + t[2, 4] + " " + t[3, 4] + " " + t[4, 4] + " " + t[5, 4] + " " + t[6, 4] + " " + t[7, 4] + " " + t[8, 4] + " " + t[9, 4] + "\n" +
            t[0, 3] + " " + t[1, 3] + " " + t[2, 3] + " " + t[3, 3] + " " + t[4, 3] + " " + t[5, 3] + " " + t[6, 3] + " " + t[7, 3] + " " + t[8, 3] + " " + t[9, 3] + "\n" +
            t[0, 2] + " " + t[1, 2] + " " + t[2, 2] + " " + t[3, 2] + " " + t[4, 2] + " " + t[5, 2] + " " + t[6, 2] + " " + t[7, 2] + " " + t[8, 2] + " " + t[9, 2] + "\n" +
            t[0, 1] + " " + t[1, 1] + " " + t[2, 1] + " " + t[3, 1] + " " + t[4, 1] + " " + t[5, 1] + " " + t[6, 1] + " " + t[7, 1] + " " + t[8, 1] + " " + t[9, 1] + "\n" +
            t[0, 0] + " " + t[1, 0] + " " + t[2, 0] + " " + t[3, 0] + " " + t[4, 0] + " " + t[5, 0] + " " + t[6, 0] + " " + t[7, 0] + " " + t[8, 0] + " " + t[9, 0] + "\n"
            );
       
    }
    //Traduce el array de gameObjects a un int[,] (done)
    int[,] TraduceTablero(int[,] tab)
    {
        int[,] result = new int[10, 5];

        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                if (tab[i, j] >0) result[i, j] = 1;
                else result[i, j] = 0;
            }
        }
        return result;
    }

}
