using UnityEngine;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Globalization;
public class ServidorExemplo : MonoBehaviour {
	private Socket cliente;
	private TcpListener server = new TcpListener(IPAddress.Any, 12598);
	// Use this for initialization
	void Start () {
		server.Start(); // inicia o servidor
	}
	void Update () {
		int tamanho = 0; //tamanho da mensagem recebida
		if (server.Pending()){
			cliente = server.AcceptSocket(); //aceita conexao
			cliente.Blocking = false;
			byte[] mensagem = new byte[1024];
			string strMessage = "";
			while(!strMessage.Contains (";")){
				try{tamanho = cliente.Receive (mensagem);
					strMessage = strMessage +
						System.Text.Encoding.UTF8.GetString(mensagem);
				}catch(System.Exception e){}
			}
			strMessage = strMessage.Split (';')[0];
			byte[] envioBuffer = new byte[4];
			envioBuffer[0] = (byte)'a';
			envioBuffer[1] = (byte)'c'; // mensagem a ser enviada ao cliente
			envioBuffer[2] = (byte)'k';
			envioBuffer[3] = 10;
			cliente.Send(envioBuffer);
			Servo_Motor_Rotacao [] servos =
				GameObject.FindObjectsOfType<Servo_Motor_Rotacao>();
			for(int i = 0; i < servos.Length; i++)
			{
				if(strMessage.Contains("paraFrente")){
					servos[i].velocidade = 200;
					servos[i].direcao_rotacao = i;
				}else if(strMessage.Contains("paraTras")){
					servos[i].velocidade = 200;
					servos[i].direcao_rotacao = 1-i;
				}else if(strMessage.Contains("parar")){
					servos[i].velocidade = 0;
				}
			}
		}
	}
}
