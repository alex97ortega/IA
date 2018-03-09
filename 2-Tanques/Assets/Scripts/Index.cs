﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Index : MonoBehaviour {

    public uint index; // 0 -> tierra 1-> barro 2-> piedra
    public uint getIndex() { return index; }

    public Sprite arena;
    public Sprite barro;
    public Sprite piedra;
    
    public GameManager gm;
    private void Start()
    {
        GetComponent<SpriteRenderer>().sprite = arena;
    }
    // Update is called once per frame
    void  OnMouseDown()
    {
        if (gm.nadaSeleccionado)
        {
            index++;
            index %= 3;

            switch (index)
            {
                case 0:
                    GetComponent<SpriteRenderer>().sprite = piedra;
				    transform.gameObject.layer = 11;
                    break;
                case 1:
                    GetComponent<SpriteRenderer>().sprite = arena;
					transform.gameObject.layer = 10;
                    break;
                default:
                    GetComponent<SpriteRenderer>().sprite = barro;
                    break;
            }
        }
        else if (index == 0) gm.SeleccionarVacio();

        else  gm.PonerCruz(transform.position);
        
	}
}
