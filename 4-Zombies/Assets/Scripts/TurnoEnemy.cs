using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnoEnemy : MonoBehaviour {

    // En cada turno, cada enemigo realiza los siguientes pasos:
    // 1 - Buscar objetivo más cercano (ya sea aliado o player)
    // 2 - Avanzar una casilla hacia el objetivo
    // 3 - Si entra en contacto con el objetivo, se lucha
    // 4 - Acaba su turno y comienza el turno otro enemigo o el player


    public bool acabadoTurno = true;
    
    public void Activar()
    {
        acabadoTurno = false;
    }
    public void Update()
    {
        if (!acabadoTurno)
        {
             TerminarTurno();
        }
    }
    void TerminarTurno()
    {
        acabadoTurno = true;
        //Debug.Log("acabadoTurno");
    }
}
