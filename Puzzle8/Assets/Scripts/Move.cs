using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Move : MonoBehaviour {
    GameManger gameManager;
    Index index;
	private void Awake() {
        gameManager = GameObject.Find("Scripts").GetComponent(typeof(GameManger)) as GameManger;
        index = GetComponent<Index>();
    }	

	void OnMouseDown(){
        gameManager.MueveFicha(index.GetIndex());
    }

}
