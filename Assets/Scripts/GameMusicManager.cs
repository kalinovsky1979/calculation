using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMusicManager : MonoBehaviour
{
	[SerializeField, Range(0.0f, 1.0f)] private float effectsVolume = 1.0f;
	[SerializeField, Range(0.0f, 1.0f)] private float musicVolume = 1.0f;

	public float EffectsVolume => effectsVolume;
	public float MusicVolume => musicVolume;

	//public AudioClip hitBaloon;
	//public AudioClip goodBaloon;
	public AudioClip rabbitWalk;
	//public AudioClip waterWalk;

	//[SerializeField] private AudioClip[] gameMusic;
	//[SerializeField] private AudioClip[] rabbitEat;
	//[SerializeField] private AudioClip[] goodHits;
	//[SerializeField] private AudioClip[] badHits;
	//[SerializeField] private AudioClip[] waterWalks;
	
	public ClipRandomList randomListsBgMusic;
	public ClipRandomList randomListsHits;
}
