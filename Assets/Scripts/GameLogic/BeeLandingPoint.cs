using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class BeeLandingPoint : MonoBehaviour, IBeeLandingPoint
{
	public float radius = 0.1f;

	private bool _busy = false;

	public Vector3 position => transform.position;

	public bool IsBusy()
	{
		return _busy;
	}

	public void MakeBusy()
	{
		_busy = true;
	}

	public void MakeFree()
	{
		_busy = false;
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;

		Gizmos.DrawWireSphere(transform.position, radius);
	}
}
