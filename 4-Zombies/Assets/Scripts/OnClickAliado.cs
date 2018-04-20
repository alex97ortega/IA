using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClickAliado : MonoBehaviour {
    public GameManager gm;

    void OnMouseDown()
    {
        if (gm.maxEnemigos > 0) gm.CreateEnemy(transform.position);
        gm.maxAliados++;
        Destroy(this.gameObject);
    }
                
}
