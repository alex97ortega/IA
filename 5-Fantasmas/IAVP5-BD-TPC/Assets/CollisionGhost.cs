using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Opsive.ThirdPersonController.Wrappers;

public class CollisionGhost : MonoBehaviour {

 
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 21)
        {
            Debug.Log("cosas");
            GetComponent<CharacterHealth>().Damage(2, transform.position, new Vector3(0, 0, 0));
        }
    }
}
