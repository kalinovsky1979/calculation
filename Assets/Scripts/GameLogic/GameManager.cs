using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * св€зывает общую логику сцены
 */
public class GameManager : MonoBehaviour, IGameManager
{
	[SerializeField] private CheeringAnimalManager cheeringAnimalManager;

	[SerializeField] private QuestManager questManager;
	private IQuestManager _questManager;

	[SerializeField] private AnswerNumberButtonManager answerNumberButtonManager;
	private IAnswerNumberButtonManager _answerNumberButtonManager;

	public event EventHandler GameOver;

	private bool readyToAnswer = true;

	private Dictionary<int, string> _numbers = new Dictionary<int, string> {
		{ 0, "zero" }, { 1, "one" }, { 2, "two" }, { 3, "three" }, { 4, "four" }, { 5, "five" },
		{ 6, "six" }, { 7, "seven" }, { 8, "eight" }, { 9, "nine" }
	};

	private void Start()
	{
		//var a = new RandomList<CheeringAnimal>(cheeringAnimals).Next();
		//var i = Instantiate(a, animalRespPoint.position, Quaternion.identity);
		//_cheeringAnimal = i.GetComponent<CheeringAnimal>();

		_answerNumberButtonManager = answerNumberButtonManager;
		_answerNumberButtonManager.OnAnswerClicked += _answerNumberButtonManager_OnAnswerClicked;

		_questManager = questManager;
		_questManager.ToysAppeared += _questManager_ToysAppeared;

		StartCoroutine(preStart());
	}

	private IEnumerator preStart()
	{
		yield return StartCoroutine(cheeringAnimalManager.Arrive());
		yield return StartCoroutine(_questManager.NextNumberCoroutine());

		_answerNumberButtonManager.TurnNextNumber(_questManager.NumberCurrent);
	}

	private void _questManager_ToysAppeared(object sender, EventArgs e)
	{
		// можно принимать ответ игрока
		readyToAnswer = true;
	}

	/*
	 * ≈сли нужно дождатьс€ два параллельных действи€, можно повтаить счетчик задачь, которые должны быть выполнены
	 * Ќапример две задачи параллелниые
	 * completedTasksCount = 0
	 * task A finished : completedTasksCount++
	 * task B finished : completedTasksCount++
	 * if(completedTasksCount == 2 && e == _questManager.NumberCurrent)
	 * 
	 * или сделать отдельный метод, который возвращает false/true разрешение на прием сл ответа
	 * 
	 * ћожет создать класс-контейнер, в который эти задачи помещаютс€. ѕотом он их запускает, и ждет их выполнени€.
	 * - и сообщает клиенту услуги
	 */

	private void _answerNumberButtonManager_OnAnswerClicked(object sender, int e)
	{
		if(readyToAnswer && e == _questManager.NumberCurrent)
		{
			readyToAnswer = false;
			StartCoroutine(rightAnswerCoroutine(e));
		}
	}

	public void PlayGame()
	{

	}

	public void StopGame()
	{

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
