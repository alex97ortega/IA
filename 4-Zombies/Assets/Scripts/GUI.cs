using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GUI : MonoBehaviour
{

    public GameObject botonAtacar;
    public GameObject botonVolver;
    public GameObject botonEsperar;
    public GameObject botonIA;

    public GameObject textoPuntos;
    public GameObject textoTurnos;

    int puntos = 0;
    int turnoPersonaje = -1; // -1 (no empezado), 0 (turno Enemies), 1 (turno personaje)
    int nEnemy;
    enum Destreza { Mala, Regular, Buena };
    Destreza destreza;
    enum Situacion { MuchoZombie, MuchoAliado, Neutral };
    Situacion situacion;

    public GameManager gm;
    public int numAliados, numEnemigos;
    bool llamado = false;
    // Script que se encarga de gestionar todas las movidas una vez que comienza el simulator
    // Puntos, turnos, acciones, destreza y situación 
    
    public void Comenzar()
    {
        numAliados = gm.maxA - gm.maxAliados;
        numEnemigos = gm.maxE - gm.maxEnemigos;
        if (turnoPersonaje == -1)
        {
            if (Random.Range(0, 2) == 0)
            {
                turnoPersonaje = 0;
                textoTurnos.GetComponent<Text>().text = "Zombies moviéndose...";
            }
            else
            {
                turnoPersonaje = 1;
                textoTurnos.GetComponent<Text>().text = "Tu turno";
                botonAtacar.SetActive(true);
                botonVolver.SetActive(true);
                botonEsperar.SetActive(true);
                botonIA.SetActive(true);
            }
        }
        else CambiarTurno();

        textoTurnos.SetActive(true);        
    }

    public void Reinciar()
    {
        DesactivarBotones();

        textoPuntos.GetComponent<Text>().text = "Puntos: 0";
        puntos = 0;
        turnoPersonaje = -1;

        textoTurnos.SetActive(false);
    }
    public void DesactivarBotones()
    {
        botonAtacar.SetActive(false);
        botonVolver.SetActive(false);
        botonEsperar.SetActive(false);
        botonIA.SetActive(false);
    }
    public void CambiarTurno()
    {
        if (turnoPersonaje == 0)
        {
            turnoPersonaje = 1;
            textoTurnos.GetComponent<Text>().text = "Tu turno";
            botonAtacar.SetActive(true);
            botonVolver.SetActive(true);
            botonEsperar.SetActive(true);
            botonIA.SetActive(true);
        }
        else
        {
            turnoPersonaje = 0;
            textoTurnos.GetComponent<Text>().text = "Zombies moviéndose...";
            nEnemy = gm.enemies.Length-1;
           
        }
    }
    private void Update()
    {
        // llamada a los zombies por orden (para que no hagan todos sus turnos a la vez)
        if(turnoPersonaje == 0)
        {

            //Debug.Log("llamando zombie " + nEnemy);
            if (gm.enemies[nEnemy].GetComponent<TurnoEnemy>().acabadoTurno)
            {
                if (!llamado)
                {
                    gm.enemies[nEnemy].GetComponent<TurnoEnemy>().Activar();
                    llamado = true;
                }
                else
                {
                    nEnemy--;
                    llamado = false;
                }
            }                
           
            if (nEnemy < 0) CambiarTurno();
            
        }
    }
    // tanto positivos como negativos
    void CambiarPuntos(int p)
    {
        puntos += p;
        textoPuntos.GetComponent<Text>().text = "Puntos: " + puntos;
    }
    // probabilidades según transparencias, se llaman antes de cada combate
    public void CalculaDestreza()
    {
        if (gm.noche)
        {
            if (numAliados == 0) destreza = Destreza.Mala;

            else if (numAliados == 1)
            {
                if (Random.Range(0, 10) > 0) destreza = Destreza.Regular;
                else destreza = Destreza.Mala;
            }
            else
            {
                if (Random.Range(0, 10) > 0) destreza = Destreza.Buena;
                else destreza = Destreza.Regular;
            }
        }
        else
        {
            if (numAliados == 0)
            {
                if (Random.Range(0, 10) > 0) destreza = Destreza.Mala;
                else destreza = Destreza.Regular;
            }
            else if (numAliados == 1) destreza = Destreza.Regular;
            else destreza = Destreza.Buena;
        }
    }
    // probabilidades según transparencias, se llaman antes de cada combate
    public void CalculaSituacion()
    {

        if (numAliados == 0) situacion = Situacion.MuchoZombie;
        if (numAliados == 1)
        {
            if (numEnemigos < 5)
            {
                int n = Random.Range(0, 10);
                if (n < 2) situacion = Situacion.MuchoZombie;
                else if (n > 3) situacion = Situacion.Neutral;
                else situacion = Situacion.MuchoAliado;
            }
            else
            {
                if (Random.Range(0, 10) > 0) situacion = Situacion.MuchoZombie;
                else situacion = Situacion.Neutral;
            }
        }
        else
        {
            if (numEnemigos < 5)
            {
                int n = Random.Range(0, 10);
                if (n < 1) situacion = Situacion.MuchoZombie;
                else if (n > 4) situacion = Situacion.Neutral;
                else situacion = Situacion.MuchoAliado;
            }
            else
            {
                if (Random.Range(0, 10) > 3) situacion = Situacion.MuchoZombie;
                else situacion = Situacion.Neutral;
            }
        }
    }

    public void MatarAliado(GameObject al)
    {
        Destroy(al);
        CambiarPuntos(-10);
        numAliados--;
    }
    public void MatarEnemigo(GameObject en, bool matadoHeroe)
    {
        Destroy(en);
        if(matadoHeroe)
            CambiarPuntos(5);
        else CambiarPuntos(1);
        numEnemigos--;
    }
    public void MatarHeroe()
    {
        //finalizar partida
    }
}
