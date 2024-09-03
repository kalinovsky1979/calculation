using System.Collections;
using System.Collections.Generic;
using TouchScript.Gestures;
using UnityEngine;

public class AnswerNumberButton : MonoBehaviour
{
	[SerializeField] private AnswerNumberButtonManager answerNumberButtonManager;
	[SerializeField] private int numberName;

	public int NumberName => numberName;

	private PressGesture pressGesture;

	AnimateVector3 pressingAnim = new AnimateVector3();

	Vector3 originPosition;

	// Start is called before the first frame update
	void Start()
	{
		pressGesture = GetComponent<PressGesture>();
		pressGesture.Pressed += PressGesture_Pressed;

		pressingAnim.OnAnimationStep = (x) =>
		{
			transform.position = x;
		};
	}

	private bool isAnimating = false;
	private void PressGesture_Pressed(object sender, System.EventArgs e)
	{
		if(isAnimating) return;

		answerNumberButtonManager.AnswerNumberFromUser(numberName);

		StartCoroutine(pressCoroutine());
	}

	private IEnumerator pressCoroutine()
	{
		isAnimating = true;

		originPosition = transform.position;

		pressingAnim.valueA = originPosition;
		pressingAnim.valueB = new Vector3(originPosition.x, originPosition.y - 0.1f, originPosition.z);
		pressingAnim.duration = 0.1f;

		while(pressingAnim.update(null))
			yield return null;

		pressingAnim.valueA = transform.position;
		pressingAnim.valueB = originPosition;
		pressingAnim.duration = 0.3f;

		while (pressingAnim.update(null))
			yield return null;

		isAnimating = false;
	}
}
