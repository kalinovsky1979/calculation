using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextNumberApproved : MonoBehaviour
{
	private Animator animator;
	[SerializeField] private TextMeshProUGUI textNumberTMP;

	// Start is called before the first frame update
	void Start()
	{
		animator = GetComponent<Animator>();
	}

	public void Appear(string txt)
	{
		// Make the object look at the main camera
		transform.LookAt(Camera.main.transform);

		// Optional: If you want to only rotate along a specific axis (e.g., Y axis)
		Vector3 direction = Camera.main.transform.position - transform.position;
		direction.y = 0; // Zero out the Y-axis to avoid tilting
		transform.rotation = Quaternion.LookRotation(direction);

		textNumberTMP.text = txt;
		animator.SetTrigger("appear");
	}

	public void Disappear()
	{
		animator.SetTrigger("disappear");
	}
}
