using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Move : MonoBehaviour {

	Vector3  posRaton;
	SensoresScript s;
    UI u;
    CreateGame ga;
    GameManger gameManager;
    Index index;
	private void Awake() {
		s = GetComponentInChildren (typeof(SensoresScript)) as SensoresScript;
        u = GameObject.Find ("Scripts").GetComponent(typeof(UI)) as UI;
        ga = GameObject.Find("Scripts").GetComponent(typeof(CreateGame)) as CreateGame;
        gameManager = GameObject.Find("Scripts").GetComponent(typeof(GameManger)) as GameManger;
        index = GetComponent<Index>();
    }	

	void OnMouseDown(){

        gameManager.MueveFicha(index.GetIndex());
        // sumar movimiento
        u.SumaMov();

        //comprobar si se ha ganado
        gameManager.Victoria();
    }

}
