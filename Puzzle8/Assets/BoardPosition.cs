using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardPosition : MonoBehaviour {
    public Vector2Int boardPosition;
    public void MoveFicha(int x, int y)
    {
        boardPosition.x += x;
        boardPosition.y += y;
    }
}
