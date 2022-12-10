using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class OptionsPanel : MonoBehaviour
{
    [SerializeField] private TMP_InputField teamANameInputField;
    [SerializeField] private TMP_InputField teamBNameInputField;

    [SerializeField] private TextMeshProUGUI teamAName;
    [SerializeField] private TextMeshProUGUI teamBName;

    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private Slider timeSlider;

    [SerializeField] private TextMeshProUGUI passText;
    [SerializeField] private Slider passSlider;

    private int gameTime;
    private int passLimit;

    private void OnEnable()
    {
        teamAName.text = PlayerPrefs.GetString("TeamAName", teamAName.text);
        teamBName.text = PlayerPrefs.GetString("TeamBName", teamBName.text);
        SetTime();
        SetPassLimit();
        SaveOptions();
    }

    private void OnDisable()
    {
        SaveOptions();
    }

    public void SetNameTeamA()
    {
        teamAName.text = teamANameInputField.text;
    }

    public void SetNameTeamB()
    {
        teamBName.text = teamBNameInputField.text;
    }

    public void SetTime()
    {
        gameTime = (int)timeSlider.value * 15;

        timeText.text = TimeSpan.FromSeconds(gameTime).ToString("mm\\:ss");
    }

    public void SetPassLimit()
    {
        passText.text = passSlider.value.ToString();
        passLimit = (int)passSlider.value;
    }

    public void SaveOptions()
    {
        PlayerPrefs.SetString("TeamAName", teamAName.text);
        PlayerPrefs.SetString("TeamBName", teamBName.text);
        PlayerPrefs.SetInt("GameTime", gameTime);
        PlayerPrefs.SetInt("PassLimit", passLimit);
    }

    public void OnBackButtonClicked()
    {
        gameObject.SetActive(false);
    }
}
