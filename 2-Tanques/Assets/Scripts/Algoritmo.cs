using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Algoritmo : MonoBehaviour {

	public bool calcularRuta(GameObject tanque, Vector3 destino, GameObject[,] estadoTablero,
        Vector3 posOther1, Vector3 posOther2)
    {
        bool haySolucion = true;
        bool[] marcados = new bool[estadoTablero.Length] ;
        
        for (int i = 0; i < marcados.Length; i++) marcados[i] = false;

        // marcar tanques

        marcados[(int)posOther1.x * 10 + (int)posOther1.y] = true;
        marcados[(int)posOther2.x * 10 + (int)posOther2.y] = true;

        // marcar muros
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                if (estadoTablero[i, j].GetComponent<Index>().getIndex() == 0)
                    marcados[i * 10 + j] = true;
            }
        }

        // tenemos que traducir esto a nuestro tablero, a parte de buscar el camino mínimo
        // tenemos que guardarlo para que pueda hacer el recorrido el tanque y eso es 
        //lo que no sé como hacerlo
        /* int getCamino() const {

            return distTo[salida.first * C + salida.second];
        }
        // dfs
       std::queue<size_t> q;
        marcado[i * C + j] = true;
        q.push(i * C + j);
        while (!q.empty())
        {
            auto v = q.front(); q.pop();
            for (Dir d : dirs)
            {
                int ni = v / C + d.first;
                int nj = v % C + d.second;
                if (dentro(ni, nj))
                {
                    if (!marcado[ni * C + nj])
                    {
                        distTo[ni * C + nj] = distTo[v] + 1;
                        marcado[ni * C + nj] = true;
                        q.push(ni * C + nj);
                    }
                }//dentro
            }//dirs
        }//q.empty*/


        return haySolucion;
    }
}
