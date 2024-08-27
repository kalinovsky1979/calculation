using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheeringAnimal : MonoBehaviour, ICheeringAnimal
{
	[SerializeField] private Animator animator;

	RotateAndGoSection moving;

	private void Awake()
	{
		moving = new RotateAndGoSection(transform);
	}

	public IEnumerator CheerUpCoroutine()
	{

		yield return null;
	}

	public IEnumerator MoveCoroutine(Vector3 toPoint)
	{
		moving.NextTarget(toPoint);

		while(moving.update(null))
			yield return null;
	}
}

public interface ICheeringAnimal
{
	IEnumerator MoveCoroutine(Vector3 toPoint);
}