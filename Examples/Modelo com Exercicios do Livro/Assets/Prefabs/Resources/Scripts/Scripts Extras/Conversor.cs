/*--------------------------------
 * Conversor.cs 
 * Definicao: Auxiliar para conversoes de tipo.
 *-------------------------------- 
*/

using UnityEngine;
public class Conversor : MonoBehaviour {
	//Converte byte[] em int
	public static int getInt(byte [] vetor, int i){
		int retorno = 0;
		while(vetor[i] >= 48 && vetor[i] < 58){
			retorno = retorno*10 + vetor[i]-48;
			i++;
		}
		return retorno;
	}
}
