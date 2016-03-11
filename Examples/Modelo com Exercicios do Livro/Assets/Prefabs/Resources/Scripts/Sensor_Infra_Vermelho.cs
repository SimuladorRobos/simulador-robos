/*--------------------------------
 * Sensor_Infra_Vermelho.cs 18/10/2014
 * Definicao: Este script tem funçao de detectar
 * a distancia em centimetros do objeto mais proximo
 * na direcao do objeto que possui este script.
 * Uso: Usado no Prefab SensorIV.
 *-------------------------------- 
*/

using UnityEngine;
using System.Collections;
using System;
using System.IO;
using Random = System.Random;

public class Sensor_Infra_Vermelho : MonoBehaviour{
	public float alcance;    //Alcance do sensor
	public int ruido;        //Erro introduzido a leitura
	public bool ver_raio = false;   //Indicador da necessidade de plotar um raio para visualizacao 
	public float ultima_leitura;	//ultimo valor de distancia lido
	void Start(){}
	//funcao de atualizacao
	// esta funcao plota um raio se necessario conforme a variavel "ver_raio"
	void Update(){
		if (ver_raio){
			GetDistancia();
			this.GetComponent<LineRenderer>().enabled = true;	//plota a linha
			this.GetComponent<LineRenderer>().SetPosition(1,new Vector3(0,0,ultima_leitura/100.0f)); //define tamanho da linha
		}
		else{	
			LineRenderer linha = this.GetComponent<LineRenderer>();
			linha.enabled = false; //apaga a linha plotada
			linha.SetPosition(1,Vector3.zero);
		}
	}

	//funçao responsavel por medir a distancia do objeto a frente
	public float GetDistancia(){
		Random rand = new Random();
		float erro = 0;    // variavel que carrega o erro
		if(ruido != 0)
			erro = (rand.Next()%(ruido))-ruido/2;   //calculo do erro
		erro = erro / 7;
		Vector3 origem = this.transform.position;
		Vector3 direcao = this.transform.forward;  //posicionamento e direcionamento do sensor
		origem = origem + direcao.normalized*0.01f;
		Ray myray = new Ray(origem,direcao*alcance/100.0f); //criação do raio que verificara a colisao
		RaycastHit hit; 
		if(Physics.Raycast(myray,out hit,alcance/100)) //em caso de colisao
		{
			if(hit.distance*100+erro>alcance)
			{	ultima_leitura = alcance;   //caso a distancia + erro ultrapasse o alcance
				return alcance;
			}
			else if(hit.distance*100+erro<0)
			{	ultima_leitura = 0;		    //caso esteja muito proximo
				return 0;
			}
			else
			{	ultima_leitura = (hit.distance*100+erro);
				return ultima_leitura;		//retorna distancia aplicando o erro
			}
		}
		ultima_leitura = alcance; //nao ha objeto a frente
		return alcance;
	}
}