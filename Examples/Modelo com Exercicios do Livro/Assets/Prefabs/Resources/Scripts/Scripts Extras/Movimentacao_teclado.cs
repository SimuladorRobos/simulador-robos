/*--------------------------------
 * Movimentacao_teclado.cs 26/10/2014
 * Definicao: Script utilitario para posicionamento
 * e movimentacao de objetos, utilizando teclado.
 * Uso: Usado geralmente em cameras.
 *-------------------------------- 
*/

/*entradas padrao:
	ctrl esquerdo: habilita/desabilita movimentacao
	teclas W,S,D,A: movimenta objeto
    teclas de setas cima, baixo, esquerda, direita: rotaciona o objeto
*/

using UnityEngine;
using System.Collections;
public class Movimentacao_teclado : MonoBehaviour {

	public float velocidade = 0.002f; // regula a velocidade de deslocamento da camera em metros por ciclo
	private bool ativo; // indica se o usuario esta no modo movimentacao ou nao
	private bool[] loc; // variavel auxiliar

	void Start () {
		ativo = false;
		loc = new bool[10];
	}
	
	// funcao responsavel por movimentar o objeto dado entradas
	void Translacao () {
		//Os movimentos sao baseados na varivavel 'velocidade' em metros nas direcoes:	
		if (Input.GetKey (KeyCode.A)) {
			this.transform.position = this.transform.position - this.transform.right * velocidade; // esquerda
		}
		if (Input.GetKey (KeyCode.S)) {
			this.transform.position = this.transform.position - this.transform.forward * velocidade; // para tras
		}
		if (Input.GetKey (KeyCode.D)) {
			this.transform.position = this.transform.position + this.transform.right * velocidade; // direita
		}
		if (Input.GetKey (KeyCode.W)) {
			this.transform.position = this.transform.position + this.transform.forward * velocidade; // frente
		}
		if (Input.GetKey (KeyCode.Space)) {
			this.transform.position = this.transform.position + this.transform.up * velocidade; // cima
		}
		if (Input.GetKey (KeyCode.LeftShift)) {
			this.transform.position = this.transform.position - this.transform.up * velocidade; // baixo
		}
	}

	// funcao responsavel por rotacionar o objeto dado entradas
	void Rotacao(){
		//rotacao em Y
		if (Input.GetKey (KeyCode.LeftArrow)) {
			this.transform.localRotation = Quaternion.Euler(this.transform.localRotation.eulerAngles - Vector3.up);
		}
		if (Input.GetKey (KeyCode.RightArrow)) {
			this.transform.localRotation = Quaternion.Euler(this.transform.localRotation.eulerAngles + Vector3.up);
		}

		//rotacao em X
		if (Input.GetKey (KeyCode.UpArrow)) {
			if(!(this.transform.localRotation.eulerAngles.x <= 274 && this.transform.localRotation.eulerAngles.x > 180)){
				this.transform.localRotation = Quaternion.Euler(this.transform.localRotation.eulerAngles - Vector3.right);
			}
		}
		if (Input.GetKey (KeyCode.DownArrow)) {
			if(!(this.transform.localRotation.eulerAngles.x >= 87f && this.transform.localRotation.eulerAngles.x < 180)){
				this.transform.localRotation = Quaternion.Euler(this.transform.rotation.eulerAngles + Vector3.right);
			}
		}
	}

	//funcao de atualizacao que verifica se esta no modo movimentacao e dai chama as funcoes de rotacao
	//e translacao
	void Update () {
		if (Input.GetKey (KeyCode.LeftControl)) {
			if(!loc[0])
			{
				ativo = !ativo;				//liga/desliga o modo movimentacao
			}
			loc[0] = true;
		}else {loc[0] = false;}
		
		if (ativo) {
			Translacao ();
			Rotacao ();
		}
	}
}
