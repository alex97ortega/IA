using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Algoritmo : MonoBehaviour {

    GameObject[,] estadoTablero;

    int[,] tablero;

	public bool CalcularRuta(GameObject tanque, Vector3 destino, GameObject[,] estadoTablero)
    {

        return true;
    }

    Vector2Int[] Neightbours(Vector2Int pos)
    {
        Vector2Int[] result;
        List<Vector2Int> lResult = new List<Vector2Int>();

        int N = pos.y - 1;
        int S = pos.y + 1;
        int W = pos.x - 1;
        int E = pos.x + 1;






        result = new Vector2Int[lResult.Count];
        int i = 0;
        foreach(var r in lResult)
        {
            result[i] = r;
            i++;
        }
        return result;

    }

    int ManhattanDistance(Vector2Int point, Vector2Int goal)
    {
        return Mathf.Abs(point.x - goal.x) + Mathf.Abs(point.y - goal.y);
    }

    bool CanWalkHere(Vector2Int pos) {
        return true;
    }

    int[,] TraduceTablero(GameObject[,] GOtablero) {
        int[,] result = new int[10, 10];

        for(int i = 0; i < 10; i++)
        {
            for(int j = 0; j < 10; j++)
            {
                result[i, j] = (int)GOtablero[i, j].GetComponent<Index>().getIndex();
            }
        }

        return result;
    }
}
