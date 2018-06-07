using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MouseDetection : MonoBehaviour {

    
    public GameObject npc;
    void OnMouseDown()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            npc.GetComponent<NavMeshAgent>().SetDestination(hit.point);
        }
    }
}
