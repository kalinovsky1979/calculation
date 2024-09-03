using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToysMatrix : MonoBehaviour
{
	[SerializeField] private Transform[] points;
	[SerializeField] private int numberName;

	public int NumberName => numberName;

	private AnimateVector3 animateToyScale;
	private GameObject toy;
	private List<GameObject> instantiatedToys;

	public IEnumerator AppearToysCoroutine(GameObject toy)
	{
		if(points.Length == 0) yield break;

		ClearToys();

		if(instantiatedToys == null) instantiatedToys = new List<GameObject>();
		if(animateToyScale == null)
		{
			animateToyScale = new AnimateVector3();
			animateToyScale.OnAnimationStep = (x) =>
			{
				foreach (GameObject iToy in instantiatedToys)
				{
					iToy.transform.localScale = x;
				}
			};
		}

		this.toy = toy;

		foreach (var point in points)
		{
			float randomAngle = Random.Range(0f, 360f);
			Quaternion randomRotation = Quaternion.Euler(0f, randomAngle, 0f);
			instantiatedToys.Add(Instantiate(this.toy, point.position, randomRotation));
		}

		animateToyScale.valueA = new Vector3(0, 0, 0);
		animateToyScale.valueB = toy.transform.localScale;
		animateToyScale.duration = 0.5f;

		while(animateToyScale.update(null))
			yield return null;
	}

	private void ClearToys()
	{
		if (instantiatedToys == null) return;

		if (instantiatedToys.Count > 0)
		{
			foreach (GameObject instantiated in instantiatedToys)
			{
				Destroy(instantiated);
			}
			instantiatedToys.Clear();
		}
	}

	public IEnumerator DisappearToysCoroutine()
	{
		if (points.Length == 0) yield break;

		animateToyScale.valueA = toy.transform.localScale;
		animateToyScale.valueB = new Vector3(0, 0, 0);
		animateToyScale.duration = 0.3f;

		while (animateToyScale.update(null))
			yield return null;

		ClearToys();
	}

	private void OnDrawGizmos()
	{
		if(points == null) return;
		if (points.Length == 0) return;

		Gizmos.color = Color.yellow;

		foreach(var p in points)
		{
			Gizmos.DrawSphere(p.position, 0.7f);
		}
	}
}
