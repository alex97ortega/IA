using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdCasilla : MonoBehaviour {
    uint id = 1; // 0->hueco , 1->normal , 2->sangre, 3->barro, 4->barro+sangre

    public Sprite hueco;
    public Sprite arena;
    public Sprite sangre;
    public Sprite barro;
    public Sprite barroSangre;

    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = arena;
    }
   
    public void ChangeId(uint nuevo)
    {
        id = nuevo;
        switch (id)
        {
            case 0:
                GetComponent<SpriteRenderer>().sprite = hueco;
                break;
            case 1:
                GetComponent<SpriteRenderer>().sprite = arena;
                break;
            case 2:
                GetComponent<SpriteRenderer>().sprite = sangre;
                break;
            case 3:
                GetComponent<SpriteRenderer>().sprite = barro;
                break;
            case 4:
                GetComponent<SpriteRenderer>().sprite = barroSangre;
                break;
        }
    }
}
