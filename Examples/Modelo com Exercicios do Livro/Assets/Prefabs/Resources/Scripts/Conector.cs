/*--------------------------------
 * Conector.cs 18/10/2014
 * Definicao: Este script tem funçao de unir dois objetos
 * de maneira que pareçam estar grudados um ao outro.
 * Uso: Usado no Prefab Cola.
 *-------------------------------- 
*/
using UnityEngine;
using System.Collections;

public class Conector : MonoBehaviour {
	
	public GameObject Objeto1; // objeto publico que sera grudado ao objeto2 
	public GameObject Objeto2; // objeto publico que sera grudado ao objeto1
	
	//funçao que inicia o script
	//Essa fuçao define um fixedJoint que unira os dois objetos
	void Start () {
		FixedJoint ligacao;
		if (Objeto1 != null && Objeto2 != null) {
			ligacao = Objeto1.AddComponent<FixedJoint> ();
			ligacao.connectedBody = Objeto2.GetComponent<Rigidbody>();
		}
	}
}
