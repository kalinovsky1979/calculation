using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class QuestManager : MonoBehaviour, IQuestManager
{
	[SerializeField] private GameObject[] toys;
	[SerializeField] private Transform toysMatrixPoint;
	[SerializeField] private ToysMatrix[] toysMatrices;

	public event EventHandler ToysAppeared;

	private RandomList<GameObject> _toysRandomList;

	public int NumberCurrent => _toyMatrixCurrent == null ? -1 : _toyMatrixCurrent.NumberName;
	private RandomList<ToysMatrix> randomListToysMatrix;
	private ToysMatrix _toyMatrixCurrent;

	public IEnumerator NextNumberCoroutine()
	{
		if(_toyMatrixCurrent != null)
		{
			yield return StartCoroutine(_toyMatrixCurrent.DisappearToysCoroutine());
			Destroy(_toyMatrixCurrent.gameObject);
		}

		_toyMatrixCurrent = Instantiate(randomListToysMatrix.Next(), toysMatrixPoint);

		yield return StartCoroutine(_toyMatrixCurrent.AppearToysCoroutine(_toysRandomList.Next()));

		ToysAppeared?.Invoke(this, EventArgs.Empty);
	}

	// Start is called before the first frame update
	void Start()
	{
		randomListToysMatrix = new RandomList<ToysMatrix>(toysMatrices, false);
		_toysRandomList = new RandomList<GameObject>(toys, false);

		StartCoroutine(NextNumberCoroutine());
	}
}

public interface IQuestManager
{
	event EventHandler ToysAppeared;
	IEnumerator NextNumberCoroutine();
	int NumberCurrent {  get; }
}