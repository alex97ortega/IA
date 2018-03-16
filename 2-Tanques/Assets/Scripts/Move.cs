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

    private int routeIndex;

    public void SetRoute(List<Vector2Int> route_)
    {
         
        routeIndex = 1;
        route = route_;
        moving = true;
        llamar = true;
        //Debug
        string s = "Ruta: ";
        foreach (var n in route)
            s += "(" + n.x + ", " + n.y + ") ";
        Debug.Log(s);
    }

    // Update is called once per frame
    void Update()
    {

        
        if (llamar) // 1 vez solo
        {
            GetComponent<Rigidbody2D>().velocity = CalculateVel();
            llamar = false;
            rotar(route[routeIndex]);
        }
        if (moving) UpdateRoute();
    }
    private Vector2 CalculateVel()
    {
        return new Vector2(velocity * (route[routeIndex].x - route[routeIndex - 1].x), velocity * (route[routeIndex].y - route[routeIndex - 1].y));
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


        Debug.Log("x:" + (int)transform.position.x + " y: " + (int)transform.position.y);

        if ((vx > 0 ? transform.position.x >= route[routeIndex].x : transform.position.x <= route[routeIndex].x) &&
            (vy > 0 ? transform.position.y >= route[routeIndex].y : transform.position.y <= route[routeIndex].y)
            || (x == route[routeIndex].x && y == route[routeIndex].y) ||
            (vx < 0 && vy == 0 && Math.Floor(transform.position.x) == -1) ||
            (vy < 0 && vx == 0 && Math.Floor(transform.position.y) == -1))
 
        {

                routeIndex++;
            if (routeIndex == route.Count)
            {

                moving = false;
                GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                route = new List<Vector2Int>();
            }
            else
            {
                GetComponent<Rigidbody2D>().velocity = CalculateVel();
                rotar(route[routeIndex]);
            }
                
        }
    }
    void rotar(Vector2Int o)
    {
        Vector3 objetivo = new Vector3(transform.position.x - o.x, transform.position.y- o.y, 0);
        
        transform.rotation =   Quaternion.LookRotation(objetivo);
        transform.eulerAngles = new Vector3(0f, 0f, transform.eulerAngles.x+90f);
    }
}
