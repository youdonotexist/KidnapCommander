using UnityEngine;
 
public class GameState : MonoBehaviour
{
	private static GameState instance;
 
	public static GameState Instance
	{
		get
		{
			if (instance == null)
			{
				GameObject go = GameObject.Find("GameState");
				if (go == null) {
					instance = new GameObject ("GameState").AddComponent<GameState> ();
				}
				else {
					instance = go.GetComponent<GameState>();	
				}
				
				instance.audioSource = instance.gameObject.AddComponent<AudioSource>();
			}
 
			return instance;
		}
	}
	
	public enum GAMESTATE {
		PLANNING,
		ACTION_PAUSE,
		NORMAL,
		START
	};
	public GAMESTATE _gameState = GAMESTATE.PLANNING;
	
	//public bool GameOver = false;
	//public bool DidWin = false;
	//public bool ApplicationQuit = false;
	//public bool Paused = false;
	
	public AudioSource audioSource = null;
	
	public bool IsActionPause() {return _gameState == GAMESTATE.ACTION_PAUSE;}
	public bool IsPlanning() {return _gameState == GAMESTATE.PLANNING;}
	
	public void SetActionPaused(bool paused) {
		if (paused) {
			_gameState = GAMESTATE.ACTION_PAUSE;	
		}
		else {
			_gameState = GAMESTATE.NORMAL;	
		}	
	}
	
}
