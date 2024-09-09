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
	//[SerializeField] private CountdownTimer cdTimer;
	[SerializeField] private ReadySetGoTimer rsgTimer;
	[SerializeField] private GameScoresManager gameScoresManager;

	[SerializeField] private string[] greetingTexts;

	[SerializeField] private GameObject preventGaming;

	private IGameManager _gameManager;

	public float gameDelay = 5;

	private AudioSource player;

	private int _level;

	//private SaveLoadSettings settings = null;

	private RandomList<string> greetingTextsRandomList;

	// Start is called before the first frame update
	void Start()
	{
		_gameManager = gameManager;

		_gameManager.GameOver += _gameManager_GameOver;

		StartCoroutine(firstInitAndStartCoroutine());
	}

	private void _gameManager_GameOver(object sender, System.EventArgs e)
	{
		StartCoroutine(toFinalPageCoroutine());
	}

	private IEnumerator firstInitAndStartCoroutine()
	{
		yield return new WaitForEndOfFrame();

		player = GetComponent<AudioSource>();
		player.loop = true;
;
		rulesPage.StartGame += RulesPage_StartGame;
		finalPage.RestartClicked += FinalPage_RestartClicked;

		greetingTextsRandomList = new RandomList<string>(greetingTexts, false);

		StartCoroutine(enterRulePageCoroutine());
	}
	private IEnumerator enterRulePageCoroutine()
	{
		preventToGamePage = true;

		darkenPage.DarkenInstantly(1.0f);

		rulesPage.EnterPage($"Count the number of toys and press the corresponding number");
		//gamePage.EnterPage();

		yield return new WaitForSeconds(1.0f);

		yield return StartCoroutine(darkenPage.UndarkenScreen(0.7f));

		yield return new WaitForSeconds(2.0f);

		//player.volume = gameMusicManager.MusicVolume;
		player.clip = gameMusicManager.gameMenuList.Next();
		player.Play();

		preventToGamePage = false;
	}

	private bool preventToGamePage = true;

	private void RulesPage_StartGame(object sender, System.EventArgs e)
	{
		if(preventToGamePage) return;
		StartCoroutine(toGamePageCoroutine());
	}

	private void FinalPage_RestartClicked(object sender, System.EventArgs e)
	{
		StartCoroutine(toStartPageCoroutine());
	}

	//private void GameManager_GameWin(object sender, System.EventArgs e)
	//{
	//	StartCoroutine(toFinalPageCoroutine());
	//}

	private IEnumerator toGamePageCoroutine()
	{
		rulesPage.ExitPage();
		yield return new WaitForSeconds(0.3f);
		//yield return StartCoroutine(rsgTimer.rsgStarting());
		
		player.Stop();

		preventGaming.SetActive(false);

		player.clip = gameMusicManager.gameMusicList.Next();
		player.Play();

		_gameManager.PlayGame();
	}

	private IEnumerator toStartPageCoroutine()
	{
		preventToGamePage = true;

		player.Stop();
		yield return StartCoroutine(darkenPage.DarkenScreen(1.0f, 0.7f));

		//gameScoresManager.ResetScores();

		gamePage.EnterPage();
		finalPage.ExitPage();
		rulesPage.EnterPage($"Count the number of toys and press the corresponding number");

		preventGaming.SetActive(true);

		yield return new WaitForSeconds(0.5f);
		yield return StartCoroutine(darkenPage.UndarkenScreen(0.7f));

		yield return new WaitForSeconds(2.0f);

		player.clip = gameMusicManager.gameMenuList.Next();
		player.Play();

		preventToGamePage = false;
	}

	private IEnumerator toFinalPageCoroutine()
	{
		yield return new WaitForSeconds(1.5f);

		gamePage.ExitPage();

		finalPage.GreatingText(greetingTextsRandomList.Next());
		finalPage.EnterPage();
	}
}
