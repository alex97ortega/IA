using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Este Script es llamado en cuanto se descubren el Arma y el Muerto.
// Le llega del Script Algoritmo el camino a seguir desde su posición hasta su casa y simplemente se encarga de seguirlo

public class VueltaAcasa : MonoBehaviour {

    public Algoritmo alg;
    List<Vector2Int> route = new List<Vector2Int>();
    bool moving = false;
    bool llamar = false;

    float velocity;
    int routeIndex;
    float vx, vy, posx,posy;

    bool arriba, dch;
    // Use this for initialization

    public void Volver(Vector2Int pos, int[,] tab, float vel,bool a,bool d) {
        moving = true;
        llamar = true;
        velocity = vel;
        routeIndex = 1;

        arriba = a;
        dch = d;

        route = alg.CalcularRuta(pos,tab);

        /*string s = "Ruta: ";
        foreach (var n in route)
            s += "(" + n.x + ", " + n.y + ") ";
        Debug.Log(s);*/

        Debug.Log("Trazando recorrido de " + (route.Count - 1) + " casillas...");
    }

    void Update()
    {
        if (llamar) // 1 vez solo
        {
            GetComponent<Rigidbody2D>().velocity = CalculateVel();
            llamar = false;
            
        }
        if (moving) UpdateRoute();
    }
    private Vector2 CalculateVel()
    {
        return new Vector2(velocity * (route[routeIndex].x - route[routeIndex - 1].x),
            velocity * (route[routeIndex].y - route[routeIndex - 1].y));
    }

    void UpdateRoute()
    {
         vx = GetComponent<Rigidbody2D>().velocity.x;
         vy = GetComponent<Rigidbody2D>().velocity.y;
        DamePos();
        if (posx == route[routeIndex].x && posy == route[routeIndex].y)

        {
            routeIndex++;
            // termina recorrido si llegó al final o hay obstáculo nuevo
            if (routeIndex == route.Count)   Parar();          
          
            else     
                GetComponent<Rigidbody2D>().velocity = CalculateVel();                  
        }
    }
    void DamePos()
    {
        //toa la movida de siempre

        if (vy < 0) arriba = false;
        else if (vy > 0) arriba = true;

        if (arriba) posy = (int)transform.position.y;
        else posy = (int)Mathf.Round(transform.position.y + 0.5f);


        if (vx < 0) dch = false;
        else if (vx > 0) dch = true;

        if (dch) posx = (int)transform.position.x;
        else posx = (int)Mathf.Round(transform.position.x + 0.5f);

        //print("posicion (" + posx + " , " + posy + ")");
    }
    public void Parar()
    {
        moving = false;
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        route = new List<Vector2Int>();
    }
    public void Velocity()
    {
        if (velocity < 2) velocity = 4;
        else velocity = 1;
    }    
}
