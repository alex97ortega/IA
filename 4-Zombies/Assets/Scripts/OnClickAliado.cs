using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClickAliado : MonoBehaviour {
    public GameManager gm;

    void OnMouseDown()
    {
        if (!gm.Comenzado())
        {
            if (gm.numeroEnemigos > 0) gm.CreateEnemy(transform.position);
            gm.numeroAliados++;
            Destroy(this.gameObject);
        }       
    }
                
}
