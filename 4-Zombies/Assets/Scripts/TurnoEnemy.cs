using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class TurnoEnemy : MonoBehaviour {

    // En cada turno, cada enemigo realiza los siguientes pasos:
    // 1 - Buscar objetivo más cercano (ya sea aliado o player)
    // 2 - Avanzar una casilla hacia el objetivo
    // 3 - Si entra en contacto con el objetivo, se lucha
    // 4 - Acaba su turno y comienza el turno otro enemigo o el player

    public GameManager gm;
    public GUI gui;
    GameObject objetivo;
    Vector2Int casillaSig;
    public bool acabadoTurno = true;

    int posx, posy;
    int N, S, E, W;
    int distancia;
    int vx, vy;
    bool arriba=true, dch=false;
    
    public void Activar()
    {
        BuscaObjetivo();
        acabadoTurno = false;
    }
    public void Update()
    {
        if (!acabadoTurno)
        {
            DamePos();

            if (posx == casillaSig.x && posy == casillaSig.y) TerminarTurno();
        }
    }
    void BuscaObjetivo()
    {
        DamePos();
        objetivo = gm.protas;
        distancia = Math.Abs(posx - (int)objetivo.transform.position.x) +
             Math.Abs(posy- (int)objetivo.transform.position.y);
        Debug.Log("Distancia con prota: " + distancia);

        if(gui.numAliados != 0)
        {
            foreach (GameObject a in gm.alys)
            {
                int aux = Math.Abs(posx - (int)a.transform.position.x) +
                 Math.Abs(posy - (int)a.transform.position.y);

                Debug.Log("Distancia con aly: " + aux);
                if (aux <= distancia)
                {

                    objetivo = a;
                    distancia = aux;
                }
            }
        }        
        CasillaSiguiente();
    }

    void CasillaSiguiente()
    {
        bool myS = S > -1;
        bool myN = N < gm.filas;
        bool myE = E < gm.columnas;
        bool myW = W > -1;
        int x = 0, y = 0;
        if(myE)
        {
            
            int aux = Math.Abs(posx + 1 - (int)objetivo.transform.position.x) +
            Math.Abs(posy - (int)objetivo.transform.position.y);
            //Debug.Log("Distancia con objE: " + aux);
            if ( aux <= distancia )
            {
                x = 1;
                y = 0;
                distancia = aux;
            }
        }
        if (myN)
        {
            
            int aux = Math.Abs(posx  - (int)objetivo.transform.position.x) +
             Math.Abs(posy +1 - (int)objetivo.transform.position.y);

            //Debug.Log("Distancia con objN: " + aux);
            if (aux <= distancia )
            {
                x = 0;
                y = 1;
                distancia = aux;
            }
        }

        if (myS)
        {
            int aux = Math.Abs(posx  - (int)objetivo.transform.position.x) +
             Math.Abs(posy -1 - (int)objetivo.transform.position.y);
            //Debug.Log("Distancia con objS: " + aux);

            if (aux <= distancia )
            {
                x = 0;
                y = -1;
                distancia = aux;
            }

        }
        if (myW)
        {
            int aux = Math.Abs(posx - 1 - (int)objetivo.transform.position.x) +
             Math.Abs(posy - (int)objetivo.transform.position.y);
            //Debug.Log("Distancia con objW: " + aux);

            if (aux <= distancia )
            {
                x = -1;
                y = 0;
                distancia = aux;
            }
        }
        
        vx = x; vy = y;
        casillaSig = new Vector2Int(posx, posy);

        casillaSig.x = posx + x;
        casillaSig.y = posy + y;

        //Debug.Log("CasillaSig(" + casillaSig.x + "," + casillaSig.y + ")");
        DarVelocidad();
    }

    void DarVelocidad()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(vx, vy);
    }
    void DamePos()
    {
        //la movida de always
        if (vy < 0) arriba = false;
        else if (vy > 0) arriba = true;

        if (arriba) posy = (int)transform.position.y;
        else posy = (int)Mathf.Round(transform.position.y + 0.5f);


        if (vx < 0) dch = false;
        else if (vx > 0) dch = true;

        if (dch) posx = (int)transform.position.x;
        else posx = (int)Mathf.Round(transform.position.x + 0.5f);
        
        Debug.Log("x: " + posx + " y: " + posy);
        N = posy + 1;
        S = posy - 1;
        W = posx - 1;
        E = posx + 1;
    }

    void TerminarTurno()
    {
        vx = 0; vy = 0;
        DarVelocidad();
        //combate
        acabadoTurno = true;
        //Debug.Log("acabadoTurno");
    }
}
