using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensoresScript : MonoBehaviour {

	public GameObject izq, dr, arr, ab; // objetos de los 4 sensores
	public float distancia; // distancia que se va a comprobar
	public bool ocupadoIzq, ocupadoDr, ocupadoArr, ocupadoAb;
	// Use this for initialization

	void Start () {
		
	}
	
	// Por cada frame comprobamos si las casillas colindantes están ocupadas o no
	void Update () {
        
		ocupadoIzq = Physics2D.OverlapCircle (izq.transform.position, distancia);
		ocupadoDr = Physics2D.OverlapCircle (dr.transform.position, distancia);
		ocupadoArr = Physics2D.OverlapCircle (arr.transform.position, distancia);
		ocupadoAb = Physics2D.OverlapCircle (ab.transform.position, distancia);
	}
}
