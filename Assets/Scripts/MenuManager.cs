using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuManager : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
	public float speedRotate;

	void FixedUpdate () {
		Camera.main.transform.RotateAround (new Vector3 (4, 0, 4), new Vector3 (0, 1, 0),Time.deltaTime * speedRotate);
	}

	public void StartGame(){
		SceneManager.LoadScene ("game");
	}


	public void ExitGame(){
		//Debug.Log ("Game is exiting...");
		Application.Quit ();
	}
}