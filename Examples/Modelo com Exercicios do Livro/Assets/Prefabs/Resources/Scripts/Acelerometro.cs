/*--------------------------------
 * Acelerometro.cs 20/01/2015
 * Definicao: Este script descreve o
 * funcionamento de um acelerometro
 * calculando a diferença de velocidades
 * em relação a variacao do tempo.
 * Uso: Usado no Prefab Acelerometro.
 *-------------------------------- 
*/

using UnityEngine;
using System.Collections;

public class Acelerometro : MonoBehaviour {
	private float lastVX;
	private float lastVY;  //Ultimas velocidades registradas
	private float lastVZ;

	public float aceleracaoX;
	public float aceleracaoY;  //Aceleracao atual
	public float aceleracaoZ;
	public float valorMaximo = 10.0f; //Limite de aceleracao do componente

	private Transform nucleo; //Objeto da hierarquia do acelerometro, usado para calcular a aceleracao

	// Use this for initialization
	void Start () {
		foreach (Transform filho in transform) {
			if(filho.name == "AcelNucleo"){
				nucleo = filho;
			}
		}
	}

	float normalizar(float valor){
		if(Mathf.Abs (valor) > valorMaximo) return (valor / Mathf.Abs (valor) * valorMaximo); 
		return valor;
	}

	// Fica atualizando a aceleracao
	void FixedUpdate () {
	    lastVX = nucleo.GetComponent<Rigidbody>().velocity.x - lastVX;
		lastVY = nucleo.GetComponent<Rigidbody>().velocity.y - lastVY; //Armazena o diferencial de velocidade
		lastVZ = nucleo.GetComponent<Rigidbody>().velocity.z - lastVZ;

		Vector3 acel = (new Vector3 (lastVX, lastVY, lastVZ));
		acel = this.transform.worldToLocalMatrix * acel;

		aceleracaoX = acel.x/(Time.deltaTime*980.0f);
		aceleracaoY = acel.y/(Time.deltaTime*980.0f); //divide pelo tempo diferencial
		aceleracaoZ = acel.z/(Time.deltaTime*980.0f);

		aceleracaoX = normalizar (aceleracaoX);
		aceleracaoY = normalizar (aceleracaoY);
		aceleracaoZ = normalizar (aceleracaoZ);

		lastVX = this.GetComponent<Rigidbody>().velocity.x;
		lastVY = this.GetComponent<Rigidbody>().velocity.y;
		lastVZ = this.GetComponent<Rigidbody>().velocity.z;

		nucleo.position = this.transform.position;                 // Reposiciona o nucleo
		nucleo.GetComponent<Rigidbody>().velocity = this.GetComponent<Rigidbody>().velocity;
	}
}
