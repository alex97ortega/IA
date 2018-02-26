using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour {

	Vector3  posRaton;
	SensoresScript s;
	//puzzle

	private void Awake() {
		s = GetComponentInChildren (typeof(SensoresScript)) as SensoresScript;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		// comprobar si se ha ganado
	}

	void OnMouseDown(){
		
		//posRaton = transform.position - Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y,0));
        Vector3 pos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
        Vector3 aux = pos;
        Vector3 posActual;

        if (!s.ocupadoIzq) posActual = new Vector3(transform.position.x - 3.6f, transform.position.y, 0);
        else if (!s.ocupadoDr) posActual = new Vector3(transform.position.x + 3.6f, transform.position.y, 0);
        else if (!s.ocupadoArr) posActual = new Vector3(transform.position.x, transform.position.y + 3.6f, 0);
        else if (!s.ocupadoAb) posActual = new Vector3(transform.position.x, transform.position.y - 3.6f, 0);
        else return;

        transform.position = posActual;
    }
}
