using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * управление объектами игры
 */
public class GameManager : MonoBehaviour, IGameManager
{
	[SerializeField] private BeeLandingManager beeLandingManager;

	//private GameMusicManager gameMusicManager;
	//private AudioSource audioSource;

	private void Start()
	{
		//gameMusicManager = FindAnyObjectByType<GameMusicManager>();
		//audioSource = GetComponent<AudioSource>();

		//audioSource.clip = gameMusicManager.randomListsBgMusic.Next();
		//audioSource.loop = true;
		//audioSource.Play();

		//beeLandingManager.Run();
	}

	public void PlayGame()
	{
		beeLandingManager.Run();
	}

	public void StopGame()
	{
		beeLandingManager.StopAndReset();
	}
}
