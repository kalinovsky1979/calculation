using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnswerNumberButtonManager : MonoBehaviour, IAnswerNumberButtonManager
{
	public event EventHandler<int> OnAnswerClicked;

	public void SendNumber(int n)
	{
		OnAnswerClicked?.Invoke(this, n);
	}
}

public interface IAnswerNumberButtonManager
{
	event EventHandler<int> OnAnswerClicked;
}
