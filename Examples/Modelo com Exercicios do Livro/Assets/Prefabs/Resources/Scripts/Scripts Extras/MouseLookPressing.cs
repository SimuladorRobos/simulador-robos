/*--------------------------------
 * Sensor_Botao.cs 18/10/2014
 * Definicao: Esse script eh uma modificacao do script padrao
 * do Unity3d "mouse look", que faz com que seja possivel
 * mover a camera apenas ao pressionar espaco.
 * Uso: Uso geral.
 *-------------------------------- 
*/
using UnityEngine;
using System.Collections;

//De certa forma o script e identico ao padrao o unico ponto em que foi bem modificado eh no proximo comentario
public class MouseLookPressing : MonoBehaviour {

	public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
	public RotationAxes axes = RotationAxes.MouseXAndY;
	public float sensitivityX = 15F;
	public float sensitivityY = 15F;

	public float minimumX = -360F;
	public float maximumX = 360F;

	public float minimumY = -60F;
	public float maximumY = 60F;

	float rotationY = 0F;

	void Update ()
	{
		//verifica se apertou a tecla "espa�o"
		if(Input.GetButton("Jump")){
		if (axes == RotationAxes.MouseXAndY)
		{
			float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;
			
			rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
			rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
			
			transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
		}
		else if (axes == RotationAxes.MouseX)
		{
			transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityX, 0);
		}
		else
		{
			rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
			rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
			
			transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
		}}
	}
	
	void Start ()
	{
		if (GetComponent<Rigidbody>())
			GetComponent<Rigidbody>().freezeRotation = true;
	}
}