using Newtonsoft.Json;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using TMPro;
using System.IO;

public class MainMenu : MonoBehaviour
{
    private string jsonURL;
    private string jsonLocalPath;

    [SerializeField] private GameObject updateWordsArea;
    [SerializeField] private Animator updateWordsAreaButtonAnim;
    [SerializeField] private Animator updateWordsAreaTextAnim;
    [SerializeField] private TextMeshProUGUI requestStatusText;

    private void Awake()
    {
        if (!PlayerPrefs.HasKey("PassLimit"))
            PlayerPrefs.SetInt("PassLimit", 3);

        if (!PlayerPrefs.HasKey("GameTime"))
            PlayerPrefs.SetInt("GameTime", 90);

        if (!PlayerPrefs.HasKey("TeamAName"))
            PlayerPrefs.SetString("TeamAName", "Takım A");

        if (!PlayerPrefs.HasKey("TeamBName"))
            PlayerPrefs.SetString("TeamBName", "Takım B");
    }

    [Obsolete]
    private void OnEnable()
    {
        jsonLocalPath = Path.Combine(Application.persistentDataPath, "words-tr.json");

        GetJsonFromLocal();
        StartCoroutine(WordListUpdateCheck());
    }

    [Obsolete]
    public void OnUpdateWordsButtonClicked()
    {
        StartCoroutine(GetJsonFromURL());
    }

    [Obsolete]
    private IEnumerator GetJsonFromURL()
    {
        // URL of json file
        jsonURL = "https://raw.githubusercontent.com/FarukKayaduman/taboo-word-game/main/Assets/Resources/words-tr.json";

        UnityWebRequest requestToGetJsonString = UnityWebRequest.Get(jsonURL); // Create web request.
        yield return requestToGetJsonString.SendWebRequest();

        requestStatusText.text = requestToGetJsonString.result.ToString(); // Temp
        
        if (requestToGetJsonString.error != null)
        {
            Debug.LogError(requestToGetJsonString.error);
        }
        else
        {
            GameManager.jsonString = requestToGetJsonString.downloadHandler.text; // Assign requested text to jsonString variable.
            GameManager.tabooData = JsonConvert.DeserializeObject<List<GameManager.TabooData>>(GameManager.jsonString); // Deserialize jsonString to TabooData class.

            if (!File.Exists(jsonLocalPath)) // If json file doesn't exist, create and write jsonString to it.
            {
                File.CreateText(jsonLocalPath).Close();
                File.WriteAllText(jsonLocalPath, GameManager.jsonString);
            }
            else 
            {
                File.WriteAllText(jsonLocalPath, GameManager.jsonString); // If json file exists, just write jsonString to it.
            }

            updateWordsAreaButtonAnim.Play("UpdateWordsButtonExit");
            updateWordsAreaTextAnim.Play("UpdateWordsTextExit");
            updateWordsArea.SetActive(false);
        }
    }

    private void GetJsonFromLocal()
    {
        if (!File.Exists(jsonLocalPath)) // If json file doesn't exist on jsonLocalPath path.
        {
            File.CreateText(jsonLocalPath).Close(); // Create new text file on jsonLocalPath path.
            TextAsset jsonTextAsset = Resources.Load("words-tr") as TextAsset; // Read current json file as TextAsset. (Placed in Assets/Resources/).
            GameManager.jsonString = jsonTextAsset.text;
            File.WriteAllText(jsonLocalPath, GameManager.jsonString); // Write jsonString to new-created text (json) file.
        }
        else
        {
#if UNITY_EDITOR_WIN
            GameManager.jsonString = (Resources.Load("words-tr") as TextAsset).text; // If json file exist, just read text (json) file.
#endif

#if UNITY_ANDROID
            GameManager.jsonString = File.ReadAllText(jsonLocalPath); // If json file exist, just read text (json) file.
#endif
        }

        // Deserialize jsonString to TabooData class.
        GameManager.tabooData = JsonConvert.DeserializeObject<List<GameManager.TabooData>>(GameManager.jsonString);
    }

    private IEnumerator WordListUpdateCheck()
    {
        string jsonStringLocal = "";
        string jsonStringRemote = "";

#if UNITY_EDITOR_WIN
        jsonStringLocal = (Resources.Load("words-tr") as TextAsset).text;
#endif

#if UNITY_ANDROID
        jsonStringLocal = File.ReadAllText(jsonLocalPath);
#endif

        // URL of json file
        jsonURL = "https://raw.githubusercontent.com/FarukKayaduman/taboo-word-game/main/Assets/Resources/words-tr.json";

        UnityWebRequest requestToGetJsonString = UnityWebRequest.Get(jsonURL); // Create web request.
        yield return requestToGetJsonString.SendWebRequest();
        jsonStringRemote = requestToGetJsonString.downloadHandler.text; // Assign requested text to jsonString variable.

        if (jsonStringLocal != jsonStringRemote)
        {
            updateWordsArea.SetActive(true);
        }
        else
        {
            updateWordsAreaButtonAnim.Play("UpdateWordsButtonExit");
            updateWordsAreaTextAnim.Play("UpdateWordsTextExit");
            updateWordsArea.SetActive(false);
        }
    }

    public void OnNewGameButtonClicked()
    {
        UIManager.Instance.GameplayUI.SetActive(true);
    }

    public void OnOptionsButtonClicked()
    {
        UIManager.Instance.OptionsPanel.SetActive(true);
    }

    public void OnAboutButtonClicked()
    {
        UIManager.Instance.AboutPanel.SetActive(true);
    }
}
