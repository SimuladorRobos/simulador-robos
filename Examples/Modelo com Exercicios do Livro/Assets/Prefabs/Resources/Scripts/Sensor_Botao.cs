/*--------------------------------
 * Sensor_Botao.cs 18/10/2014
 * Definicao: Este script verifica se algum objeto
 * esta na distancia considerada como precionamento
 * de um botao.
 * Uso: Usado no Prefab Botao.
 *-------------------------------- 
*/
using UnityEngine;
using System.Collections;
using System;
using System.IO;
using Random = System.Random;

public class Sensor_Botao : MonoBehaviour{
	public bool estado;   // indica se o botao esta pressionado
	void Start(){}
	//funcao de atualizacao que indicara se o botao esta pressionado
	public void FixedUpdate(){
		float alcance = 0.008f;  //distancia considerada como pressionamento
		Vector3 origem = this.transform.position;
		Vector3 direcao = this.transform.forward; //posicionamento do sensor, possui o mesmo funcionamento do sensor infravermelho
		Vector3 horizontal = this.transform.right;
		Vector3 vertical = this.transform.up;
		origem = origem + direcao.normalized*0.008f;
		RaycastHit hit; 
		Ray myray;
		int i;
		estado = false; //zera a variavel de estado
		//utiliza varios raios de colisao para verificar se o botao esta sendo pressionado:
		for (i = 0; i<=2; i++) {
			Debug.DrawRay(origem+horizontal*(-1+i)*0.0035f,direcao*alcance);
			myray = new Ray (origem+horizontal*(-1+i)*0.0035f, direcao * alcance);  //verifica pressionamento                             //criação do raio
			if (Physics.Raycast (myray, out hit, alcance)) {	
				estado = true;
			}
		}
		for (i = 0; i<=2; i+=2) {
			Debug.DrawRay(origem+vertical*(-1+i)*0.0035f,direcao*alcance);
			myray = new Ray (origem+vertical*(-1+i)*0.0035f, direcao * alcance);   //verifica pressionamento                           //criação do raio
			if (Physics.Raycast (myray, out hit, alcance)) {	
				estado = true;
			}
		}
	}
}

