using System;
using Audio;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Lobby : MonoBehaviour
{
	private void Start()
	{
	}
    
	public void LoadMap(int s)
	{
		if (s.Equals(""))
		{
			return;
		}
		SceneManager.LoadScene(s);
		Game.Instance.StartGame();
		//this.playing = true;
		//this.done = false;
		Time.timeScale = 1f;
		UIManger.Instance.StartGame();
		Timer.Instance.StartTimer();

	}
    
	public void Exit()
	{
		Application.Quit(0);
	}
    
	public void ButtonSound()
	{
		AudioManager.Instance.Play("Button");
	}
    
}
