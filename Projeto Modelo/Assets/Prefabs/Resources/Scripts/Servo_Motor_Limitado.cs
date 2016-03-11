/*--------------------------------
 * Servo_Motor_Limitado.cs 18/10/2014
 * Definicao: Este script tem funçao de rotacionar um objeto arbitrario
 * em torno do eixo Y do objeto que possui este script como componente
 * para uma direcao definida.
 * Uso: Usado no Prefab ServoMotorLimitado.
 *-------------------------------- 
*/
using UnityEngine;
using System.Collections;

public class Servo_Motor_Limitado : MonoBehaviour {
	public int rotacao = 0; //define o angulo que devera esta posicionado
	public GameObject ligado_ao_eixo; //objeto que esta ligado ao eixo e por sua vez sera rotacionado
	private HingeJoint ligacao; //auxiliar do tipo Hinge joint para fazer a rotaçao
	public float forca = 4.0f;
	public float atrito = 0.3f;
	//funcao que inicia o script
	// Esta funcao cria um vinculo entre o objeto ligado ao eixo e o objeto
	//dono script por meio do componente hingejoint 
	void Start () {
		if(ligado_ao_eixo != null){
			this.gameObject.AddComponent<HingeJoint>();
			ligacao = this.GetComponent<HingeJoint>();
			if(ligado_ao_eixo.GetComponent<Rigidbody>() == null) ligado_ao_eixo.AddComponent<Rigidbody>();
			ligacao.connectedBody = ligado_ao_eixo.GetComponent<Rigidbody>();
			ligacao.useSpring = true;
			ligacao.autoConfigureConnectedAnchor = true;		
			ligacao.anchor = new Vector3(0.0f,0.031f,-0.0158f); //ponto de conexao
			ligacao.axis = new Vector3(0,1,0);                  //eixo de rotacao
			JointSpring sprg = new JointSpring();
			sprg.spring = forca;						//força do servo/velocidade de rotacao
			sprg.damper = atrito;						//"atrito"
			ligacao.spring = sprg;
			  //define o objeto conectado
		}
	}
	//funcao de atualizacao
	// esta funcao faz a rotaçao do objeto conectado ao eixo para a direcao descrita em "rotacao"
	void FixedUpdate (){
		Transform horn = this.transform.GetChild (1);
		Vector3 direcao = horn.localEulerAngles;

		if (rotacao > 180 && rotacao < 270)
						rotacao = 180;		//estabelece um limite em 180 grau
		if (rotacao > 270 || rotacao < 0)
						rotacao = 0;		//estabelece um limite em 0 grau
		if (ligado_ao_eixo != null){
			JointSpring sprg = new JointSpring();
			sprg = ligacao.spring;
			sprg.targetPosition = -90 + rotacao;  //faz a rotacao para o angulo requisitado
			ligacao.spring = sprg;
			direcao.y = 90+ligacao.angle;
		}else{
			direcao.y = -180+rotacao;
		}
		horn.localEulerAngles = direcao;
	}
}
