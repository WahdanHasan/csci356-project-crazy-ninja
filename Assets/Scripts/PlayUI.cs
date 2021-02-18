using System;
using TMPro;
using UnityEngine;

public class PlayUI : MonoBehaviour
{
	private void Start()
	{
		//float[] times = SaveManager.Instance.state.times;
		//for (int i = 0; i < this.maps.Length; i++)
		//{
		//	MonoBehaviour.print("i: " + times[i]);
		//	this.maps[i].text = Timer.Instance.GetFormattedTime(times[i]);
		//}
	}
    
	public TextMeshProUGUI[] maps;
}
