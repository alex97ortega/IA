using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnoPlayer : MonoBehaviour {

	// En cada turno del player, ocurren las siguientes acciones:
    // 1 - Tomar una decisión, ya sea por el usuario o por la IA
    // 2 - Realizar acción correspondiente (atacar, retroceder o esperar)
    // 3 - En el caso de atacar, encontrar enemigo más cercano y ver si al avanzar la casilla entra en contacto para luchar
    // 4 - Una vez acabada la acción termina su turno

    
    bool decisionTomada = false;

    public GUI gui;
    Vector2Int casillaSig;

    int posx, posy;
    int N, S, E, W;
    int distancia;
    int vx, vy;
    bool arriba = true, dch = true;

    public void TomarDecision(int d)
    {
        if (d == 0) Atacar();
        else if (d == 1) Retroceder();
        else TerminarTurno();                

        gui.DesactivarBotones();
        decisionTomada = true;            
    }
    void Atacar()
    {
        
    }
    void Retroceder()
    {
        
    }
    private void Update()
    {
        
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
    public void DecisionIA()
    {
        // movidas de redes bayesianas

        gui.DesactivarBotones();
        decisionTomada = true;
    }
    void TerminarTurno()
    {
        decisionTomada = false;
        gui.CambiarTurno();
    }
}
