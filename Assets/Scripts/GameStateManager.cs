using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Loading;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
	[SerializeField] private DarkenPage darkenPage;
	[SerializeField] private GamePage gamePage;
	[SerializeField] private FinalPage finalPage;
	[SerializeField] private RulesPage rulesPage;

	[SerializeField] private GameManager gameManager;
	[SerializeField] private GameMusicManager gameMusicManager;
	[SerializeField] private CountdownTimer cdTimer;
	[SerializeField] private ReadySetGoTimer rsgTimer;
	[SerializeField] private GameScoresManager gameScoresManager;

	[SerializeField] private GameObject preventGaming;

	private IGameManager _gameManager;

	public float gameDelay = 5;

	private AudioSource player;

	private int _level;

	private SaveLoadSettings settings = null;

	// Start is called before the first frame update
	void Start()
	{
		_gameManager = gameManager;

		StartCoroutine(starting());
	}

	private IEnumerator starting()
	{
		yield return new WaitForEndOfFrame();

		player = GetComponent<AudioSource>();
		player.loop = true;
;
		rulesPage.StartGame += RulesPage_StartGame;
		finalPage.RestartClicked += FinalPage_RestartClicked;

		cdTimer.Expired += CdTimer_Expired;

		StartCoroutine(enterRulePage());
	}
	private IEnumerator enterRulePage()
	{
		darkenPage.DarkenInstantly(1.0f);

		rulesPage.EnterPage($"HIT AS MANY FLIES AS YOU CAN!");
		gamePage.EnterPage();

		yield return new WaitForSeconds(1.0f);

		yield return StartCoroutine(darkenPage.UndarkenScreen(0.7f));

		player.volume = gameMusicManager.MusicVolume;
		player.clip = gameMusicManager.randomListsBgMusic.Next();
		player.Play();
	}

	private void RulesPage_StartGame(object sender, System.EventArgs e)
	{
		StartCoroutine(startGame());
	}

	private void FinalPage_RestartClicked(object sender, System.EventArgs e)
	{
		StartCoroutine(restartGame());
	}

	private void GameManager_GameWin(object sender, System.EventArgs e)
	{
		StartCoroutine(toFinalPage());
	}

	private IEnumerator startGame()
	{
		rulesPage.ExitPage();
		yield return new WaitForSeconds(0.3f);
		yield return StartCoroutine(rsgTimer.rsgStarting());
		
		preventGaming.SetActive(false);

		_gameManager.PlayGame();
		cdTimer.Go();
	}

	private IEnumerator restartGame()
	{
		player.Stop();
		yield return StartCoroutine(darkenPage.DarkenScreen(1.0f, 0.7f));

		gameScoresManager.ResetScores();

		gamePage.EnterPage();
		finalPage.ExitPage();
		rulesPage.EnterPage($"HIT AS MANY FLIES AS YOU CAN!");

		preventGaming.SetActive(true);

		yield return new WaitForSeconds(0.5f);
		yield return StartCoroutine(darkenPage.UndarkenScreen(0.7f));

		player.clip = gameMusicManager.randomListsBgMusic.Next();
		player.Play();
	}

	private IEnumerator toFinalPage()
	{
		yield return new WaitForSeconds(1.5f);

		cdTimer.StopTimer();
		_gameManager.StopGame();

		gamePage.ExitPage();

		finalPage.GreatingText($"BRILLIANT! YOU HIT {gameScoresManager.Scores} FLIES!!!");
		finalPage.EnterPage();
	}

	private void CdTimer_Expired(object sender, System.EventArgs e)
	{
		// final of game

		StartCoroutine(toFinalPage());
	}
}
