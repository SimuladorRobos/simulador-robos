/*--------------------------------
 * ServidorPredador.cs
 * Definicao: Script para controle remoto
 * do robo predador, da cena competicao.
 * Uso: Usado na cena Competicao.
 *-------------------------------- 
*/

using UnityEngine;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Sockets;

public class ServidorPredador : MonoBehaviour {
	
	public Servo_Motor_Rotacao motorEsquerdoFrontal;
	public Servo_Motor_Rotacao motorDireitoFrontal;
	public Servo_Motor_Rotacao motorEsquerdoTraseiro;  //partes do robo
	public Servo_Motor_Rotacao motorDireitoTraseiro;   //devem ser definidas no Unity
	public SensorRGB sensorRGB;
	public Sensor_Infra_Vermelho sensorIVEsquerdo;
	public Sensor_Infra_Vermelho sensorIVCentral;
	public Sensor_Infra_Vermelho sensorIVDireito;

	private Socket cliente;
	private TcpListener server = new TcpListener(IPAddress.Any, 12598);  
	
	// Use this for initialization
	void Start () {
		server.Start();  // inicia o servidor
	}

	//Comandos de movimento-----------------------
	void frente(){
		motorEsquerdoFrontal.direcao_rotacao = 0;
		motorDireitoFrontal.direcao_rotacao = 1;
		motorEsquerdoTraseiro.direcao_rotacao = 0;
		motorDireitoTraseiro.direcao_rotacao = 1;
		andar();
	}

	void tras(){
		motorEsquerdoFrontal.direcao_rotacao = 1;
		motorDireitoFrontal.direcao_rotacao = 0;
		motorEsquerdoTraseiro.direcao_rotacao = 1;
		motorDireitoTraseiro.direcao_rotacao = 0;
		andar();
	}

	void esquerda(){
		motorEsquerdoFrontal.direcao_rotacao = 1;
		motorDireitoFrontal.direcao_rotacao = 1;
		motorEsquerdoTraseiro.direcao_rotacao = 1;
		motorDireitoTraseiro.direcao_rotacao = 1;
		andar();
	}

	void direita(){
		motorEsquerdoFrontal.direcao_rotacao = 0;
		motorDireitoFrontal.direcao_rotacao = 0;
		motorEsquerdoTraseiro.direcao_rotacao = 0;
		motorDireitoTraseiro.direcao_rotacao = 0;
		andar();
	}

	void andar(){
		motorEsquerdoFrontal.velocidade = 300;
		motorDireitoFrontal.velocidade = 300;
		motorEsquerdoTraseiro.velocidade = 300;
		motorDireitoTraseiro.velocidade = 300;
	}

	void parar(){
		motorEsquerdoFrontal.velocidade = 0;
		motorDireitoFrontal.velocidade = 0;
		motorEsquerdoTraseiro.velocidade = 0;
		motorDireitoTraseiro.velocidade = 0;
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
			}else if(strMessage.Contains("distanciaE")){
				float distancia = sensorIVEsquerdo.GetDistancia();
				envioBuffer[0] = (byte)distancia;
				envioBuffer[1] = 10;
			}else if(strMessage.Contains("distanciaC")){
				float distancia = sensorIVCentral.GetDistancia();
				envioBuffer[0] = (byte)distancia;
				envioBuffer[1] = 10;
			}else if(strMessage.Contains("distanciaD")){
				float distancia = sensorIVDireito.GetDistancia();
				envioBuffer[0] = (byte)distancia;
				envioBuffer[1] = 10;
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