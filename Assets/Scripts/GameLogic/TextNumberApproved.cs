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
		textNumberTMP.text = txt;
		animator.SetTrigger("appear");
	}

	public void Disappear()
	{
		animator.SetTrigger("disappear");
	}
}
