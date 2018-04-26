using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class TurnoPlayer : MonoBehaviour {

	// En cada turno del player, ocurren las siguientes acciones:
    // 1 - Tomar una decisión, ya sea por el usuario o por la IA
    // 2 - Realizar acción correspondiente (atacar, retroceder o esperar)
    // 3 - En el caso de atacar, encontrar enemigo más cercano y ver si al avanzar la casilla entra en contacto para luchar
    // 4 - Una vez acabada la acción termina su turno

    
    bool decisionTomada = false;

    public GUI gui;
    public GameManager gm;
    Vector2Int casillaSig;
    GameObject objetivo;

    public int posx, posy;
    int N, S, E, W;
    int distancia;
    int vx, vy;
    bool arriba = true, dch = true;

    public GameObject textofin;

    public void TomarDecision(int d)
    {
        DamePos();
        casillaSig = new Vector2Int(posx,posy);

        if (d == 0) Atacar();
        else if (d == 1) Retroceder();
        else TerminarTurno(); //esperar               

        gui.DesactivarBotones();           
    }
    void Atacar()
    {
        distancia = 200;
        
            foreach (GameObject a in gm.enemies)
            {
            if (a != null)
            {
                int aux = Math.Abs(posx - (int)a.GetComponent<TurnoEnemy>().posx) +
                 Math.Abs(posy - (int)a.GetComponent<TurnoEnemy>().posy);
                if (aux <= distancia)
                {
                    objetivo = a;
                    distancia = aux;
                }
            }
                
            }
        //Debug.Log("Distancia con el muerto: " + distancia);
        if(distancia!=0)   CasillaSiguiente();
        decisionTomada = true;
    }
    void Retroceder()
    {
        if (posy > 0)  vy=-1;
        else vx=-1;
        casillaSig.x += vx;
        casillaSig.y += vy;
        DarVelocidad();
        decisionTomada = true;
    }
    public void DecisionIA()
    {
        // movidas de redes bayesianas

        gui.DesactivarBotones();
        decisionTomada = true;
    }
    void CasillaSiguiente()
    {
        bool myS = S > -1;
        bool myN = N < gm.filas;
        bool myE = E < gm.columnas;
        bool myW = W > -1;
        int x = 0, y = 0;

        if (myS)
        {
            int aux = Math.Abs(posx - objetivo.GetComponent<TurnoEnemy>().posx) +
             Math.Abs(posy - 1 - objetivo.GetComponent<TurnoEnemy>().posy);

            if (aux <= distancia)
            {
                x = 0;
                y = -1;
                distancia = aux;
            }

        }
        if (myW)
        {
            int aux = Math.Abs(posx - 1 - objetivo.GetComponent<TurnoEnemy>().posx) +
             Math.Abs(posy - objetivo.GetComponent<TurnoEnemy>().posy);

            if (aux <= distancia)
            {
                x = -1;
                y = 0;
                distancia = aux;
            }
        }
        if (myE)
        {

            int aux = Math.Abs(posx + 1 - objetivo.GetComponent<TurnoEnemy>().posx) +
            Math.Abs(posy - objetivo.GetComponent<TurnoEnemy>().posy);
            if (aux <= distancia)
            {
                x = 1;
                y = 0;
                distancia = aux;
            }
        }
        if (myN)
        {

            int aux = Math.Abs(posx - objetivo.GetComponent<TurnoEnemy>().posx) +
             Math.Abs(posy + 1 - objetivo.GetComponent<TurnoEnemy>().posy);

            if (aux <= distancia)
            {
                x = 0;
                y = 1;
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
    private void Update()
    {
        DamePos();
        if (decisionTomada)
        {
            if(casillaSig.x ==posx && casillaSig.y == posy)
            {
                if (posx == 0 && posy == 0) AcabarPartida();
                else if ((Math.Abs(posx - objetivo.GetComponent<TurnoEnemy>().posx) +
                 Math.Abs(posy - objetivo.GetComponent<TurnoEnemy>().posy))==0) {

                    // combate si toca el objetivo
                    // tengo que comprobar tambien si hay varios objetivos para combatirlos por separado
                    if (gui.MatarEnemigo(objetivo, true)) AcabarPartida();
                    else TerminarTurno();
                }
                else TerminarTurno();
            }
        }
    }
    void DamePos()
    {
        if (vy < 0) arriba = false;
        else if (vy > 0) arriba = true;

        if (arriba) posy = (int)transform.position.y;
        else posy = (int)Mathf.Round(transform.position.y + 0.5f);


        if (vx < 0) dch = false;
        else if (vx > 0) dch = true;

        if (dch) posx = (int)transform.position.x;
        else posx = (int)Mathf.Round(transform.position.x + 0.5f);

        //Debug.Log("x: " + posx + " y: " + posy);
        N = posy + 1;
        S = posy - 1;
        W = posx - 1;
        E = posx + 1;
    }
    void DarVelocidad()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(vx, vy);
    }
    
    void TerminarTurno()
    {
        vx = 0;
        vy = 0;
        DarVelocidad();
        decisionTomada = false;
        gui.CambiarTurno();
    }
    public void AcabarPartida()
    {
        vx = 0;
        vy = 0;
        DarVelocidad();
        decisionTomada = false;
        textofin.SetActive(true);
    }
}
