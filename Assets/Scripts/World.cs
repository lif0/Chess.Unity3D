using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;


public class World : MenuManager {

	[SerializeField] bool _pause = false;
	public bool PAUSED{get{return _pause;}}

	public Light _sun;
	private AudioSource[] _worldsound;

	public GameObject _UI;
	public Text _msg;


	// Use this for initialization
	void Start () {
		_worldsound = this.GetComponents<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyUp(KeyCode.Escape) && !_pause) {
			PauseGame ();
		}
		else if (Input.GetKeyUp(KeyCode.Escape) && _pause) 
		{
			ContinueGame ();
		}
	}

	public void ContinueGame(){
		_pause = false;
		//Time.timeScale = 1;
		_sun.enabled = true;
		_worldsound[1].Play ();
		_UI.SetActive (false);
	}

	public void PauseGame(){
		_msg.text = "";
		_pause = true;
		//Time.timeScale = 0;
		_sun.enabled = false;
		_worldsound[1].Pause ();
		_UI.SetActive (true);
	}
		
	public void Wins(bool isWhite){
		PauseGame ();
		_msg.enabled = true;
		_worldsound [2].Play ();
		if (isWhite)
			_msg.text = "<color=#00ff00ff>Победила команда Белых</color>";
		else
			_msg.text = "<color=#00ff00ff>Победила команда Черных</color>";
	}
}
