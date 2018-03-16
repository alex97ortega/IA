using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public float velocity ;

    bool moving = false;
    bool llamar = false;
    List<Vector2Int> route = new List<Vector2Int>();
    public GameManager gm;
    public GameObject t1;
    public GameObject t2;
    private int routeIndex;

    
    public void SetRoute(List<Vector2Int> route_)
    {
         
        routeIndex = 1;
        route = route_;
        moving = true;
        llamar = true;

        //Debug ruta

        /*string s = "Ruta: ";
        foreach (var n in route)
            s += "(" + n.x + ", " + n.y + ") ";
        Debug.Log(s);*/

        // Debug numero de casillas
        Debug.Log("Trazando recorrido de " + (route.Count - 1) + " casillas...");
    }

    // Update is called once per frame
    void Update()
    {

        
        if (llamar) // 1 vez solo
        {
            GetComponent<Rigidbody2D>().velocity = CalculateVel();
            llamar = false;
            Rotar(route[routeIndex]);
        }
        if (moving) UpdateRoute();
    }
    private Vector2 CalculateVel()
    {
        float velAux = velocity;
        if (gm.casillas[route[routeIndex].x, route[routeIndex].y].GetComponent<Index>().index == 2) velAux /= 3;
        return new Vector2(velAux * (route[routeIndex].x - route[routeIndex - 1].x), velAux * (route[routeIndex].y - route[routeIndex - 1].y));
    }

    void UpdateRoute()
    {
        float vx = GetComponent<Rigidbody2D>().velocity.x;
        float vy = GetComponent<Rigidbody2D>().velocity.y;

        int x = 0;
        if(vx < 0)  x = 1;
        x += (int)transform.position.x;
        int y = 0;
        if(vy < 0)  y = 1;
        y += (int)transform.position.y;

        // debug casillas actuales 
        //Debug.Log("x:" + (int)transform.position.x + " y: " + (int)transform.position.y);

        if ((vx > 0 ? transform.position.x >= route[routeIndex].x : transform.position.x <= route[routeIndex].x) &&
            (vy > 0 ? transform.position.y >= route[routeIndex].y : transform.position.y <= route[routeIndex].y)
            || (x == route[routeIndex].x && y == route[routeIndex].y) ||
            (vx < 0 && vy == 0 && Math.Floor(transform.position.x) == -1) ||
            (vy < 0 && vx == 0 && Math.Floor(transform.position.y) == -1))
 
        {

                routeIndex++;
            // termina recorrido si llegó al final o hay obstáculo nuevo
            if (routeIndex == route.Count 
                || gm.casillas[route[routeIndex].x, route[routeIndex].y].GetComponent<Index>().index == 0)
                
            {

                Parar();
            }
            /*else if ((int)t1.transform.position.x == route[routeIndex].x && 
                (int)t1.transform.position.y == route[routeIndex].y ) 
            {
                t1.gameObject.GetComponent<Move>().Parar();
                Parar();
            }
            else if ((int)t2.transform.position.x == route[routeIndex].x && 
                (int)t1.transform.position.y == route[routeIndex].y)
            {
                t2.gameObject.GetComponent<Move>().Parar();
                Parar();
            }*/
            else
            {
                GetComponent<Rigidbody2D>().velocity = CalculateVel();
                Rotar(route[routeIndex]);
            }
                
        }
    }
    public void Parar()
    {
        moving = false;
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        route = new List<Vector2Int>();
    }
    void Rotar(Vector2Int o)
    {
        Vector3 objetivo = new Vector3(transform.position.x - o.x, transform.position.y- o.y, 0);
        
        transform.rotation =   Quaternion.LookRotation(objetivo);
        transform.eulerAngles = new Vector3(0f, 0f, transform.eulerAngles.x+90f);
    }
 
}
