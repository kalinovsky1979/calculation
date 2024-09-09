using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMusicManager : MonoBehaviour
{
	[SerializeField, Range(0.0f, 1.0f)] private float effectsVolume = 1.0f;
	[SerializeField, Range(0.0f, 1.0f)] private float musicVolume = 1.0f;

	public float EffectsVolume => effectsVolume;
	public float MusicVolume => musicVolume;
	
	public ClipRandomList gameMusicList;
	public ClipRandomList gameMenuList;

	public ClipRandomList goodHitSoundList;
	public ClipRandomList badHitSoundList;
	public ClipRandomList animalCheerSoundList;
}
