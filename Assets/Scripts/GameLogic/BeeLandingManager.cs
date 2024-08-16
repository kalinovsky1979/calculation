using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BeeLandingManager : MonoBehaviour
{
	private BeeLandingPoint[] points;
	private BeeThinker[] beeThinkers;

	public int maxBees = 8;

	private Coroutine mainCoroutine;
	private bool isRunning = false;


	private void Start()
	{

	}

	private void Update()
	{

	}

	public void StopAndReset()
	{
		if (isRunning)
		{
			//StopCoroutine(mainCoroutine);
			isRunning = false;
		}

		foreach (var bee in beeThinkers)
		{
			bee.StopAndReset();
		}
	}

	public void Run()
	{
		if(beeThinkers == null)
			beeThinkers = FindObjectsByType<BeeThinker>(FindObjectsSortMode.None);
		if(points == null)
			points = FindObjectsByType<BeeLandingPoint>(FindObjectsSortMode.None);

		mainCoroutine = StartCoroutine(startingBeeManager());
	}

	public void ReleasePoint(IBeeLandingPoint p)
	{
		if(p == null)
		{
			Debug.LogWarning("it is not BeeLandingPoint");
			return;
		}

		p.MakeFree();
	}

	private IEnumerator startingBeeManager()
	{
		isRunning = true;

		while (isRunning) 
		{
			if (points.Count(x => x.IsBusy()) < maxBees)
			{
				var freePoints = points.Where(x => !x.IsBusy()).ToArray();

				if(freePoints.Length > 0)
				{
					var freePoint = freePoints[Random.Range(0, freePoints.Length)];

					var freeBees = beeThinkers.Where(x => !x.IsBusy).ToArray();
					if(freeBees.Length > 0)
					{
						var freeBee = freeBees[Random.Range(0, freeBees.Length)];

						freePoint.MakeBusy();
						freeBee.Go(freePoint);
					}
				}
			}

			yield return new WaitForSeconds(0.5f);
		}
	}
}
