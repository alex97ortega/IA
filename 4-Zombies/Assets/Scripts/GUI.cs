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
    public int getPuntos() { return puntos; }

    int turnoPersonaje = -1; // -1 (no empezado), 0 (turno Enemies), 1 (turno personaje)
    int nEnemy;

    public enum Destreza { Mala, Regular, Buena };
    public Destreza destreza;
    public enum Situacion { MuchoZombie, MuchoAliado, Neutral };
    public Situacion situacion;

    public GameManager gm;
    public int numAliados, numEnemigos;
    bool llamado = false;
    public bool finPartida = false;
    // Script que se encarga de gestionar todas las movidas una vez que comienza el simulator
    // Puntos, turnos, acciones, destreza y situación 
    
    public void Comenzar()
    {
        finPartida = false;
        numAliados = gm.maxA - gm.numeroAliados;
        numEnemigos = gm.maxE - gm.numeroEnemigos;

        nEnemy = numEnemigos - 1;
        //Debug.Log("numEnemigos: " + numEnemigos);
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
            nEnemy = numEnemigos-1;
           
        }
    }
    private void Update()
    {
        // llamada a los zombies por orden (para que no hagan todos sus turnos a la vez)
        if(turnoPersonaje == 0 && !finPartida)
        {

            //Debug.Log("llamando zombie " + nEnemy);
            if (gm.enemies[nEnemy] != null && gm.enemies[nEnemy].GetComponent<TurnoEnemy>().acabadoTurno)
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
            else if (gm.enemies[nEnemy] == null) nEnemy--;
            if (nEnemy < 0) CambiarTurno();
            
            
        }
    }
    // tanto positivos como negativos
    void CambiarPuntos(int p)
    {
        puntos += p;
        textoPuntos.GetComponent<Text>().text = "Puntos: " + puntos;
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
        if (numEnemigos == 0)
        {
            finPartida = true;
            gm.protas.GetComponent<TurnoPlayer>().AcabarPartida();
        }
    }
    public void MatarHeroe()
    {
        //finalizar partida
        finPartida = true;
        CambiarPuntos(-50);
        gm.protas.GetComponent<TurnoPlayer>().AcabarPartida();
        gm.protas.SetActive(false);
    }

    public void Combate(Vector2Int casilla)
    {

        bool victoriaHeroe;
        int rand = Random.Range(0, 10);
        // -10% de posibilidad de ganar los buenos si es de noche
        int nocheVision = 0;
        //más posibilidad de ganar si es el turno del personaje
        int heroe = 2 * turnoPersonaje;
        if (gm.noche) nocheVision = 1;

        int probVictory;
        if (numAliados >= 3)
        {
            probVictory = 7 + heroe - nocheVision; //70-30% en lugar de 90-10%
            victoriaHeroe = rand < probVictory;
        }
        else if (numAliados != 0)
        {
            probVictory = 5 + heroe - nocheVision; //50-50% o 60-40% en el caso de noche
            victoriaHeroe = rand < probVictory;
        }

        else
        {
            probVictory = 3 + heroe - nocheVision; // 30-70%
            victoriaHeroe = rand < probVictory;
        }

        Debug.Log("Probabilidad de ganar soldados :" + probVictory * 10 + "%");

        //cargarse los bichos
        if (victoriaHeroe)
        {
            foreach (GameObject a in gm.enemies)
            {
                if (a != null && a.GetComponent<TurnoEnemy>().posx==casilla.x
                    && a.GetComponent<TurnoEnemy>().posy == casilla.y)
                {
                    bool estaHeroe = gm.protas.GetComponent<TurnoPlayer>().posx == casilla.x &&
                        gm.protas.GetComponent<TurnoPlayer>().posy == casilla.y;
                    MatarEnemigo(a, estaHeroe);
                }
            }
        }
        //cargarse al heroe o a los aliados que sea
        else
        {
            if (gm.protas.GetComponent<TurnoPlayer>().posx == casilla.x
                && gm.protas.GetComponent<TurnoPlayer>().posy == casilla.y) MatarHeroe();
            else
            {
                foreach (GameObject a in gm.alys)
                {
                    if (a != null && a.transform.position.x == casilla.x
                        && a.transform.position.y == casilla.y)
                    {
                        MatarAliado(a);
                    }
                }
            }
        }
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
}
