using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AnswerNumberButtonManager : MonoBehaviour, IAnswerNumberButtonManager
{
	[SerializeField] private AnswerNumberButton[] answerNumberButtons;
	[SerializeField] private Transform[] buttonPoints;

	public event EventHandler<int> OnAnswerClicked;

	private List<AnswerNumberButton> selectedButtons = new List<AnswerNumberButton>();

	public void AnswerNumberFromUser(int n)
	{
		OnAnswerClicked?.Invoke(this, n);
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
			obj.gameObject.transform.position = buttonPoints[iI].position;
			obj.gameObject.SetActive(true);
			iI++;
		}
	}

	private void clearSelectedButtons()
	{
		if(selectedButtons.Count != 0)
		{
			foreach (var obj in selectedButtons)
			{
				obj.gameObject.SetActive(false);
			}

			selectedButtons.Clear();
		}
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.blue;

		foreach (var obj in buttonPoints)
			Gizmos.DrawSphere(obj.position, 0.2f);
	}
}

public interface IAnswerNumberButtonManager
{
	event EventHandler<int> OnAnswerClicked;
	void TurnNextNumber(int n);
}
