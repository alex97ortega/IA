using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HillClibing_Resolutor : MonoBehaviour {
    List<Vector2[]> list;
    List<Vector2[]> seen;

    public Vector2[] currentState;

    Vector2[] solvedState = new Vector2[9];

    struct Direction{public int i; public int j;};

    Direction[] directions = new Direction[4];

    void Start () {
        //SolvedState initialization
        int n = 0;
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                solvedState[n] = new Vector2(i, j);
                n++;
            }
        }

        list.Add(currentState);
        //Directions initialization
        directions[0].i = -1; directions[0].j = 0;
        directions[1].i = 1; directions[1].j = 0;
        directions[2].i = 0; directions[1].j = -1;
        directions[3].j = 0; directions[3].j = 1;
    }
	
    void Resolve()
    {
        while (!CheckCorrectState(list[0]))
        {
            for (int i = 0; i < directions.Length; i++)
            {
                Vector2[] newState = ApplyMove(directions[i], list[0]);
                if (newState == null) continue;
                if (!seen.Contains(newState))
                    list.Add(newState);
            }
            SortListByProximity(ref list);
        }
    }

    struct PonderedState { public int priority; public Vector2[] state; }
    private void SortListByProximity(ref List<Vector2[]> list)
    {
        List<PonderedState> ponderedState = new List<PonderedState>();
        foreach(Vector2[] state in list)
        {
            PonderedState ps;
            ps.state = state;
            ps.priority = GetPriority(state);
            ponderedState.Add(ps);
        }
    }


    private int GetPriority(Vector2[] state)
    {
        throw new NotImplementedException();
    }

    bool CheckCorrectState(Vector2[] state)
    {
        bool res = true;
        for (int i = 0; res && i < solvedState.Length; i++)
        {
            if (state[i] != solvedState[i])
                res = false;
            
        }
       return res;
    }

    Vector2[] ApplyMove(Direction dir, Vector2[] state)
    {
        //TODO
        bool a = true;
        Vector2[] newState = new Vector2[9];
        if (a)
        return newState;
        return null;
    }


}
