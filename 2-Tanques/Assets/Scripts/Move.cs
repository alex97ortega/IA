using System.Collections;
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

        string s = "Ruta: ";
        foreach (var n in route)
            s += "(" + n.x + ", " + n.y + ") ";
        Debug.Log(s);
    }

    // Update is called once per frame
    void Update()
    {

        
        if (llamar)
        {
            GetComponent<Rigidbody2D>().velocity = CalculateVel(route[routeIndex]);
            llamar = false;
            rotar(route[routeIndex]);
        }
        if (moving) UpdateRoute();
    }
    private Vector2 CalculateVel(Vector2Int target)
    {
        return new Vector2(velocity * (target.x - transform.position.x), velocity * (target.y - transform.position.y));
    }

    void UpdateRoute()
    {

        int x;
        if (GetComponent<Rigidbody2D>().velocity.x >= 0)
            x = (int)transform.position.x;
        else x = (int)transform.position.x + 1;
        int y;
        if (GetComponent<Rigidbody2D>().velocity.y >= 0)
            y= (int)transform.position.y;
        else  y = (int)transform.position.y + 1;
            
        Debug.Log("x:" + x + " y: " + y);
        if (x == route[routeIndex].x && y == route[routeIndex].y) 
        {
                Debug.Log("entro");
                routeIndex++;
            if (routeIndex == route.Count)
            {
                moving = false;
                GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                route = new List<Vector2Int>();
            }
            else
            {
                GetComponent<Rigidbody2D>().velocity = CalculateVel(route[routeIndex]);
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
