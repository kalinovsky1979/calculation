using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * связывает общую логику сцены
 */
public class GameManager : MonoBehaviour, IGameManager
{
	[SerializeField] private GameMusicManager gameMusicManager;

	[SerializeField] private CheeringAnimalManager cheeringAnimalManager;

	[SerializeField] private QuestManager questManager;
	private IQuestManager _questManager;

	[SerializeField] private AnswerNumberButtonManager answerNumberButtonManager;
	private IAnswerNumberButtonManager _answerNumberButtonManager;

	public event EventHandler GameOver;

	private bool readyToAnswer = true;

	private AudioSource audioSource;

	private Dictionary<int, string> _numbers = new Dictionary<int, string> {
		{ 0, "zero" }, { 1, "one" }, { 2, "two" }, { 3, "three" }, { 4, "four" }, { 5, "five" },
		{ 6, "six" }, { 7, "seven" }, { 8, "eight" }, { 9, "nine" }
	};

	private void Start()
	{
		_answerNumberButtonManager = answerNumberButtonManager;
		_answerNumberButtonManager.OnAnswerClicked += _answerNumberButtonManager_OnAnswerClicked;

		_questManager = questManager;
		_questManager.ToysAppeared += _questManager_ToysAppeared;

		audioSource = GetComponent<AudioSource>();
	}

	private IEnumerator restartCoroutine()
	{
		_questManager.Restart();
		yield return StartCoroutine(cheeringAnimalManager.Arrive());
		yield return StartCoroutine(_questManager.NextNumberCoroutine());

		_answerNumberButtonManager.TurnNextNumber(_questManager.NumberCurrent);
	}

	private void _questManager_ToysAppeared(object sender, EventArgs e)
	{
		readyToAnswer = true;
	}

	private void _answerNumberButtonManager_OnAnswerClicked(object sender, int e)
	{
		if(readyToAnswer && e == _questManager.NumberCurrent)
		{
			audioSource.pitch = UnityEngine.Random.Range(0.7f, 1.2f);
			audioSource.clip = gameMusicManager.goodHitSoundList.Next();
			audioSource.Play();

			readyToAnswer = false;
			StartCoroutine(rightAnswerCoroutine(e));
		}
		else if (readyToAnswer && e != _questManager.NumberCurrent)
		{
			audioSource.pitch = UnityEngine.Random.Range(0.7f, 1.2f);
			audioSource.clip = gameMusicManager.badHitSoundList.Next();
			audioSource.Play();
		}
	}

	public void PlayGame()
	{
		StartCoroutine(restartCoroutine());
	}

	private IEnumerator rightAnswerCoroutine(int n)
	{
		yield return StartCoroutine(cheeringAnimalManager.Cheer(_numbers[n]));

		yield return StartCoroutine(_questManager.NextNumberCoroutine());

		if (_questManager.MatricesAreOver)
		{
			yield return StartCoroutine(endGameCoroutine());
			GameOver?.Invoke(this, EventArgs.Empty);
			yield break;
		}
		_answerNumberButtonManager.TurnNextNumber(_questManager.NumberCurrent);
	}

	private IEnumerator endGameCoroutine()
	{
		yield return StartCoroutine(cheeringAnimalManager.Leave());
	}

	private IEnumerator startingGameCoroutine()
	{
		yield return StartCoroutine(cheeringAnimalManager.Arrive());
	}
}
