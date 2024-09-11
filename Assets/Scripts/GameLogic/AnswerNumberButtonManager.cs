using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class AnswerNumberButtonManager : MonoBehaviour, IAnswerNumberButtonManager
{
	[SerializeField] private AnswerNumberButton[] answerNumberButtons;
	[SerializeField] private Transform pointA;
	[SerializeField] private Transform pointB;

	private Vector3[] buttonPoints;

	public event EventHandler<int> OnAnswerClicked;

	private List<AnswerNumberButton> selectedButtons = new List<AnswerNumberButton>();

	public void AnswerNumberFromUser(int n)
	{
		OnAnswerClicked?.Invoke(this, n);
	}

	private void Start()
	{
		buttonPoints = calculatePointsBetween(pointA.position, pointB.position, 5);
	}

	public void TurnNextNumber(int n)
	{
		clearSelectedButtons();

		var givenButton = answerNumberButtons.FirstOrDefault(x => x.NumberName == n);

		if (givenButton == null) throw new Exception($"There is not button with number {n}");

		// Step 2: Exclude the given object from the array
		var filteredArray = answerNumberButtons.Where(obj => obj != givenButton).ToArray();

		// Step 3: Shuffle the remaining array using Unity's Random tool
		for (int i = filteredArray.Length - 1; i > 0; i--)
		{
			int randomIndex = UnityEngine.Random.Range(0, i + 1);
			var temp = filteredArray[i];
			filteredArray[i] = filteredArray[randomIndex];
			filteredArray[randomIndex] = temp;
		}

		// Step 4: Select the first 3 objects from the shuffled array
		selectedButtons = filteredArray.Take(4).ToList();

		// Step 5: Insert the given object at a random position in the new array
		int randomPosition = UnityEngine.Random.Range(0, 4);
		selectedButtons.Insert(randomPosition, givenButton);

		int iI = 0;
		foreach (var obj in selectedButtons)
		{
			//obj.gameObject.transform.position = buttonPoints[iI];
			//obj.gameObject.SetActive(true);
			obj.TurnOn(buttonPoints[iI]);
			iI++;
		}
	}

	//private bool _isAwaitingForAnswer = true;

	//public bool IsAwaitingForAnswer => _isAwaitingForAnswer;

	private void clearSelectedButtons()
	{
		if(selectedButtons.Count != 0)
		{
			foreach (var obj in selectedButtons)
			{
				//obj.gameObject.SetActive(false);
				obj.TurnOff();
			}

			selectedButtons.Clear();
		}
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawSphere(pointA.position, 0.4f);
		Gizmos.DrawSphere(pointB.position, 0.4f);

		Gizmos.color = Color.blue;

		//foreach (var obj in buttonPoints)
		//	Gizmos.DrawSphere(obj, 0.2f);
	}

	private Vector3[] calculatePointsBetween(Vector3 a, Vector3 b, int totalPoints)
	{
		List<Vector3> res = new List<Vector3>();

		for (int i = 0; i < totalPoints; i++)
		{
			float t = (float)i / (totalPoints - 1); // Normalize 'i' between 0 and 1
			Vector3 pointPosition = Vector3.Lerp(a, b, t);

			res.Add(pointPosition);
		}

		return res.ToArray();
	}
}

public interface IAnswerNumberButtonManager
{
	event EventHandler<int> OnAnswerClicked;
	void TurnNextNumber(int n);
}
