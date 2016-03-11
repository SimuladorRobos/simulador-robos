/*--------------------------------
 * ServidorPresa.cs
 * Definicao: Script para controle remoto
 * do robo presa, da cena competicao.
 * Uso: Usado na cena Competicao.
 *-------------------------------- 
*/

using UnityEngine;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Sockets;

public class ServidorPresa : MonoBehaviour {
	
	public Servo_Motor_Rotacao motorEsquerdo;
	public Servo_Motor_Rotacao motorDireito;
	public SensorRGB sensorRGB;
	
	private Socket cliente;
	private TcpListener server = new TcpListener(IPAddress.Any, 12599);  
	
	// Use this for initialization
	void Start () {
		server.Start();  // inicia o servidor
	}
	
	//Comandos de movimento-----------------------
	void frente(){
		motorEsquerdo.direcao_rotacao = 0;
		motorDireito.direcao_rotacao = 1;
		andar();
	}
	
	void tras(){
		motorEsquerdo.direcao_rotacao = 1;
		motorDireito.direcao_rotacao = 0;
		andar();
	}
	
	void esquerda(){
		motorEsquerdo.direcao_rotacao = 0;
		motorDireito.direcao_rotacao = 0;
		andar();
	}
	
	void direita(){
		motorEsquerdo.direcao_rotacao = 1;
		motorDireito.direcao_rotacao = 1;
		andar();
	}
	
	void andar(){
		motorEsquerdo.velocidade = 200;
		motorDireito.velocidade = 200;
	}
	
	void parar(){
		motorEsquerdo.velocidade = 0;
		motorDireito.velocidade = 0;
	}
	//-------------------------------------
	
	void Update () {
		int tamanho = 0;  //tamanho da mensagem recebida
		if (server.Pending()){
			cliente = server.AcceptSocket(); //aceita conexao
			cliente.Blocking = false;
			
			byte[] mensagem = new byte[1024];
			string strMessage = "";
			while(!strMessage.Contains (";")){
				try{tamanho = cliente.Receive (mensagem);   //recebe a mensagem
					strMessage = strMessage + System.Text.Encoding.UTF8.GetString(mensagem);
				}catch(System.Exception e){}
			}
			
			string comando = strMessage.Split (';')[0];
			byte[] envioBuffer = new byte[4];
			envioBuffer[0] = (byte)'a';
			envioBuffer[1] = (byte)'c';  // mensagem a ser enviada ao cliente
			envioBuffer[2] = (byte)'k';
			envioBuffer[3] = 10;
			
			//decodifica a mensagem
			if(strMessage.Contains("cor")){
				int cor = sensorRGB.getCor();
				envioBuffer[0] = (byte)((cor >> 16) & 255); // R
				envioBuffer[1] = (byte)((cor >> 8) & 255);  // G
				envioBuffer[2] = (byte)(cor & 255);         // B
			}else if(strMessage.Contains("frente")){
				frente();
			}else if(strMessage.Contains("tras")){
				tras();
			}else if(strMessage.Contains("direita")){
				direita();
			}else if(strMessage.Contains("esquerda")){
				esquerda();
			}else if(strMessage.Contains("parar")){
				parar();
			} 
			cliente.Send(envioBuffer); //responde ao cliente
		}	
	}
}