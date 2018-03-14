using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public float velocity = 5;

    bool moving = false;

    List<Vector2Int> route = new List<Vector2Int>();

    private int routeIndex;

    public void SetRoute(List<Vector2Int> route_)
    {
        routeIndex = 0;
        route = route_;
        moving = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (moving)
        {
            GetComponent<Rigidbody2D>().velocity = CalculateVel(UpdateRoute());
        }
    }
    private Vector2 CalculateVel(Vector2Int target)
    {
        return new Vector2(velocity * (target.x - transform.position.x), velocity * (target.y - transform.position.y));
    }

    Vector2Int UpdateRoute()
    {
        if (transform.position.x == route[routeIndex].x && transform.position.y == route[routeIndex].y)
        {
            routeIndex++;
            moving = routeIndex != route.Count;
        }
        return route[routeIndex];
    }
}
