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
   en cuanto toque una casilla de sangre.

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
    int[,] descubiertas;

    Vector2 siguienteCasilla;
    Vector2 casillaClave;
    public float velocity;

    float vx, vy;
    int posx, posy;
    int N, S, E, W;
    // los distintos modos de moverse del agente los tomo en un enumerado
    enum Modo { Patrullando, tocadoSangre, tocadoBarro, encontradoArma, encontradoMuerto, Paro}
    enum Dir { N, S, E, W }
    Modo m;

    bool n = true;
    bool s = true;
    bool e = true;
    bool w = true;

    bool change;
    // para controlar la posición
    bool arriba = false;
    bool dch = false;
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
            case Modo.tocadoBarro:
                PatrullaBarro();
                break;
            case Modo.Paro:
                
                if(!llamado) {
                    // el otro algoritmo
                    GetComponent<VueltaAcasa>().Volver(new Vector2Int(posx, posy), descubiertas, velocity, arriba, dch);
                    llamado = true;
                }                   
              
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
        change = true;
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
        descubiertas = new int[gm.columnas, gm.filas];

        for (int i = 0; i < gm.columnas; i++)
        {
            for (int j = 0; j < gm.filas; j++)
            {
                descubiertas[i, j] = 0;
            }
        }
        descubiertas[0, 0] = 1;
    }

    // Update
    void DamePos()
    {
        //toa la movida de siempre
        if (change)
        {

            
            if (vy < 0) arriba = false;
            else if (vy > 0) arriba = true;

            if (arriba) posy = (int)transform.position.y;
            else posy = (int)Mathf.Round(transform.position.y + 0.5f);

            
            if (vx < 0) dch = false;
            else if (vx > 0) dch = true;

            if (dch) posx = (int)transform.position.x;
            else posx = (int)Mathf.Round(transform.position.x + 0.5f);

            // esto no debería ser necesario :(
            if (posx < 0) posx = 0;
            else if (posx == gm.columnas) posx = gm.columnas - 1;
            if (posy < 0) posy = 0;
            else if (posy == gm.filas) posy = gm.filas - 1;

            N = posy + 1;
            S = posy - 1;
            W = posx - 1;
            E = posx + 1;

            print("posicion (" + posx + " , " + posy + ")");
        }
        else change = true;
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

        //buscar cuerpo o arma si se encuentra uno de los dos (en un área de 3x3)
        if (posx == arma.transform.position.x && posy == arma.transform.position.y)
            CambiarModo(Modo.encontradoArma, new Vector2(posx, posy));

        //movidas impresionantes si hay barro
        else if (gm.casillas[posx, posy].GetComponent<IdCasilla>().GetTipo() == IdCasilla.Tipo.barro)
        {
            CambiarModo(Modo.tocadoBarro, new Vector2(posx, posy));
        }

        //buscar cuerpo si hay sangre (en 4 direcciones)
        else if (gm.casillas[posx, posy].GetComponent<IdCasilla>().GetTipo() == IdCasilla.Tipo.barroSangre ||
                gm.casillas[posx, posy].GetComponent<IdCasilla>().GetTipo() == IdCasilla.Tipo.sangre)
            CambiarModo(Modo.tocadoSangre, new Vector2(posx, posy));

       
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

    void PatrullaBarro()
    {
        if (posx == siguienteCasilla.x && posy == siguienteCasilla.y)
        {
            bool tocoBarro = gm.casillas[posx, posy].GetComponent<IdCasilla>().GetTipo() == IdCasilla.Tipo.barro;
            GetComponent<Rigidbody2D>().velocity = CalculateVel3(tocoBarro);
        }
        
        else if (gm.casillas[posx, posy].GetComponent<IdCasilla>().GetTipo() == IdCasilla.Tipo.barroSangre ||
                gm.casillas[posx, posy].GetComponent<IdCasilla>().GetTipo() == IdCasilla.Tipo.sangre)
            CambiarModo(Modo.tocadoSangre, new Vector2(posx, posy));
        
        if (posx == arma.transform.position.x && posy == arma.transform.position.y)
            CambiarModo(Modo.encontradoArma, new Vector2(posx, posy));
    }
    //patrulla
    Vector2 CalculateVel()
    {
        
        bool myS = S > -1 && camino[posx, S];
        bool myN = N < gm.filas && camino[posx, N];
        bool myE = E < gm.columnas && camino[E, posy] ;
        bool myW = W > -1 && camino[W, posy];

        //camino[posx, posy] = false;
        if (arriba && vy >0) myS = false;
        else if(!arriba && vy<0)myN = false;
        if (dch && vx>0) myW = false;
        else if(!dch && vx<0)myE = false;
        return DameVel(myS, myN, myE, myW);
    }

    //sangre
    Vector2 CalculateVel2()
    {
        // el recorrido para cuando se encuentra sangre es: 

        // □ x □
        // x p x
        // □ x □

        // donde p es la posición actual
               

        bool myS = S > -1 && s && descubiertas[posx, S] ==0 ;
        bool myN = N < gm.filas && n && descubiertas[posx, N] == 0;
        bool myE = E < gm.columnas && e && descubiertas[E, posy]==0;
        bool myW = W > -1 && w && descubiertas[W, posy]==0;

        return DameVel(myS, myN, myE, myW);
    }

    //barro
    Vector2 CalculateVel3(bool esBarro)
    {
        int aux = 1;
        bool myS, myN, myE, myW;
        if (esBarro)
        {
            aux = Random.Range(0, 8 - descubiertas[posx, posy]);
            if (aux == 0)
            {
                myS = S > -1;
                myN = N < gm.filas;
                myE = E < gm.columnas;
                myW = W > -1;
            }
            else
            {
                myS = S > -1 && descubiertas[posx, S] > 0;

                myN = N < gm.filas && descubiertas[posx, N] > 0;
                myE = E < gm.columnas && descubiertas[E, posy] > 0;
                myW = W > -1 && descubiertas[W, posy] > 0;
            }
        }
        else
        {
            myS = S > -1;
            myN = N < gm.filas;
            myE = E < gm.columnas;
            myW = W > -1;

            // da mas importancia a las casillas sin descubrir
            if (myE && descubiertas[E, posy] == 0) myS = myN = myW = false;
            else if (myN && descubiertas[posx, N] == 0) myS = myE = myW = false;
            else if (myS && descubiertas[posx, S] == 0) myN = myE = myW = false;
            else if (myW && descubiertas[W, posy] == 0) myS = myN = myE = false;

            // si no, que no haga bucle infinito con un rand 
            else aux = 0;         
           
        }

        if ((!myS && !myN && !myE && !myW) ||aux==0) {
            bool sePuede = false ;
            do
            {
                switch (Random.Range(0, 4))
                {
                    case 0:
                        if (S > -1)
                        {
                            myS = true;
                            myN = myE = myW = false;
                            sePuede = true;
                        }
                        break;
                    case 1:
                        if (N < gm.filas)
                        {
                            myN = true;
                            myS = myE = myW = false;
                            sePuede = true;
                        }
                        break;
                    case 2:
                        if (E < gm.columnas)
                        {
                            myE = true;
                            myN = myS = myW = false;
                            sePuede = true;
                        }
                        break;
                    default:
                        if (W > -1)
                        {
                            myW = true;
                            myN = myE = myS = false;
                            sePuede = true;
                        }
                        break;
                }

            } while (!sePuede);

        }
        return DameVel(myS,  myN,  myE,  myW);
    }

    Vector2 DameVel(bool myS, bool myN, bool myE, bool myW)
    {
        gm.casillas[posx, posy].GetComponent<SpriteRenderer>().color = Color.white;
        descubiertas[posx, posy]++;
        int x = 0, y = 0;
         if (myE)
        {
            //print("E");
            x = 1;
            y = 0;
            siguienteCasilla.x++;
            e = false;
        }
        else if (myN)
        {
            //print("N");
            x = 0;
            y = 1;
            siguienteCasilla.y++;
            n = false;
        }
        
        else if (myS)
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
        
        

        vx = x * velocity;
        vy = y * velocity;
        change = false;
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
            case Modo.tocadoBarro:
                siguienteCasilla = casillaClave;
                n = s = w = e = true;
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
                    if(descubiertas[i, j]>0) gm.casillas[i, j].GetComponent<SpriteRenderer>().color = Color.white;
                }
            }
        }
    }
}

