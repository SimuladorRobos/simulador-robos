/*--------------------------------
 * Servo_Motor_Rotacao.cs 18/10/2014
 * Definicao: Este script tem funçao de rotacionar um objeto arbitrario
 * em torno do eixo Y do objeto que possui este script como componente.
 * Uso: Usado no Prefab ServoMotorRotacao.
 *-------------------------------- 
*/
using UnityEngine;
using System.Collections;

public class Servo_Motor_Rotacao : MonoBehaviour {
	public float velocidade = 5.0f;  //define a velocidade de rotaçao
	public float forca = 1.0f;	 	 //define a forca
	public int direcao_rotacao = 1;  //define o sentido de rotacao (1: horario, 0: antihorario)
	public GameObject ligado_ao_eixo;  //objeto que esta ligado ao eixo e por sua vez sera rotacionado
	private HingeJoint ligacao;  //auxiliar do tipo Hinge joint para fazer a rotaçao

	//funcao que inicia o script
	// Essa funcao cria um vinculo entre o objeto ligado ao eixo e o objeto
	//dono script por meio do componente hingejoint 
	void Start () {
		if(ligado_ao_eixo != null){
			this.gameObject.AddComponent<HingeJoint>();
			ligacao = this.GetComponent<HingeJoint>();
			if(ligado_ao_eixo.GetComponent<Rigidbody>() == null) ligado_ao_eixo.AddComponent<Rigidbody>();
			ligacao.connectedBody = ligado_ao_eixo.GetComponent<Rigidbody>(); //define o objeto conectado
			ligacao.useMotor = true;
			ligacao.autoConfigureConnectedAnchor = true;		
			ligacao.anchor = new Vector3(0.0f,0.031f,-0.0158f);  //ponto de conexao
			ligacao.axis = new Vector3(0,1,0);                   //eixo de rotacao
			ligacao.motor = new JointMotor();
		}
	}
	//funcao de atualizacao
	// esta funcao faz a rotaçao do objeto conectado ao eixo a partir do HingeJoint "ligacao"
	void FixedUpdate (){
		Transform horn = this.transform.GetChild (1);
		Vector3 direcao = horn.localEulerAngles;
		if (velocidade < 0.0f)
						velocidade = 0f;   //nao permite velocidades negativas
		if ((velocidade >= 0.0f) && ligado_ao_eixo != null)
		{
			float rotacao = ligacao.angle;   //muda o angulo do hinge joint para forçar a rotaçao
			JointMotor motor = ligacao.motor;
			motor.force = forca;
			if(direcao_rotacao == 1) motor.targetVelocity = velocidade;
			else motor.targetVelocity = -velocidade;
			ligacao.motor = motor;
			direcao.y = -ligacao.angle;
		}
		horn.localEulerAngles = direcao;
	}
}
