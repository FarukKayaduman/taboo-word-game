using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsPanel : MonoBehaviour
{
    [SerializeField] private TMP_InputField teamANameInputField;
    [SerializeField] private TMP_InputField teamBNameInputField;

    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private Slider timeSlider;

    [SerializeField] private TextMeshProUGUI passText;
    [SerializeField] private Slider passSlider;

    [SerializeField] private TextMeshProUGUI gameVersionText;
    [SerializeField] private TextMeshProUGUI wordsListVersionText;
    
    private int _gameTime;
    private int _passLimit;
    
    private void OnEnable()
    {
        teamANameInputField.text = PlayerPrefs.GetString("TeamAName");
        teamBNameInputField.text = PlayerPrefs.GetString("TeamBName");
        SetVersionTexts();
        SetTime();
        SetPassLimit();
    }

    public void SetTime()
    {
        _gameTime = (int)timeSlider.value * 15;
        timeText.text = TimeSpan.FromSeconds(_gameTime).ToString("mm\\:ss");
    }

    public void SetPassLimit()
    {
        passText.text = $"{passSlider.value}";
        _passLimit = (int)passSlider.value;
    }

    private void SetVersionTexts()
    {
        gameVersionText.text = $"Sürüm: v{Application.version}";
        wordsListVersionText.text = $"Kelime Listesi: v{WordListController.WordsVersionLocal}";
    }

    private void SaveOptions()
    {
        PlayerPrefs.SetString("TeamAName", teamANameInputField.text);
        PlayerPrefs.SetString("TeamBName", teamBNameInputField.text);
        PlayerPrefs.SetInt("GameTime", _gameTime);
        PlayerPrefs.SetInt("PassLimit", _passLimit);
    }

    public void OnBackButtonClicked()
    {
        SaveOptions();
        gameObject.SetActive(false);
    }
}
