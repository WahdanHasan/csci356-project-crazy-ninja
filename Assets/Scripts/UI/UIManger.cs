using System;
using UnityEngine;

public class UIManger : MonoBehaviour
{
	public static UIManger Instance { get; private set; }
    
	private void Awake()
	{
		UIManger.Instance = this;
	}
    
	private void Start()
	{
		this.gameUI.SetActive(false);
        this.EventSys.SetActive(false);
	}
    
	public void StartGame()
	{
		this.gameUI.SetActive(true);
		this.DeadUI(false);
		this.WinUI(false);
        this.EventSys.SetActive(true);
    }
    
	public void GameUI(bool b)
	{
		this.gameUI.SetActive(b);
	}
    
	public void DeadUI(bool b)
	{
		this.deadUI.SetActive(b);
	}
    
	public void WinUI(bool b)
	{
		this.winUI.SetActive(b);
	}
    
	public GameObject gameUI;
    
	public GameObject deadUI;
    
	public GameObject winUI;

    public GameObject EventSys;
}
