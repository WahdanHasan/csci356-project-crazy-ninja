using System;
using UnityEngine;

public class RandomSfx : MonoBehaviour
{
	private void Awake()
	{
		AudioSource component = base.GetComponent<AudioSource>();
		component.clip = this.sounds[UnityEngine.Random.Range(0, this.sounds.Length - 1)];
		component.playOnAwake = true;
		component.pitch = 1f + UnityEngine.Random.Range(-0.3f, 0.1f);
		component.enabled = true;
	}
    
	public AudioClip[] sounds;
}
