using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdCasilla : MonoBehaviour {
    //uint id = 1; // 0->hueco , 1->normal , 2->sangre, 3->barro, 4->barro+sangre
    public Sprite hueco;
    public Sprite arena;
    public Sprite sangre;
    public Sprite barro;
    public Sprite barroSangre;

    public enum Tipo
    {
        normal,
        hueco,
        sangre,
        barro,
        barroSangre
    }
    Tipo t =  Tipo.normal;

    public Tipo GetTipo() { return t; }
    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = arena;
    }
   
    public void ChangeId(Tipo nuevo)
    {
        t = nuevo;
        switch (t)
        {
            case Tipo.hueco:
                GetComponent<SpriteRenderer>().sprite = hueco;
                break;
            case Tipo.normal:
                GetComponent<SpriteRenderer>().sprite = arena;
                break;
            case Tipo.sangre:
                GetComponent<SpriteRenderer>().sprite = sangre;
                break;
            case Tipo.barro:
                GetComponent<SpriteRenderer>().sprite = barro;
                break;
            case Tipo.barroSangre:
                GetComponent<SpriteRenderer>().sprite = barroSangre;
                break;
        }
    }
}
