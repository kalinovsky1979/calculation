using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBeeLandingPoint
{
	Vector3 position { get; }
	bool IsBusy();
	void MakeBusy();
	void MakeFree();
}
