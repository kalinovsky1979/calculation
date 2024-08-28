using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * ��������� ����� ������ �����
 */
public class GameManager : MonoBehaviour, IGameManager
{
	[SerializeField] private CheeringAnimalManager cheeringAnimalManager;

	[SerializeField] private QuestManager questManager;
	private IQuestManager _questManager;

	[SerializeField] private AnswerNumberButtonManager answerNumberButtonManager;
	private IAnswerNumberButtonManager _answerNumberButtonManager;

	private bool readyToAnswer = true;

	private void Start()
	{
		//var a = new RandomList<CheeringAnimal>(cheeringAnimals).Next();
		//var i = Instantiate(a, animalRespPoint.position, Quaternion.identity);
		//_cheeringAnimal = i.GetComponent<CheeringAnimal>();

		_answerNumberButtonManager = answerNumberButtonManager;
		_answerNumberButtonManager.OnAnswerClicked += _answerNumberButtonManager_OnAnswerClicked;

		_questManager = questManager;
		_questManager.ToysAppeared += _questManager_ToysAppeared;

		StartCoroutine(cheeringAnimalManager.Arrive());
	}

	private void _questManager_ToysAppeared(object sender, EventArgs e)
	{
		// ����� ��������� ����� ������
		readyToAnswer = true;
	}

	/*
	 * ���� ����� ��������� ��� ������������ ��������, ����� �������� ������� ������, ������� ������ ���� ���������
	 * �������� ��� ������ ������������
	 * completedTasksCount = 0
	 * task A finished : completedTasksCount++
	 * task B finished : completedTasksCount++
	 * if(completedTasksCount == 2 && e == _questManager.NumberCurrent)
	 * 
	 * ��� ������� ��������� �����, ������� ���������� false/true ���������� �� ����� �� ������
	 * 
	 * ����� ������� �����-���������, � ������� ��� ������ ����������. ����� �� �� ���������, � ���� �� ����������.
	 * - � �������� ������� ������
	 */

	private void _answerNumberButtonManager_OnAnswerClicked(object sender, int e)
	{
		if(readyToAnswer && e == _questManager.NumberCurrent)
		{
			readyToAnswer = false;
			StartCoroutine(cheeringAnimalManager.Cheer());
			StartCoroutine(_questManager.NextNumberCoroutine());
		}
	}

	public void PlayGame()
	{

	}

	public void StopGame()
	{

	}

	private IEnumerator startingGameCoroutine()
	{
		yield return StartCoroutine(cheeringAnimalManager.Arrive());
	}
}
