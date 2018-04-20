using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClickCasilla : MonoBehaviour {


    public GameManager gm;

    void OnMouseDown()
    {
        if (!gm.Comenzado())
        {
            if (transform.position.y != 0 || transform.position.x != 0 && transform.position.x != 1)
            {
                if (gm.maxAliados > 0) gm.CreateAly(transform.position);
                else if (gm.maxEnemigos > 0) gm.CreateEnemy(transform.position);
            }
        }
         
    }
}
