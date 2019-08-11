using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class BoardManager : MonoBehaviour {

	public static BoardManager Instance{set;get;}
	private bool[,] allowedMoves{set;get;}

	public Chessman[,] Chessmans{ set; get;}
	private Chessman selectedChess;

	private const float TILE_SIZE = 1.0f;
	private const float TILE_OFFSET = 0.5f;

	private int selectionX = -1;
	private int selectionY = -1;

	public  List<GameObject> chessPrefabs;
	private List<GameObject> activeChess;//=newList<GameObject>();

	public bool isWhiteTurn = true;

	public float speedRotateCamera = 25.0f;

	private AudioSource switchCameraAudio;
	private World world;

	void Start(){
		Instance = this;
		SpawnAllChess();
		world = this.GetComponent<World>();
		switchCameraAudio = this.GetComponent<AudioSource> ();
	}

	void Update () {
		//Camera.main.transform.RotateAround(new Vector3(4,0,4),new Vector3(0,1,0),180*Time.deltaTime);

		if (world.PAUSED)
			return;
		else {
			DrawBoard ();
			UpdateSelection ();

			if (Input.GetMouseButtonDown (0)) {
				if (selectionX >= 0 && selectionY >= 0) {
					if (selectedChess == null) {
						//Select the pieces
						SelectChessman (selectionX, selectionY);
					} else {
						//Move the pieces
						MoveChessman (selectionX, selectionY);
					}
				}
			}
		}
	}

	private void SelectChessman(int x,int y){
		if (Chessmans [x, y] == null) //В указанных координатах фигуры нет
			return;
		if (Chessmans [x, y].isWhite != isWhiteTurn) //В указанных координатах фигура есть,но она белая,а ход черных.
            return;

		bool hasAtleastOneMove = false;// Имеет хотя бы одно перемещение
        allowedMoves = Chessmans[x,y].PossibleMove();//Возвращает есть ли возможность сделать ход,и в какие клетки.
        for (int i = 0; i < 8; i++) 
			for (int j = 0; j < 8; j++)
				if(allowedMoves[i,j])
					hasAtleastOneMove = true;

		selectedChess = Chessmans [x, y];
		BoardHighlights.Instance.HighlightAllowedMoves (allowedMoves);//Рисуем клетку в голубой цвет,в которые можно сделать ход. BoardHighlights – класс который закрашивает клетки.
    }

	private void MoveChessman(int x,int y){
		
		if (allowedMoves[x,y]) { //В клетку х у можно идти?
			Chessman c = Chessmans[x,y]; //Получаем фигуру которая стоит в клетке куда мы собираемся идти

            if (c != null && c.isWhite != isWhiteTurn){//Если фигура есть и она не цвета моей команды


				if(c.GetType() == typeof(King)){//Мы съели короля?
					world.Wins (isWhiteTurn);//Вывод банера победы
                    return;//Все Выходим.
                }
				activeChess.Remove(c.gameObject);//иначе удаляем фигуру из списка живых
                Destroy(c.gameObject);//Удаляем фигуру с поля
            }


            //Если вдруг мы идем пешкой и после хода мы оказываемся на 
            //противоположной стороне,заменяем пешку на Королеву.
            if (selectedChess.GetType () == typeof(Pawn)) {
				if (y == 7) {
					activeChess.Remove (selectedChess.gameObject);
					Destroy (selectedChess.gameObject);
					SpawnChess(1,x,y,0);
					selectedChess = Chessmans [x, y];

				} else if (y == 0) 
				{
					activeChess.Remove (selectedChess.gameObject);
					Destroy (selectedChess.gameObject);
					SpawnChess (7, x, y, 0);
					selectedChess = Chessmans [x, y];
				}
			}
            //После сделанного хода меняем позиции шахматы в остальных
            //переменных и меняет сторону которая должна делать ход
            Chessmans[selectedChess.CurrentX, selectedChess.CurrentY] = null;
			selectedChess.transform.position = GetTileCenter (x,y);
			selectedChess.SetPosition(x,y);
			Chessmans[x, y] = selectedChess;
			isWhiteTurn = !isWhiteTurn;
			//
			//StartCoroutine(switchDerection()); //Делаем поворот
            //
        }

		BoardHighlights.Instance.Hidehightlights();
		selectedChess = null;}

/*	private IEnumerator switchDerection(){
		float currentAngle = 0.0f;
		switchCameraAudio.Play ();
		while(true){
			float step = speedRotateCamera * Time.deltaTime;
			if (currentAngle + step > 180) 
			{
				step = 180 - currentAngle;
				Camera.main.transform.RotateAround(new Vector3(4,0,4),new Vector3(0,1,0),step);
				break;
			}
			currentAngle += step;
			Camera.main.transform.RotateAround(new Vector3(4,0,4),new Vector3(0,1,0),step);
			yield return null;
		}
		switchCameraAudio.Stop ();
	}
*/


	private void UpdateSelection(){
		if(!Camera.main)
			return;

		RaycastHit hit;
		if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit,25.0f,LayerMask.GetMask("ChessPlane")))
		{
			selectionX = (int)hit.point.x;
			selectionY = (int)hit.point.z;
		}
		else
		{
			selectionX = -1;
			selectionY = -1;
		}}
	public void SpawnAllChess(){
		activeChess = new List<GameObject>();
		Chessmans = new Chessman[8, 8];
		//the White Team-----------------
		//King
		SpawnChess(0,3,0,0);
		//Queen
		SpawnChess(1,4,0,0);
		//Rooks
		SpawnChess(2,0,0,0);
		SpawnChess(2,7,0,0);
		//Bishop
		SpawnChess(3,2,0,0);
		SpawnChess(3,5,0,0);
		//Knights
		SpawnChess(4,1,0,0);
		SpawnChess(4,6,0,0);
		//Pawns
		for(int i = 0; i < 8; i++)
			SpawnChess(5,i,1,0);

		//the Black Team--------------------

		//King
		SpawnChess(6,3,7,0);
		//Queen
		SpawnChess(7,4,7,0);
		//Rooks
		SpawnChess(8,0,7,0);
		SpawnChess(8,7,7,0);
		//Bishop
		SpawnChess(9,2,7,0);
		SpawnChess(9,5,7,0);
		//Knights
		SpawnChess(10,1,7,180);
		SpawnChess(10,6,7,180);
		//Pawns
		for(int i = 0; i < 8; i++)
			SpawnChess(11,i,6,0);}


	private void SpawnChess(int index, int x, int y, int quaterY){
		
		GameObject obj = Instantiate(chessPrefabs[index], GetTileCenter(x,y), Quaternion.Euler(0,quaterY,0)) as GameObject;
		obj.transform.SetParent(transform);
		Chessmans [x, y] = obj.GetComponent<Chessman> ();
		Chessmans [x, y].SetPosition (x, y);
		activeChess.Add(obj);}
	
	private Vector3 GetTileCenter(int x,int z){
		Vector3 vec = Vector3.zero;
		vec.x += (TILE_SIZE * x) + TILE_OFFSET;
		vec.z += (TILE_SIZE * z) + TILE_OFFSET;
		return vec;}

	void DrawBoard () {
		Vector3 widthLine = Vector3.right * 8;
		Vector3 heigthLine = Vector3.forward * 8;

		for(int i = 0; i <=8 ; i++){
			Vector3 start = Vector3.forward * i;
			Debug.DrawLine(start, start+widthLine);
			for(int j = 0; j <=8 ; j++){

				start = Vector3.right * j;
				LineRenderer line = new LineRenderer ();
				Debug.DrawLine(start,start + heigthLine);

			}

		}


		//Draw the selection
		if(selectionX >= 0 && selectionY >= 0){
			Debug.DrawLine(Vector3.forward * selectionY + Vector3.right * selectionX,
				Vector3.forward * (selectionY +1) + Vector3.right * (selectionX +1));

			Debug.DrawLine(Vector3.forward * (selectionY +1) + Vector3.right * selectionX,
				Vector3.forward * selectionY + Vector3.right * (selectionX +1));
		}}


	/*private void EndGame(){
		if (isWhiteTurn)
			Debug.Log ("White team wins");
		else
			Debug.Log ("Black team wins");
		world.Wins (isWhiteTurn);

		//foreach (GameObject obj in activeChess)
			//Destroy (obj);

		//isWhiteTurn = true;
		//BoardHighlights.Instance.Hidehightlights ();
		//SpawnAllChess ();
	}*/
}
