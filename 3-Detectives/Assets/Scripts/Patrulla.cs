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
    // el camino va a ser una representación del tablero con booleanos que indican si la casilla es camino o no
    bool[,] camino;
    // necesitamos una matriz que vaya almacenando los tipos de casilla que vaya descubriendo el agente
    IdCasilla.Tipo[,] tablero;

    Vector2 siguienteCasilla;

    public float velocity;

    float vx, vy;
    int posx, posy;
    // Update is called once per frame
    void Update()
    {     
        //toa la movida de siempre
       
            // si unity truncara como es debido no haría falta esta mierda
            int extrax = 0, extray = 0;
            if (vx < 0) extrax = 1;
            else if (vy < 0) extray = 1;

            if (transform.position.x < 1) posx = 0 + extrax;
            else posx = (int)transform.position.x + extrax;

            if (transform.position.y < 1) posy = 0 + extray;
            else posy = (int)transform.position.y+ extray;

            if (vy >= 0 && siguienteCasilla.y > 3 && posx==0) posx++;


        // hacemos cosas dependiendo de la casilla en la que estemos 
        if (gm.casillas[posx, posy].GetComponent<IdCasilla>().GetTipo() == IdCasilla.Tipo.hueco) gm.Muere();
        //movidas impresionantes
        if (gm.casillas[posx, posy].GetComponent<IdCasilla>().GetTipo() == IdCasilla.Tipo.barro) ;
        //buscar cuerpo skrr
        if (gm.casillas[posx, posy].GetComponent<IdCasilla>().GetTipo() == IdCasilla.Tipo.barroSangre ||
                gm.casillas[posx, posy].GetComponent<IdCasilla>().GetTipo() == IdCasilla.Tipo.sangre) ;


            print("siguiente casilla (" + siguienteCasilla.x + " , " + siguienteCasilla.y + ")");
            print("posicion (" + posx+ " , " + posy + ")");

        if (posx == siguienteCasilla.x && posy == siguienteCasilla.y)
            {
                GetComponent<Rigidbody2D>().velocity = CalculateVel();
            }
        
    }

    public void Patrullar()
    {
        siguienteCasilla = new Vector2(0, 0);
        CreaCamino();
        //PrintCamino();
    }
   
    void CreaCamino()
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
        // ponemos el tablero de casillas descubiertas a 0
        tablero = new IdCasilla.Tipo[gm.columnas, gm.filas];

        for (int i = 0; i < gm.columnas; i++)
        {
            for (int j = 0; j < gm.filas; j++)
            {
                tablero[i, j] = IdCasilla.Tipo.normal;
            }
        }
    }


    void PrintCamino()
    {
        Debug.Log(//Menuda inmundicia
            camino[0, 4] + " " + camino[1, 4] + " " + camino[2, 4] + " " + camino[8, 4] + " " + camino[9, 4] + "\n" +
            camino[0, 3] + " " + camino[1, 3] + " " + camino[2, 3] + " " + camino[8, 3] + " " + camino[9, 3] + "\n" +
            camino[0, 2] + " " + camino[1, 2] + " " + camino[2, 2] + " " + camino[8, 2] + " " + camino[9, 2] + "\n" +
            camino[0, 1] + " " + camino[1, 1] + " " + camino[2, 1] + " " + camino[8, 1] + " " + camino[9, 1] + "\n" +
            camino[0, 0] + " " + camino[1, 0] + " " + camino[2, 0] + " " + camino[8, 0] + " " + camino[9, 0] + "\n" 
            );
    }


    Vector2 CalculateVel()
    {
        

        int N = posy + 1;
        int S = posy - 1;
        int W = posx - 1;
        int E = posx + 1;

        bool myS = S > -1 && camino[posx, S];
        bool myN = N < gm.filas && camino[posx, N];
        bool myE = E < gm.columnas && camino[E, posy];
        bool myW = W > -1 && camino[W, posy];

        camino[posx, posy] = false;
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
        return new Vector2(vx,y * vy);
    }
}

