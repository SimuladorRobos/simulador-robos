/*--------------------------------
 * SensorBussola.cs 20/01/2015
 * Definicao: Este script descreve o
 * funcionamento de uma bussola que faz
 * com que o objeto agulha esteja sempre
 * que possivel apontando para a direção
 * global Z.
 * Uso: Usado no Prefab Bussola.
 *-------------------------------- 
*/

using UnityEngine;
using System.Collections;

public class SensorBussola : MonoBehaviour {
	public float anguloNorte;
	private Transform agulha;

	// Use this for initialization
	void Start () {
		foreach (Transform filho in transform) {
			if(filho.name == "visualAgulha"){
				agulha = filho;
			}
		}
	}
	
	// Calcula a diferenca em relacao ao eixo Z
	void FixedUpdate () {
		Vector4 norte = Vector3.forward;
		Vector4 localu = -this.transform.right;
		Vector4 localv = this.transform.up;
		Vector4 localw = this.transform.forward;
		Matrix4x4 baseOrto = new Matrix4x4 ();
		baseOrto.SetRow (0, localu);
		baseOrto.SetRow (1, localv);
		baseOrto.SetRow (2, localw);
		baseOrto = baseOrto.transpose;
		norte = baseOrto*norte;
		anguloNorte = 180*Mathf.Atan2 (norte.x, norte.z)/Mathf.PI;
		Vector3 agv = agulha.localEulerAngles;
		agv.y = -anguloNorte;
		agulha.localEulerAngles = agv;
	}
}
