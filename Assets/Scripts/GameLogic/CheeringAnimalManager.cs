using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheeringAnimalManager : MonoBehaviour
{
	[SerializeField] private CheeringAnimal[] animals;
	[SerializeField] private Transform basePoint;
	[SerializeField] private Transform playPoint;
	[SerializeField] private GameMusicManager gameMusicManager;

	private RandomList<CheeringAnimal> cheeringAnimalRandomList;
	private ICheeringAnimal cheeringAnimalCurrent;

	private AudioSource audioSource;

	private void Awake()
	{
		cheeringAnimalRandomList = new RandomList<CheeringAnimal>(animals);
		audioSource = GetComponent<AudioSource>();
	}

	public IEnumerator Arrive()
	{
		if(cheeringAnimalCurrent == null)
			cheeringAnimalCurrent = Instantiate(cheeringAnimalRandomList.Next(), basePoint.position, Quaternion.identity);

		yield return StartCoroutine(cheeringAnimalCurrent.MoveCoroutine(playPoint.position));
	}

	public IEnumerator Leave()
	{
		yield return StartCoroutine(cheeringAnimalCurrent.MoveCoroutine(basePoint.position));

		cheeringAnimalCurrent.KillSelf();
		cheeringAnimalCurrent = null;
	}

	public IEnumerator Cheer(string txt)
	{
		audioSource.pitch = UnityEngine.Random.Range(0.7f, 1.2f);
		audioSource.clip = gameMusicManager.animalCheerSoundList.Next();
		audioSource.Play();
		yield return StartCoroutine(cheeringAnimalCurrent.CheerUpCoroutine(txt));
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawSphere(basePoint.position, 0.5f);

		Gizmos.color = Color.green;
		Gizmos.DrawSphere(playPoint.position, 0.5f);
	}
}
