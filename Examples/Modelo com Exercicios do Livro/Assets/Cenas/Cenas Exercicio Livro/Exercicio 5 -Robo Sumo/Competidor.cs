/*--------------------------------
 * Competidor.cs ??/??/2015
 * Definicao: Este script configura
 * o comportamento de um competidor
 * no robo sumo.
 * Uso: Usado na cena Competicao.
 *-------------------------------- 
*/

using UnityEngine;
using System.Collections;

public class Competidor : MonoBehaviour {
	static int numeroCompetidores = 0;
	int identificador;
	Vector3 posicaoAnterior;
	float tempoParado;
	bool desclassificado = false;

	// Use this for initialization
	void Start () {
		identificador = numeroCompetidores;
		numeroCompetidores++;
	}


	void desclassificar(){
		if(numeroCompetidores > 1){
			desclassificado = true;
			numeroCompetidores--;
			print("Desclassificado");
		}
	}

	//Executa as regras do robo sumo
	void Update () {
		if (posicaoAnterior == transform.position) {
			tempoParado += Time.deltaTime;
		}else{
			tempoParado = 0.0f;
		}
		if (tempoParado >= 10) {
			desclassificar();
		}
		posicaoAnterior = transform.position;
		Vector2 posicaoPlanar = new Vector2 (posicaoAnterior.x, posicaoAnterior.z);
		if (posicaoPlanar.magnitude > 0.81) {
			desclassificar ();
		}
	}
}
