using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClickEnemy : MonoBehaviour {

    public GameManager gm;

    void OnMouseDown()
    {
        gm.maxEnemigos++;
        Destroy(this.gameObject);
    }
}
