using System.Collections;
using System.Collections.Generic;
using TouchScript.Gestures;
using UnityEngine;

public class BeeThinker : MonoBehaviour
{
	[SerializeField] private Animator animator;
	[SerializeField] private LayerMask groundLayerMask;
	[SerializeField] private GameObject hitEffect;
	[SerializeField] private GameObject hitGroundEffect;
	[SerializeField] private GameObject escapingEfffect;

	private BeeLandingManager beeLandingManager;
	private RotateAndGoSection sectionRunner = null;
	private IBeeLandingPoint beeLandingPoint = null;

	private PressGesture pressGesture;

	private GameScoresManager gameScoresManager;
	private GameMusicManager gameMusicManager;
	private AudioSource audioSource;

	public bool IsBusy => _isBusy;

	private bool _isBusy = false;
	private bool _isHit = false;

	private Vector3 _originPosition;

	private ChainRunner _mainChainRunner;
	private ChainRunner _hitLandedChainRunner;
	private ChainRunner _hitFlyingChainRunner;
	private ChainRunner _currentChain;

	[SerializeField] private float approachFoodSpeed = 2.0f;
	[SerializeField] private float escapeSpeed = 4.0f;

	// Start is called before the first frame update
	void Start()
	{
		_originPosition = transform.position;
		pressGesture = GetComponent<PressGesture>();
		beeLandingManager = FindAnyObjectByType<BeeLandingManager>();
		gameScoresManager = FindAnyObjectByType<GameScoresManager>();
		gameMusicManager = FindAnyObjectByType<GameMusicManager>();

		audioSource = GetComponent<AudioSource>();

		pressGesture.Pressed += PressGesture_Pressed;
	}

	private bool isFlying = false;

	public void StopAndReset()
	{
		transform.position = _originPosition;

		if (_currentChain != null)
		{
			_currentChain.StopChain();
			_currentChain = null;
		}

		// not all objects started, so not all bee.Go(point) were started
		if(sectionRunner != null)
			sectionRunner.Stop();

		animator.ResetTrigger("fly");
		animator.ResetTrigger("die");
		animator.ResetTrigger("undie");
		animator.ResetTrigger("idle");

		animator.SetTrigger("idle");

		_currentChain = null;
		_mainChainRunner = null;
		_hitLandedChainRunner = null;
		_hitFlyingChainRunner = null;
		//sectionRunner = null;

		_isBusy = false;
		_isHit = false;

		forgetLandingPoint();
	}

	private void buildChainRunner()
	{
		_mainChainRunner = new ChainRunner();

		_mainChainRunner
			// move to the food
			.AddStep(new OperationStep
			{
				Do = () =>
				{
					_isBusy = true;
					isFlying = true;
					animator.ResetTrigger("idle");
					animator.SetTrigger("fly");
					sectionRunner.Speed = approachFoodSpeed;
					sectionRunner.NextTarget(beeLandingPoint.position);
				}
			})
			.AddStep(sectionRunner)
			// eating
			.AddStep(new StayStep
			{
				stayForSeconds = 10,
				OnEnter = (x) =>
				{
					isFlying = false;
					animator.SetTrigger("idle");
				}
			})
			// returning
			.AddStep(new OperationStep
			{
				Do = () =>
				{
					isFlying = true;
					forgetLandingPoint();
					sectionRunner.NextTarget(_originPosition);
					animator.SetTrigger("fly");
				}
			})
			.AddStep(sectionRunner)
			.AddStep(new OperationStep
			{
				Do = () =>
				{
					_isBusy = false;
					isFlying = false;
					animator.SetTrigger("idle");
				}
			});
	}

	private void buildHitLandedChainRunner()
	{
		_hitLandedChainRunner = new ChainRunner();

		_hitLandedChainRunner
			.AddStep(new StayStep
			{
				stayForSeconds = 0.1f
			})
			.AddStep(new StayStep
			{
				OnEnter = (x) =>
				{
					audioSource.clip = gameMusicManager.randomListsHits.Next();
					audioSource.pitch = Random.Range(0.7f, 1.2f);
					audioSource.Play();
					animator.SetTrigger("die");
				},
				stayForSeconds = 2.0f
			})
			.AddStep(new StayStep
			{
				OnEnter = (x) =>
				{
					animator.SetTrigger("undie");
				},
				stayForSeconds = 0.5f
			})
			.AddStep(new OperationStep
			{
				Do = () =>
				{
					sectionRunner.Speed = escapeSpeed;
					sectionRunner.NextTarget(_originPosition);
					animator.SetTrigger("fly");
					forgetLandingPoint();
				}
			})
			.AddStep(sectionRunner)
			.AddStep(new OperationStep
			{
				Do = () =>
				{
					_isHit = false;
					_isBusy = false;
					animator.SetTrigger("idle");
				}
			});
	}

	private void buildHitFlyingChainRunner()
	{
		_hitFlyingChainRunner = new ChainRunner();

		_hitFlyingChainRunner

			.AddStep(new StayStep
			{
				stayForSeconds = 0.1f
			})
			.AddStep(new LinearMoveXZStep
			{
				onlyXZMovement = false,
				movableObject = transform,
				speed = 4,
				adjustDirection = false,
				OnStartMotion = (x) =>
				{
					audioSource.clip = gameMusicManager.randomListsHits.Next();
					audioSource.pitch = Random.Range(0.7f, 1.2f);
					audioSource.Play();
					x.targetPoint = findGroundPoint();
					animator.SetTrigger("idle");
				}
			})
			.AddStep(new StayStep
			{
				OnEnter = (x) =>
				{
					animator.SetTrigger("die");
				},
				stayForSeconds = 2.0f
			})
			.AddStep(new StayStep
			{
				OnEnter = (x) =>
				{
					animator.SetTrigger("undie");
				},
				stayForSeconds = 0.5f
			})
			.AddStep(new OperationStep
			{
				Do = () =>
				{
					sectionRunner.Speed = escapeSpeed;
					sectionRunner.NextTarget(_originPosition);
					animator.SetTrigger("fly");
					forgetLandingPoint();
				}
			})
			.AddStep(sectionRunner)
			.AddStep(new OperationStep
			{
				Do = () =>
				{
					_isHit = false;
					_isBusy = false;
					animator.SetTrigger("idle");
				}
			});
	}

	private void forgetLandingPoint()
	{
		if(beeLandingPoint != null)
		{
			beeLandingManager.ReleasePoint(beeLandingPoint);
			beeLandingPoint = null;
		}
	}

	private void PressGesture_Pressed(object sender, System.EventArgs e)
	{
		if (_isHit) return;
		_isHit = true;

		gameScoresManager.AddScore(1);

		_mainChainRunner.StopChain();

		Instantiate(hitEffect, transform.position, Quaternion.identity);
		StartCoroutine(hitGround());

		if (isFlying)
		{
			_hitFlyingChainRunner.RestartChain();
			_currentChain = _hitFlyingChainRunner;
		}
		else
		{
			_hitLandedChainRunner.RestartChain();
			_currentChain = _hitLandedChainRunner;
		}
	}

	private IEnumerator hitGround()
	{
		yield return new WaitForSeconds(0.2f);
		Instantiate(hitGroundEffect, transform.position, Quaternion.identity);
	}

	// Update is called once per frame
	void Update()
	{
		if(_currentChain != null)
			_currentChain.Update();
	}

	public void Go(IBeeLandingPoint p)
	{
		if (sectionRunner == null)
		{
			sectionRunner = new RotateAndGoSection(transform);
			sectionRunner.linearOnlyXZ = false;
		}

		beeLandingPoint = p;

		if (_mainChainRunner == null) buildChainRunner();
		if (_hitLandedChainRunner == null) buildHitLandedChainRunner();
		if (_hitFlyingChainRunner == null) buildHitFlyingChainRunner();

		_currentChain = _mainChainRunner;

		_currentChain.RestartChain();
	}

	private Vector3 findGroundPoint()
	{
		Ray ray = new Ray(transform.position, Vector3.down);
		RaycastHit hit;

		// Cast the ray
		if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayerMask))
		{
			// If the ray hits an object, get the point of contact
			return hit.point;
		}
		else
		{
			return Vector3.zero;
		}
	}
}
