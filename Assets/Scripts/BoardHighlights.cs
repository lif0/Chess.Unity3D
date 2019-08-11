using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoardHighlights : MonoBehaviour {
	public static BoardHighlights Instance{set;get;}

	public GameObject boardPrefab;
	private List<GameObject> highlights;
	private void Start(){
		
		Instance = this;
		highlights = new List<GameObject> ();
	}

	private GameObject GetHighlightsObj(){
		GameObject obj  = highlights.Find(g => !g.active);

		if (obj == null) {
			obj = Instantiate(boardPrefab);
			highlights.Add (obj);
		}
		return obj;
	}

	public void HighlightAllowedMoves(bool[,] moves){
		for(int i = 0; i < 8; i++){
			for	(int j = 0; j < 8; j++){
				if(moves[i,j]){
					GameObject obj = GetHighlightsObj();
					obj.SetActive(true);
					obj.transform.position  = new Vector3(i + 0.5f, 0.01f , j+0.5f);;
				}
			}
		}
	}

	public void Hidehightlights(){
		foreach(GameObject obj in highlights){
			obj.SetActive(false);
		}
	}
}
