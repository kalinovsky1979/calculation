using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheeringAnimalManager : MonoBehaviour
{
	[SerializeField] private CheeringAnimal[] animals;
	[SerializeField] private Transform basePoint;
	[SerializeField] private Transform playPoint;

	private RandomList<CheeringAnimal> cheeringAnimalRandomList;
	private ICheeringAnimal cheeringAnimalCurrent;

	private void Awake()
	{
		cheeringAnimalRandomList = new RandomList<CheeringAnimal>(animals);
		cheeringAnimalCurrent = Instantiate(cheeringAnimalRandomList.Next(), basePoint);
	}

	public IEnumerator Arrive()
	{
		yield return StartCoroutine(cheeringAnimalCurrent.MoveCoroutine(playPoint.position));
	}

	public IEnumerator Leave()
	{
		yield return StartCoroutine(cheeringAnimalCurrent.MoveCoroutine(basePoint.position));
	}

	public IEnumerator Cheer()
	{
		yield return StartCoroutine(cheeringAnimalCurrent.CheerUpCoroutine());
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawSphere(basePoint.position, 0.5f);

		Gizmos.color = Color.green;
		Gizmos.DrawSphere(playPoint.position, 0.5f);
	}
}
