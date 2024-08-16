using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CountdownTimer : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI timeText;

	public float delay = 5.0f;
	public bool showGoOnEnd = false;

	public event EventHandler Expired;

	private bool stopped = false;

	private void Start()
	{
		timeText.text = delay.ToString();
	}

	public void Go()
	{
		StartCoroutine(delayCoroutine());
	}

	public void StopTimer()
	{
		stopped = true;
	}

	private IEnumerator delayCoroutine()
	{
		stopped = false;

		float remainingTime = delay;

		while (remainingTime > 0 && !stopped)
		{
			timeText.text = remainingTime.ToString("0");
			yield return new WaitForSeconds(1f);
			remainingTime--;
		}

		timeText.text = 0.ToString("0");
		Expired?.Invoke(this, EventArgs.Empty);
	}
}
