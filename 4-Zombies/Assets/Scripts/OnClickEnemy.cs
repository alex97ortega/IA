using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClickEnemy : MonoBehaviour {

    public GameManager gm;

    void OnMouseDown()
    {
        if (!gm.Comenzado())
        {
            gm.maxEnemigos++;
            if (gm.maxEnemigos == gm.maxE)
                gm.botonComenzar.SetActive(false);
            Destroy(this.gameObject);
        }       
    }
}
