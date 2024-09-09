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

	public bool MatricesAreOver => _matricesAreOver;
	private bool _matricesAreOver = false;

	private RandomList<GameObject> _toysRandomList;
	private Queue<ToysMatrix> _toysMatricesQueueList;

	public int NumberCurrent => _toyMatrixCurrent == null ? -1 : _toyMatrixCurrent.NumberName;
	//private RandomList<ToysMatrix> randomListToysMatrix;
	private ToysMatrix _toyMatrixCurrent;

	//private void printItems(ToysMatrix[] m)
	//{
	//	string s = "";

	//	foreach (var item in m)
	//	{
	//		s += item.NumberName.ToString() + ",";
	//	}

	//	Debug.Log(s);
	//}

	public IEnumerator NextNumberCoroutine()
	{
		//printItems(_toysQueueList.ToArray());

		if(_matricesAreOver) yield break;

		if (_toysMatricesQueueList.Count == 0)
		{
			_matricesAreOver = true;
			if (_toyMatrixCurrent != null)
			{
				yield return StartCoroutine(_toyMatrixCurrent.DisappearToysCoroutine());
				Destroy(_toyMatrixCurrent.gameObject);
			}
			yield break;
		}

		if(_toyMatrixCurrent != null)
		{
			yield return StartCoroutine(_toyMatrixCurrent.DisappearToysCoroutine());
			Destroy(_toyMatrixCurrent.gameObject);
		}

		_toyMatrixCurrent = Instantiate(_toysMatricesQueueList.Dequeue(), toysMatrixPoint);

		yield return StartCoroutine(_toyMatrixCurrent.AppearToysCoroutine(_toysRandomList.Next()));

		ToysAppeared?.Invoke(this, EventArgs.Empty);
	}

	private void Awake()
	{
		_toysRandomList = new RandomList<GameObject>(toys, false);
	}

	// Method to shuffle an array using the Fisher-Yates shuffle algorithm
	private void ShuffleArray<T>(T[] array)
	{
		System.Random rng = new System.Random();
		int n = array.Length;
		while (n > 1)
		{
			n--;
			int k = rng.Next(n + 1);
			T value = array[k];
			array[k] = array[n];
			array[n] = value;
		}
	}

	public void Restart()
	{
		var _m = new List<ToysMatrix>(toysMatrices).ToArray();
		ShuffleArray(_m);
		_toysMatricesQueueList = new Queue<ToysMatrix>(_m);
		_matricesAreOver = false;
	}
}

public interface IQuestManager
{
	event EventHandler ToysAppeared;
	IEnumerator NextNumberCoroutine();
	int NumberCurrent { get; }
	bool MatricesAreOver { get; }
	void Restart();
}