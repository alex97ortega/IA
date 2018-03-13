using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Algoritmo : MonoBehaviour
{

    GameObject[,] estadoTablero;

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

        public Node parent;
        public int indice;
        public int x;
        public int y;
        public int f;
        public int g;

    }

    public List<Vector2Int> CalcularRuta(Vector3 tanque, Vector3 otherTank, Vector3 anotherTank, Vector3 destino, GameObject[,] estadoTablero)
    {
        //Get the current state into the local format
        start = new Vector2Int((int)tanque.x, (int)tanque.y);
        goal = new Vector2Int((int)destino.x, (int)destino.y);
        tablero = TraduceTablero(estadoTablero, otherTank, anotherTank);

        PrintTablero(tablero);
        
        //INIT LOCAL VARIABLES
        Node myPathStart = new Node(null, new Vector2Int(start.x, start.y));
        Node myPathEnd = new Node(null, new Vector2Int(goal.x, goal.y));
        //Constains all the world cells
        bool[] AStar = new bool[worldSize];
        //list of currently open Nodes
        List<Node> open = new List<Node> { myPathStart };
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
            open.RemoveAt(min);
            myNode = open[0];

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

                    if (!AStar[myPath.indice])
                    {
                        //estimated cost of this particular route so far
                        myPath.g = myNode.g + ManhattanDistance(myNeightbours[i], new Vector2Int(myNode.x, myNode.y));
                        //estimated cost of entire guessed route to the destination
                        myPath.f = myNode.f + ManhattanDistance(myNeightbours[i], new Vector2Int(myPathEnd.x, myPathEnd.y));
                        //remember this new path for testing above
                        open.Add(myPath);
                        //mark this node int the world graph as visited
                        AStar[myPath.indice] = true;

                    }
                }
                //remember this route as having no more untested options
                closed.Add(myNode);
            }
        }//keep iterating until the open list is empty
        
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

        if (tablero[x, y] == 0) return false;
        if (x < 0 || x >= 10) return false;
        if (y < 0 || y >= 10) return false;

        return true;
    }
    //Imprime en un mismo debug todo el tablero (done)
    void PrintTablero(int[,] t)
    {
        Debug.Log(//Menuda inmundicia
            t[0, 9] + " " + t[1, 9] + " " + t[2, 9] + " " + t[3, 9] + " " + t[4, 9] + " " + t[5, 9] + " " + t[6, 9] + " " + t[7, 9] + " " + t[8, 9] + " " + t[9, 9] + "\n" +
            t[0, 8] + " " + t[1, 8] + " " + t[2, 8] + " " + t[3, 8] + " " + t[4, 8] + " " + t[5, 8] + " " + t[6, 8] + " " + t[7, 8] + " " + t[8, 8] + " " + t[9, 8] + "\n" +
            t[0, 7] + " " + t[1, 7] + " " + t[2, 7] + " " + t[3, 7] + " " + t[4, 7] + " " + t[5, 7] + " " + t[6, 7] + " " + t[7, 7] + " " + t[8, 7] + " " + t[9, 7] + "\n" +
            t[0, 6] + " " + t[1, 6] + " " + t[2, 6] + " " + t[3, 6] + " " + t[4, 6] + " " + t[5, 6] + " " + t[6, 6] + " " + t[7, 6] + " " + t[8, 6] + " " + t[9, 6] + "\n" +
            t[0, 5] + " " + t[1, 5] + " " + t[2, 5] + " " + t[3, 5] + " " + t[4, 5] + " " + t[5, 5] + " " + t[6, 5] + " " + t[7, 5] + " " + t[8, 5] + " " + t[9, 5] + "\n" +
            t[0, 4] + " " + t[1, 4] + " " + t[2, 4] + " " + t[3, 4] + " " + t[4, 4] + " " + t[5, 4] + " " + t[6, 4] + " " + t[7, 4] + " " + t[8, 4] + " " + t[9, 4] + "\n" +
            t[0, 3] + " " + t[1, 3] + " " + t[2, 3] + " " + t[3, 3] + " " + t[4, 3] + " " + t[5, 3] + " " + t[6, 3] + " " + t[7, 3] + " " + t[8, 3] + " " + t[9, 3] + "\n" +
            t[0, 2] + " " + t[1, 2] + " " + t[2, 2] + " " + t[3, 2] + " " + t[4, 2] + " " + t[5, 2] + " " + t[6, 2] + " " + t[7, 2] + " " + t[8, 2] + " " + t[9, 2] + "\n" +
            t[0, 1] + " " + t[1, 1] + " " + t[2, 1] + " " + t[3, 1] + " " + t[4, 1] + " " + t[5, 1] + " " + t[6, 1] + " " + t[7, 1] + " " + t[8, 1] + " " + t[9, 1] + "\n" +
            t[0, 0] + " " + t[1, 0] + " " + t[2, 0] + " " + t[3, 0] + " " + t[4, 0] + " " + t[5, 0] + " " + t[6, 0] + " " + t[7, 0] + " " + t[8, 0] + " " + t[9, 0] + "\n"
            );
        //Esta mostruasidad está generada c++ porque si no nos podemos morir con este código
        /*int main()
          {
            string s;
            int i;
            for(int j = 9; j >=0 ; j--)
            {
                for (i = 0; i < 10 - 1 ; i++)
                {
                    s += " t[" + to_string(i) + ", " + to_string(j) + "] + ' ' + ";
                }
                s += " t[" + to_string(i) + ", " + to_string(j) + "] + '\\n'";
                cout<< s<< endl;
                s = "";
            }
          }
          */
    }
    //Traduce el array de gameObjects a un int[,] (done)
    int[,] TraduceTablero(GameObject[,] GOtablero, Vector3 a, Vector3 b)
    {
        int[,] result = new int[10, 10];

        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                //comprobar si i == x && j == y
                if ((j == a.y && i == a.x) || (j == b.y && i == b.x))
                    result[i, j] = 0;
                else
                    result[i, j] = (int)GOtablero[i, j].GetComponent<Index>().getIndex();
            }
        }
        return result;
    }

}
