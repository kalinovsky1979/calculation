using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheeringAnimal : MonoBehaviour, ICheeringAnimal
{
	[SerializeField] private TextNumberApproved textApproved;

	[SerializeField] private Animator animator;
	[SerializeField] private float speed = 4;
	[SerializeField] private float rotationSpeed = 2;

	RotateAndGoSection moving;

	private void Awake()
	{
		moving = new RotateAndGoSection(transform);

		moving.OnStartRotation = () =>
		{
			animator.SetTrigger("run");
		};

		moving.OnEndLinear = () =>
		{
			animator.SetTrigger("idle");
		};

		moving.Speed = speed;
	}

	public IEnumerator CheerUpCoroutine(string txt)
	{
		animator.SetTrigger("cheer1");

		textApproved.Appear(txt);

		yield return new WaitForSeconds(5);

		textApproved.Disappear();
		//animator.SetTrigger("idle");
	}

	public IEnumerator MoveCoroutine(Vector3 toPoint)
	{
		moving.NextTarget(toPoint);

		while(moving.update(null))
			yield return null;
	}

	public void KillSelf()
	{
		Destroy(gameObject);
	}
}

public interface ICheeringAnimal
{
	IEnumerator MoveCoroutine(Vector3 toPoint);
	IEnumerator CheerUpCoroutine(string txt);
	void KillSelf();
}