/*--------------------------------
 * Sensor_Sonar.cs 02/12/2014
 * Definicao: Script que executa a funçao
 * de um sonar, o qual consiste em
 * um equipamento eletronico que detecta
 * a distancia de um objeto a partir do uso
 * de ondas de ultrasom.
 * Uso: Usado no prefab sonar.
 *-------------------------------- 
*/

using UnityEngine;
using System.Collections;
using System;
using System.IO;
using Random = System.Random;

public class Sensor_Sonar : MonoBehaviour 
{
	public float alcance;    //Distancia maxima que o sensor consegue fazer a leitura
	public float raio;		//angulo de abertura dos raios de distancia do sensor
	public int ruido;       //margem de erro relacionado a leitura do sensor
	public float ultima_leitura;	// guarda o ultimo valor lido
	public bool ver_raio;	// variavel que indica se o raio de ilustracao devera ser plotado

	void Start()
	{
	}

	//funcao de atualizacao
	// esta funcao plota um raio se necessario conforme a variavel "ver_raio"
	void FixedUpdate(){
		GetDistancia();
		if (ver_raio){
			GetDistancia();

			Vector3 posicao = transform.position;
			Vector3 direcao = new Vector3(0,0,0); //posicao e direcao do sensor
			RaycastHit hit;

			this.GetComponent<LineRenderer>().enabled = true;	//plota a linha

			direcao = new Vector3(0,0,0);
			direcao += alcance/100*transform.forward;

			Ray myray = new Ray (posicao, direcao);                //cria um raio para verificar colisoes

			float Draio = alcance/100;

			if(Physics.Raycast(myray,out hit,Draio)){
				if(hit.distance < alcance){
					Draio = hit.distance;
				}
			}

			this.GetComponent<LineRenderer>().SetPosition(1,new Vector3(0,0,Draio)); //define tamanho da linha

			int n_linha = 1;
			Transform tr = transform;
			foreach (Transform filho in transform){
				LineRenderer linha = filho.GetComponent<LineRenderer>();
				if(linha != null){
					int x = 0,y = 0;
					switch(n_linha){
						case 1:
							x = 1;y = 0;
						break;
						case 2:
							x = 0;y = 1;
						break;
						case 3:
							x = 0;y = -1;
						break;
						case 4:
							x = -1;y = 0;
						break;
					}
					n_linha++;
					
					direcao = new Vector3(0,0,0);
					direcao += alcance/100*tr.forward;
					direcao += tr.up*y*raio/100;
					direcao += tr.right*x*raio/100;

					Vector3 direcao2 = new Vector3(x*raio/100,y*raio/100,alcance/100);
					
					myray = new Ray (posicao, direcao);                //cria um raio para verificar colisoes
					
					Draio = alcance/100;

					//Debug.DrawRay (posicao, direcao);

					if(Physics.Raycast(myray,out hit,Draio)){
						if(hit.distance < alcance){
							Draio = hit.distance/Draio;
						}
					}else{
						Draio = 1;
					}

					filho.GetComponent<LineRenderer>().enabled = true;	//plota a linha
					filho.GetComponent<LineRenderer>().SetPosition(1,Draio*direcao2); //define tamanho da linha
				}
			}
		}
		else
		{	LineRenderer Linha;
			//this.GetComponent<LineRenderer>().enabled = false; //apaga a linha plotada
			Linha = this.GetComponent<LineRenderer>();
			Linha.enabled = false;
			Linha.SetPosition(1,Vector3.zero);
			foreach (Transform filho in transform){
				LineRenderer linha = filho.GetComponent<LineRenderer>();
				if(linha != null){
					linha.SetPosition(1,Vector3.zero);
					linha.enabled = false;	//plota a linha
				}
			}
		}
		
	}
	//funcao responsavel por fazer a leitura do sensor
	public float GetDistancia() // funcao que será chamada pelo servidor quando o cliente pedir a leitura do sensor
	{
		Random rand = new Random();
		float erro = 0;    // variavel que carrega o erro
		if(ruido != 0)
			erro = (rand.Next()%(ruido))-ruido/2;   //calculo do erro
		erro = erro / 7;

		Vector3 posicao = transform.position;
		Vector3 direcao = new Vector3(0,0,0); //posicao e direcao do sensor
		RaycastHit hit;

		float contadorRay = 0;		//contador que acumula a quantidade de raios de leitura disparados pelo sensor 
		float somaRay = 0;			//soma das leituras de cada raio para no final ralizar uma media

		Ray myray;

		float peso;

		for (double i=0; i < 2*Math.PI; i+=0.1) {		//faz uma rotaçao em torno do eixo

			peso = 0.5f;

			direcao += transform.up * (float) Math.Cos(i) * raio;
			direcao +=  (float) Math.Sin(i) * transform.right * raio;
			direcao +=  alcance*transform.forward;
			myray = new Ray (posicao, direcao/100);                //cria um raio para verificar colisoes
			Debug.DrawRay (posicao, direcao/100);

			if(Physics.Raycast(myray,out hit,alcance/100)){
				if(hit.distance < alcance){
					somaRay += (hit.distance*100)*peso;			//se houve colisao acumula na variavel
					contadorRay += peso;
				}
			}

			peso = 0.75f;

			direcao = new Vector3(0,0,0);
			direcao += transform.up * (float) Math.Cos(i) * (float)(raio*0.7);		//faz outro raio mais interno
			direcao +=  (float) Math.Sin(i) * transform.right * (float) (raio*0.7);
			direcao +=  alcance * transform.forward;
			myray = new Ray (posicao, direcao/100);
			Debug.DrawRay (posicao, direcao/100);

			direcao = new Vector3(0,0,0);

			if(Physics.Raycast(myray,out hit,alcance/100))
			{	
				if(hit.distance < alcance){
					somaRay += (hit.distance*100)*peso;
					contadorRay += peso;
				}	                            //checa se o raio colide e acumula
			}
		}

		peso = 1.0f;

		direcao = new Vector3(0,0,0);
		direcao +=  alcance*transform.forward;
		myray = new Ray (posicao, direcao/100);                //cria um raio para verificar colisoes
		Debug.DrawRay (posicao, direcao/100);
		
		if(Physics.Raycast(myray,out hit,alcance/100)){
			if(hit.distance < alcance){
				somaRay += (hit.distance*100)*peso;			//se houve colisao acumula na variavel
				contadorRay += peso;
			}
		}

		if (contadorRay > 0) {
			ultima_leitura = (somaRay/contadorRay) + erro;
			if(ultima_leitura < 0){
				ultima_leitura = 0;
			}else if(ultima_leitura > alcance){			//corrige a leitura para nao serem retornados valores irreais
				ultima_leitura = alcance;
			}
		}else{
			ultima_leitura = alcance;
		}
		return ultima_leitura;
	}
}

