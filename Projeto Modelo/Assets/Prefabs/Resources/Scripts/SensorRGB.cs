/*--------------------------------
 * SensorRGB.cs 02/12/2014
 * Definicao: Script que executa a funçao
 * de um sensor RGB, o qual consiste em
 * um equipamento eletronico que detecta
 * as cores em tons de vermelho, verde e azul
 * em determinado ponto de seu alcance.
 * Para que este script funcione em texturas
 * eh necessario configura-las, modificando
 * as opcoes de Type para avançada e ativando
 * leitura e escrita
 * Uso: No prefab sensorRGB.
 *-------------------------------- 
*/

using UnityEngine;
using System.Collections;
using System;
using System.IO;
//using UnityEditor;

public class SensorRGB : MonoBehaviour 
{
	private float dist_ultima_leitura;	//variavel auxiliar para plotagem do raio
	public float alcance;    //Distancia que o sensor atinge
	public bool ver_raio;
	public int ultimo_R;
	public int ultimo_G;
	public int ultimo_B;

	void Start(){
		dist_ultima_leitura = alcance;
	}

	void FixedUpdate()
	{
		if (ver_raio){
			getCor();

			this.GetComponent<LineRenderer>().enabled = true;	//plota a linha
			this.GetComponent<LineRenderer>().SetPosition(1,new Vector3(0,0,-(dist_ultima_leitura+0.095f/2))); //define tamanho da linha
		}
		else{	
			LineRenderer linha = this.GetComponent<LineRenderer>();
			linha.enabled = false; //apaga a linha plotada
			linha.SetPosition(1,Vector3.zero);
		}
		getCor ();
	}
 	// funcao responsavel por fazer a leitura da cor retorna um inteiro no formato:
	//				getCor <- 0x00RRGGBB
 	public int getCor() 
 	{
		Vector3 direcao = -transform.forward; 		//direcao do raio de deteccao
		Vector3 posicao = transform.position + direcao * 0.095f/2; //posicao do sensor
 		Ray myray = new Ray(posicao,direcao*alcance/100);                 //criação do raio
  		RaycastHit hit; 
  		int retorno = 0;
		if(Physics.Raycast(myray,out hit,alcance/100))   //ultimo tipo eh o layer antes definido como 1<<8
  		{
			GameObject obj = hit.collider.gameObject;
			dist_ultima_leitura = hit.distance;
			Texture2D texture;
			try{

				texture = (Texture2D)obj.GetComponent<Renderer>().GetComponent<Renderer>().material.GetTexture("_MainTex");	//seleciona a textura do objeto o qual o raio colidiu
				Color C = texture.GetPixelBilinear(hit.textureCoord.x,hit.textureCoord.y);
				retorno += ((int)(C.r*255))<<16;
				retorno += ((int)(C.g*255))<<8;			// transforma as cores para um inteiro para o que possibilita mais facilidade para trasmissao em mensagens de rede 
				retorno += ((int)(C.b*255));
				ultimo_R = (int)(C.r*255);
				ultimo_G = (int)(C.g*255);
				ultimo_B = (int)(C.b*255);
				return retorno;
			}catch(Exception ok){
				MeshRenderer grafico = obj.transform.GetComponent<MeshRenderer>();
				if(grafico != null){
					Color C = grafico.material.color;
					retorno += ((int)(C.r*255))<<16;
					retorno += ((int)(C.g*255))<<8;			// transforma as cores para um inteiro para o que possibilita mais facilidade para trasmissao em mensagens de rede 
					retorno += ((int)(C.b*255));
					ultimo_R = (int)(C.r*255);
					ultimo_G = (int)(C.g*255);
					ultimo_B = (int)(C.b*255);
					return retorno;
				}else{	
					Component[] busca = obj.transform.GetComponentsInChildren<MeshRenderer>();
					foreach (MeshRenderer grafic in busca){
						if(grafic != null){
							Color C = grafic.material.color;
							retorno += ((int)(C.r*255))<<16;
							retorno += ((int)(C.g*255))<<8;			// transforma as cores para um inteiro para o que possibilita mais facilidade para trasmissao em mensagens de rede 
							retorno += ((int)(C.b*255));
							ultimo_R = (int)(C.r*255);
							ultimo_G = (int)(C.g*255);
							ultimo_B = (int)(C.b*255);
							return retorno;
						}
					}
					busca = obj.transform.GetComponentsInParent<MeshRenderer>();
					foreach (MeshRenderer grafic2 in busca){
						if(grafic2 != null){
							Color C = grafic2.material.color;
							retorno += ((int)(C.r*255))<<16;
							retorno += ((int)(C.g*255))<<8;			// transforma as cores para um inteiro para o que possibilita mais facilidade para trasmissao em mensagens de rede 
							retorno += ((int)(C.b*255));
							ultimo_R = (int)(C.r*255);
							ultimo_G = (int)(C.g*255);
							ultimo_B = (int)(C.b*255);
							return retorno;
						}
					}
				}
			}
  		}
  		retorno += (int)(UnityEngine.Random.Range(60,180))<<16;
  		retorno += (int)(UnityEngine.Random.Range(60,180))<<8;	//caso nao tenha realizado leitura atribui uma cor aleatoria
  		retorno += (int)(UnityEngine.Random.Range(60,180));
		ultimo_R = (retorno>>16)&255;
		ultimo_G = (retorno>>8)&255;
		ultimo_B = retorno&255;
		dist_ultima_leitura = alcance/100;
  		return retorno;
 	}
}