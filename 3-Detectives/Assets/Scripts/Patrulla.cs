using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* este Script se encarga de que el detective haga una patrulla de búsqueda estándar de este estilo:
 
   □ □ □ □ □ □ □ □ □ □ □ 
   □ x x x x x x x x x □   
   □ □ □ □ □ □ □ □ □ x □ 
   □ x x x x x x x x x □ 
   s x □ □ □ □ □ □ □ □ □  
  
   Ya que, según el enunciado, la clave es encontrar el cadáver que está rodeado de sangre, así que buscamos un área
   de 2 casillas en el mapa. Por lo que podemos obviar filas en la patrulla para hacerla lo más mínima posible.
   El camino con X es el recorrido mínimo de casillas que puede hacer el detective para encontrar el cadáver en cualquier
   punto del mapa.

   Después, el script también se encargará de bordear los huecos (que es lo chungo) y encontrar el cadáver y el arma
   en cuanto toque una casilla de sangre. Para ello se emplea lógica proposicional.

   Una vez encontrado cadáver y arma, llama al script del algoritmo para encontrar el camino mínimo a casa, conociendo ya
   parte del tablero.
*/
public class Patrulla : MonoBehaviour {


    public GameManager gm;
    public GameObject arma;
    public GameObject cadaver;

    // el camino va a ser una representación del tablero con booleanos que indican si la casilla es camino o no
    bool[,] camino;

    // el mapa se va marcando con las casillas por las que va pasando el detective, y esto tiene dos funciones:
    // que el detective no pase innecesariamente por una casilla ya descubierta en cuanto encuentra la sangre
    // y que el detective pueda volver a su casa al encontrar lo que busca por el camino más corto entre las casillas
    // descubiertas
    bool[,] descubiertas;

    Vector2 siguienteCasilla;
    Vector2 casillaClave;
    public float velocity;

    float vx, vy;
    int posx, posy;

    // los distintos modos de moverse del agente los tomo en un enumerado
    enum Modo { Patrullando, tocadoSangre, encontradoArma, encontradoMuerto, Paro}
    Modo m;

    bool n = true;
    bool s = true;
    bool e = true;
    bool w = true;

    // para controlar la posición
    bool arriba = true;
    bool dch = true;
    bool llamado = false;

    bool started = false;
    // Update is called once per frame
    void Update()
    {
        DamePos();
        //muere si cae al pozo
        if (gm.casillas[posx, posy].GetComponent<IdCasilla>().GetTipo() == IdCasilla.Tipo.hueco) gm.Muere();

        switch (m)
        {
            case Modo.Patrullando:
                PatrullandoIndaNight();
                break;
            case Modo.tocadoSangre:
                PatrullaSangre();
                break;
            case Modo.Paro:
                
                if(!llamado) {
                    GetComponent<VueltaAcasa>().Volver(new Vector2Int(posx, posy), descubiertas, velocity, arriba, dch);
                    llamado = true;
                }             
                    
                // el otro algoritmo
                break;
            case Modo.encontradoArma:
                if (posx == cadaver.transform.position.x && posy == cadaver.transform.position.y)
                {
                    gm.casillas[posx, posy].GetComponent<SpriteRenderer>().color = Color.white;
                    cadaver.GetComponent<SpriteRenderer>().color = Color.white;
                    cadaver.transform.position = new Vector2(11.5f, 0);
                    m = Modo.Paro;
                }
                else if (posx == siguienteCasilla.x && posy == siguienteCasilla.y)
                {
                    GetComponent<Rigidbody2D>().velocity = CalculateVel();
                }
                break;
            case Modo.encontradoMuerto:
                if (posx == arma.transform.position.x && posy == arma.transform.position.y)
                {
                    gm.casillas[posx, posy].GetComponent<SpriteRenderer>().color = Color.white;
                    arma.GetComponent<SpriteRenderer>().color = Color.white;
                    arma.transform.position = new Vector2(10.5f, 0);
                    m = Modo.Paro;
                }               

                else if (posx == siguienteCasilla.x && posy == siguienteCasilla.y)
                {
                    GetComponent<Rigidbody2D>().velocity = CalculateVel();
                }
                break;
            default:
                break;
        }        
    }

    // Start
    public void Patrullar()
    {
        m = Modo.Patrullando;
        llamado = false;
        started = true;
        n = true;
        s = true;
        e = true;
        w = true;

        vx = 0;
        vy = 0;
        siguienteCasilla = new Vector2(0, 0);
        CreaCamino();
        //PrintCamino();
    }

    public void CreaCamino()
    {
        /* □ □ □ □ □ □ □ □ □ □ □ 
           □ x x x x x x x x x □   
           □ □ □ □ □ □ □ □ □ x □ 
           □ x x x x x x x x x □ 
           s x □ □ □ □ □ □ □ □ □  */

        camino = new bool[gm.columnas, gm.filas];

        for (int i = 0; i < gm.columnas; i++)
        {
            for (int j = 0; j < gm.filas; j++)
            {
                if (j % 2 != 0 && i != 0 && i != gm.columnas - 1) camino[i, j] = true;

                else if (i == 1 && j % 4 == 0) camino[i, j] = true;

                else if (i == gm.columnas - 2 && j % 4 != 0) camino[i, j] = true;

                else camino[i, j] = false;
            }
        }
        // ponemos el tablero de casillas descubiertas a false excepto la 0,0
        descubiertas = new bool[gm.columnas, gm.filas];

        for (int i = 0; i < gm.columnas; i++)
        {
            for (int j = 0; j < gm.filas; j++)
            {
                descubiertas[i, j] = false;
            }
        }
        descubiertas[0, 0] = true;
    }

    // Update
    void DamePos()
    {
        //toa la movida de siempre

        if (vy < 0) arriba = false;
        else if (vy>0) arriba = true;

        if (arriba) posy = (int)transform.position.y;
        else posy = (int)Mathf.Round(transform.position.y + 0.5f);


        if (vx < 0) dch = false;
        else if (vx > 0) dch = true;

        if(dch) posx = (int)transform.position.x;
        else posx = (int)Mathf.Round(transform.position.x + 0.5f);

        //print("posicion (" + posx + " , " + posy + ")");
    }

    void PatrullandoIndaNight()
    {
        //print("siguiente casilla (" + siguienteCasilla.x + " , " + siguienteCasilla.y + ")");
        //print("posicion (" + posx+ " , " + posy + ")");

        if (posx == siguienteCasilla.x && posy == siguienteCasilla.y)
        {
            GetComponent<Rigidbody2D>().velocity = CalculateVel();
        }


        // hacemos cosas dependiendo de la casilla en la que estemos 



        //movidas impresionantes si hay barro
       /* if (gm.casillas[posx, posy].GetComponent<IdCasilla>().GetTipo() == IdCasilla.Tipo.barro)
            TocadoBarro(new Vector2(posx, posy));*/

        //buscar cuerpo si hay sangre (en 4 direcciones)
        else if (gm.casillas[posx, posy].GetComponent<IdCasilla>().GetTipo() == IdCasilla.Tipo.barroSangre ||
                gm.casillas[posx, posy].GetComponent<IdCasilla>().GetTipo() == IdCasilla.Tipo.sangre)
            CambiarModo(Modo.tocadoSangre, new Vector2(posx, posy));

        //buscar cuerpo o arma si se encuentra uno de los dos (en un área de 3x3)
        if (posx == arma.transform.position.x && posy == arma.transform.position.y)
            CambiarModo(Modo.encontradoArma, new Vector2(posx, posy));
        else if (posx == cadaver.transform.position.x && posy == cadaver.transform.position.y)
            CambiarModo(Modo.encontradoMuerto,new Vector2(posx, posy));
    }

    void PatrullaSangre()
    {
        //print("siguiente casilla (" + siguienteCasilla.x + " , " + siguienteCasilla.y + ")");


        if (posx == arma.transform.position.x && posy == arma.transform.position.y)
            CambiarModo(Modo.encontradoArma,new Vector2(posx, posy));
        else if (posx == cadaver.transform.position.x && posy == cadaver.transform.position.y)
            CambiarModo(Modo.encontradoMuerto,new Vector2(posx, posy));

        if (casillaClave == siguienteCasilla && (posx == siguienteCasilla.x && posy == siguienteCasilla.y))
        {
            GetComponent<Rigidbody2D>().velocity = CalculateVel2();
        }
        else if (posx == siguienteCasilla.x && posy == siguienteCasilla.y)
        {
            vx *= -1;
            vy *= -1;
            gm.casillas[posx, posy].GetComponent<SpriteRenderer>().color = Color.white;
            GetComponent<Rigidbody2D>().velocity = new Vector2(vx, vy);
            siguienteCasilla = casillaClave;
        }
    }

    Vector2 CalculateVel()
    {
        

        int N = posy + 1;
        int S = posy - 1;
        int W = posx - 1;
        int E = posx + 1;

        bool myS = S > -1 && camino[posx, S];
        bool myN = N < gm.filas && camino[posx, N];
        bool myE = E < gm.columnas && camino[E, posy] ;
        bool myW = W > -1 && camino[W, posy];

        camino[posx, posy] = false;
        gm.casillas[posx,posy].GetComponent<SpriteRenderer>().color = Color.white;
        descubiertas[posx, posy] = true;
        int x=0, y=0;
        if (myN)
        {
            //print("N");
            x = 0;
            y = 1;
            siguienteCasilla.y++;
        }
        else if (myE)
        {
            //print("E");
            x = 1;
            y = 0;
            siguienteCasilla.x++;
        }
        else if (myS)
        {
            //print("S");
            x = 0;
            y = -1;
            siguienteCasilla.y--;
        }
        else if (myW)
        {
            //print("W");
            x = -1;
            y = 0;
            siguienteCasilla.x--;
        }
        vx = x * velocity;
        vy = y * velocity;
        return new Vector2(vx,vy);
    }

    Vector2 CalculateVel2()
    {
        // el recorrido para cuando se encuentra sangre es: 

        // □ x □
        // x p x
        // □ x □

        // donde p es la posición actual

        int N = posy + 1;
        int S = posy - 1;
        int W = posx - 1;
        int E = posx + 1;

        bool myS = S > -1 && s && !descubiertas[posx, S] ;
        bool myN = N < gm.filas && n && !descubiertas[posx, N];
        bool myE = E < gm.columnas && e && !descubiertas[E, posy];
        bool myW = W > -1 && w && !descubiertas[W, posy];

        gm.casillas[posx, posy].GetComponent<SpriteRenderer>().color = Color.white;
        descubiertas[posx, posy] = true;
        int x = 0, y = 0;
        if (myS)
        {
            //print("S");
            x = 0;
            y = -1;
            siguienteCasilla.y--;
            s = false;
        }
        else if (myW)
        {
            //print("W");
            x = -1;
            y = 0;
            siguienteCasilla.x--;
            w = false;
        }
        else if (myN)
        {
            //print("N");
            x = 0;
            y = 1;
            siguienteCasilla.y++;
            n = false;
        }
        else if (myE)
        {
            //print("E");
            x = 1;
            y = 0;
            siguienteCasilla.x++;
            e = false;
        }
        
        vx = x * velocity;
        vy = y * velocity;
        return new Vector2(vx, vy);
    }

    // casillaInicio = casilla donde se cambia de modo, para tenerla de referencia
    void CambiarModo(Modo cambiar, Vector2 casillaInicio)
    {
        m = cambiar;
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        casillaClave = casillaInicio;
        switch (m)
        {
            case Modo.tocadoSangre:
                siguienteCasilla = casillaClave;
                break;
            case Modo.encontradoArma:
                arma.GetComponent<SpriteRenderer>().color = Color.white;
                arma.transform.position = new Vector2(10.5f, 0);
                CreaPatrulla2();
                break;
            case Modo.encontradoMuerto:
                cadaver.GetComponent<SpriteRenderer>().color = Color.white;
                cadaver.transform.position = new Vector2(11.5f, 0);
                CreaPatrulla2();
                break;
            default:
                break;
        }
    }

    void CreaPatrulla2()
    {
        // la patrulla para cuando se encuentra arma o muerto es: 

        // x x x
        // x p x
        // x x x

        // donde p es la posición actual (casilla clave)

        for (int i = 0; i < gm.columnas; i++)
        {
            for (int j = 0; j < gm.filas; j++)
            {
                camino[i, j] = false;
            }
        }
        Vector2 aux = new Vector2(casillaClave.x - 1, casillaClave.y - 1);

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if((int)aux.x + i >= 0 && (int)aux.x + i < gm.columnas && (int)aux.y + j >= 0 && (int)aux.y + j < gm.filas)
                    camino[(int)aux.x+i, (int)aux.y+j] = true;
            }
        }
        camino[(int)casillaClave.x, (int)casillaClave.y] = false;
        siguienteCasilla = casillaClave;
    }

    public void Velocity()
    {
        if (velocity < 2) velocity = 4;
        else velocity = 1;
    }
    public void Noche()
    {
        if (started)
        {
            for (int i = 0; i < gm.columnas; i++)
            {
                for (int j = 0; j < gm.filas; j++)
                {
                    if(descubiertas[i, j]) gm.casillas[i, j].GetComponent<SpriteRenderer>().color = Color.white;
                }
            }
        }
    }
}

