using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnoPlayer : MonoBehaviour {

	// En cada turno del player, ocurren las siguientes acciones:
    // 1 - Tomar una decisión, ya sea por el usuario o por la IA
    // 2 - Realizar acción correspondiente (atacar, retroceder o esperar)
    // 3 - En el caso de atacar, encontrar enemigo más cercano y ver si al avanzar la casilla entra en contacto para luchar
    // 4 - Una vez acabada la acción termina su turno

    enum Decision { Atacar, Retroceder, Esperar}
    Decision dec;
    bool decisionTomada = false;

    public GUI gui;
    public void TomarDecision(int d)
    {
        if (d == 0) dec = Decision.Atacar;
        else if (d == 1) dec = Decision.Retroceder;
        else
        {
            dec = Decision.Esperar;
            TerminarTurno();
        }            

        gui.DesactivarBotones();
        decisionTomada = true;            
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
