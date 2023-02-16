using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public class TabooData
    {
        public string Word { get; set; }
        public List<string> TabooWords { get; set; }
    }

    public static List<TabooData> tabooData;

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

    private bool isPlayingTeamB;

    private bool gameStarted;
    private bool gamePaused;
    private int currentScore;
    private int currentPassLimit;
    private int currentWordIndex;
    private float gameTime;

    public static string jsonString;
    private List<int> randomizedIndexes;

    private void Awake()
    {
        SetRandomizedWordIndexes();
    }

    private void OnEnable()
    {
        if (!PlayerPrefs.HasKey("ScoreTeamA"))
            PlayerPrefs.SetInt("ScoreTeamA", 0);

        if (!PlayerPrefs.HasKey("ScoreTeamB"))
            PlayerPrefs.SetInt("ScoreTeamB", 0);

        pausePanelNameTeamA.text = PlayerPrefs.GetString("TeamAName");
        pausePanelNameTeamB.text = PlayerPrefs.GetString("TeamBName");
        pausePanelWhoseeTurn.text = isPlayingTeamB ? "Sıra: " + PlayerPrefs.GetString("TeamBName") : "Sıra: " + PlayerPrefs.GetString("TeamAName");

        gameTime = PlayerPrefs.GetInt("GameTime");
        currentTimeLeftText.text = TimeSpan.FromSeconds(gameTime).ToString("m\\:ss");

        currentScore = 0;
        currentScoreText.text = "0";

        SetPassLimit();
        
    }

    private void Update()
    {
        if (gameStarted && !gamePaused)
        {
            gameTime -= Time.deltaTime;
            currentTimeLeftText.text = TimeSpan.FromSeconds(gameTime).ToString("m\\:ss");

            if (gameTime <= 0)
            {
                GameOver();
            }
        }
    }

    private void GameOver()
    {
        gameStarted = false;
        SetTeamScore();
        gameTime = PlayerPrefs.GetInt("GameTime");
        currentScore = 0;
        isPlayingTeamB = !isPlayingTeamB;
        pausePanelWhoseeTurn.text = isPlayingTeamB ? "Sıra: " + PlayerPrefs.GetString("TeamBName") : "Sıra: " + PlayerPrefs.GetString("TeamAName");
        pausePanel.SetActive(true);
    }

    private void ResetGame()
    {
        gameStarted = false;
        SetPassLimit();
        gameTime = PlayerPrefs.GetInt("GameTime");
        currentScore = 0;
        currentScoreText.text = currentScore.ToString();
        PlayerPrefs.SetInt("ScoreTeamA", 0);
        PlayerPrefs.SetInt("ScoreTeamB", 0);
        pausePanelScoreTeamA.text = "0";
        pausePanelScoreTeamB.text = "0";
        isPlayingTeamB = false;
    }

    private void SetTeamScore()
    {
        if (isPlayingTeamB)
        {
            PlayerPrefs.SetInt("ScoreTeamB", PlayerPrefs.GetInt("ScoreB") + currentScore);
            pausePanelScoreTeamB.text = PlayerPrefs.GetInt("ScoreTeamB").ToString();
        }
        else // !(isPlayingTeamB) --> Team A plays
        {
            PlayerPrefs.SetInt("ScoreTeamA", PlayerPrefs.GetInt("ScoreTeamA") + currentScore);
            pausePanelScoreTeamA.text = PlayerPrefs.GetInt("ScoreTeamA").ToString();
        }
    }

    private void SetPassLimit()
    {
        currentPassLimit = PlayerPrefs.GetInt("PassLimit");
        currentPassLimitText.text = currentPassLimit.ToString();
    }

    public void LoadNextWord()
    {
        mainWordText.text = tabooData[randomizedIndexes[currentWordIndex]].Word;
        for (int i = 0; i < 5; i++)
        {
            tabooWordsTexts[i].text = tabooData[randomizedIndexes[currentWordIndex]].TabooWords[i];
        }
        if(currentWordIndex != randomizedIndexes.Count - 1)
            currentWordIndex++;
        else
        {
            currentWordIndex = 0;
            SetRandomizedWordIndexes();
        }
    }

    private void SetRandomizedWordIndexes()
    {
        randomizedIndexes = Enumerable.Range(0, tabooData.Count).ToList();
        System.Random random = new();

        for (int i = 0; i < randomizedIndexes.Count; i++)
        {
            int j = random.Next(i + 1);
            (randomizedIndexes[j], randomizedIndexes[i]) = (randomizedIndexes[i], randomizedIndexes[j]);
        }
    }

    public void OnStartButtonClicked()
    {
        if (!gameStarted)
        {
            gameStarted = true;
            LoadNextWord();
        }
        if (gamePaused)
        {
            gamePaused = false;
            gameStarted = true;
        }
        currentPassLimit = PlayerPrefs.GetInt("PassLimit");
        currentPassLimitText.text = currentPassLimit.ToString();
        currentScoreText.text = currentScore.ToString();
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
        gamePaused = true;
    }

    public void OnTabooButtonClicked()
    {
        currentScore -= 1;
        LoadNextWord();
        currentScoreText.text = currentScore.ToString();
    }

    public void OnPassButtonClicked()
    {
        if(currentPassLimit > 0)
        {
            currentPassLimit -= 1;
            LoadNextWord();
            currentPassLimitText.text = currentPassLimit.ToString();
        }
    }

    public void OnCorrectButtonClicked()
    {
        LoadNextWord();
        currentScore += 1;
        currentScoreText.text = currentScore.ToString();
    }
}
