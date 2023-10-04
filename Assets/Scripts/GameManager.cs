using AdMobController;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI mainWordText;
    [SerializeField] private List<TextMeshProUGUI> tabooWordsTexts;

    [SerializeField] private TextMeshProUGUI currentScoreText;
    [SerializeField] private TextMeshProUGUI currentPassLimitText;
    [SerializeField] private TextMeshProUGUI currentTimeLeftText;

    [SerializeField] private GameObject pausePanel;
    [SerializeField] private TextMeshProUGUI pausePanelNameTeamA;
    [SerializeField] private TextMeshProUGUI pausePanelNameTeamB;
    [SerializeField] private TextMeshProUGUI pausePanelScoreTeamA;
    [SerializeField] private TextMeshProUGUI pausePanelScoreTeamB;
    [SerializeField] private TextMeshProUGUI pausePanelWhoseeTurn;

    private bool _isPlayingTeamB;

    private bool _gameStarted;
    private bool _gamePaused;
    private int _currentScore;
    private int _currentPassLimit;
    private int _currentWordIndex;
    private int _gameIterationCount;
    private int _scoreTeamA;
    private int _scoreTeamB;
    private float _gameTime;

    private List<int> _randomizedIndexes;
    
    private void OnEnable()
    {
        pausePanelNameTeamA.text = PlayerPrefs.GetString("TeamAName");
        pausePanelNameTeamB.text = PlayerPrefs.GetString("TeamBName");
        pausePanelWhoseeTurn.text = _isPlayingTeamB ? "Sıra: " + PlayerPrefs.GetString("TeamBName") : "Sıra: " + PlayerPrefs.GetString("TeamAName");

        _gameTime = PlayerPrefs.GetInt("GameTime");
        currentTimeLeftText.text = TimeSpan.FromSeconds(_gameTime).ToString("m\\:ss");

        _currentScore = 0;
        currentScoreText.text = "0";

        SetPassLimit();
    }
    
    private void Start()
    {
        SetRandomizedWordIndexes();
    }

    private void Update()
    {
        if (_gameStarted && !_gamePaused)
        {
            _gameTime -= Time.deltaTime;
            currentTimeLeftText.text = TimeSpan.FromSeconds(_gameTime).ToString("m\\:ss");

            if (_gameTime <= 0)
            {
                GameOver();
            }
        }
    }

    private void GameOver()
    {
        _gameStarted = false;
        SetTeamScore();
        _gameTime = PlayerPrefs.GetInt("GameTime");
        _currentScore = 0;
        _isPlayingTeamB = !_isPlayingTeamB;
        pausePanelWhoseeTurn.text = _isPlayingTeamB ? "Sıra: " + PlayerPrefs.GetString("TeamBName") : "Sıra: " + PlayerPrefs.GetString("TeamAName");
        _gameIterationCount++;
        pausePanel.SetActive(true);
        if (_gameIterationCount % 2 == 0)
        {
            GoogleAdMobController.Instance.ShowInterstitialAd();
        }
    }

    private void ResetGame()
    {
        _gameStarted = false;
        SetPassLimit();
        _gameTime = PlayerPrefs.GetInt("GameTime");
        _currentScore = 0;
        currentScoreText.text = _currentScore.ToString();
        _scoreTeamA = 0;
        _scoreTeamB = 0;
        pausePanelScoreTeamA.text = _scoreTeamA.ToString();
        pausePanelScoreTeamB.text = _scoreTeamB.ToString();
        _isPlayingTeamB = false;
    }

    private void SetTeamScore()
    {
        if (_isPlayingTeamB)
        {
            _scoreTeamB += _currentScore;
            pausePanelScoreTeamB.text = _scoreTeamB.ToString();
        }
        else // !(_isPlayingTeamB) --> Team A plays
        {
            _scoreTeamA += _currentScore;
            pausePanelScoreTeamA.text = _scoreTeamA.ToString();
        }
    }

    private void SetPassLimit()
    {
        _currentPassLimit = PlayerPrefs.GetInt("PassLimit");
        currentPassLimitText.text = _currentPassLimit.ToString();
    }

    private void LoadNextWord()
    {
        mainWordText.text = WordListController.Words[_randomizedIndexes[_currentWordIndex]].Word;
        for (int i = 0; i < 5; i++)
        {
            tabooWordsTexts[i].text = WordListController.Words[_randomizedIndexes[_currentWordIndex]].TabooWords[i];
        }
        if(_currentWordIndex != _randomizedIndexes.Count - 1)
            _currentWordIndex++;
        else
        {
            _currentWordIndex = 0;
            SetRandomizedWordIndexes();
        }
    }

    private void SetRandomizedWordIndexes()
    {
        _randomizedIndexes = Enumerable.Range(0, WordListController.Words.Count).ToList();
        System.Random random = new();

        for (int i = 0; i < _randomizedIndexes.Count; i++)
        {
            int j = random.Next(i + 1);
            (_randomizedIndexes[j], _randomizedIndexes[i]) = (_randomizedIndexes[i], _randomizedIndexes[j]);
        }
    }

    public void OnStartButtonClicked()
    {
        if (!_gameStarted)
        {
            _gameStarted = true;
            LoadNextWord();
        }
        if (_gamePaused)
        {
            _gamePaused = false;
            _gameStarted = true;
        }
        _currentPassLimit = PlayerPrefs.GetInt("PassLimit");
        currentPassLimitText.text = _currentPassLimit.ToString();
        currentScoreText.text = _currentScore.ToString();
        pausePanel.SetActive(false);
    }

    public void OnMenuButtonClicked()
    {
        ResetGame();
        gameObject.SetActive(false);
    }

    public void OnTimeButtonClicked()
    {
        pausePanel.SetActive(true);
        _gamePaused = true;
    }

    public void OnTabooButtonClicked()
    {
        _currentScore -= 1;
        LoadNextWord();
        currentScoreText.text = _currentScore.ToString();
    }

    public void OnPassButtonClicked()
    {
        if(_currentPassLimit > 0)
        {
            _currentPassLimit -= 1;
            LoadNextWord();
            currentPassLimitText.text = _currentPassLimit.ToString();
        }
    }

    public void OnCorrectButtonClicked()
    {
        LoadNextWord();
        _currentScore += 1;
        currentScoreText.text = _currentScore.ToString();
    }
}
