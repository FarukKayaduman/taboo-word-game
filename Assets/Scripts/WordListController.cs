using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class VersionsList
{
    public string TR;
}

[Serializable]
public class TabooData
{
    public string Word;
    public List<string> TabooWords;
}

public class WordListController : MonoBehaviour
{
    private static string _wordsVersionLocal = "0.0.2";
    public static string WordsVersionLocal => _wordsVersionLocal;
    
    private string _wordsVersionRemote;
    private string _wordsLocalPath;
    private string _wordsVersionsLocalPath;
    private string _wordsJsonString;
    
    private const string WordsJsonURL = "https://raw.githubusercontent.com/FarukKayaduman/taboo-word-game/main/Assets/Resources/Words/tr.json";
    private const string WordsVersionURL = "https://raw.githubusercontent.com/FarukKayaduman/taboo-word-game/main/Assets/Resources/versions_list.json";

    public static List<TabooData> Words { get; private set; }

    private void OnEnable()
    {
        InitPaths();
        LoadWordsFromLocal();
        StartCoroutine(CheckWordsVersion());
    }

    private void InitPaths()
    {
        _wordsLocalPath = Path.Combine(Application.persistentDataPath, "Words/tr.json");
        _wordsVersionsLocalPath = Path.Combine(Application.persistentDataPath, "versions_list.json");
    }
    
    private void LoadWordsFromLocal()
    {
        if (File.Exists(_wordsLocalPath))
        {
            _wordsJsonString = File.ReadAllText(_wordsLocalPath);
        }
        else
        {
            TextAsset jsonTextAsset = Resources.Load("Words/tr") as TextAsset;
            if (jsonTextAsset != null)
                _wordsJsonString = jsonTextAsset.text;

            string directoryPath = Path.Combine(Application.persistentDataPath, "Words");
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);
            File.WriteAllText(_wordsLocalPath, _wordsJsonString);
        }
        
        Words = JsonConvert.DeserializeObject<List<TabooData>>(_wordsJsonString);
    }
    
    private IEnumerator CheckWordsVersion()
    {
        UnityWebRequest requestToGetVersionString = UnityWebRequest.Get(WordsVersionURL);
        yield return requestToGetVersionString.SendWebRequest();
        if (requestToGetVersionString.error != null)
            yield break;
        
        string versionsJsonStringRemote = requestToGetVersionString.downloadHandler.text;
        _wordsVersionRemote = JsonConvert.DeserializeObject<VersionsList>(versionsJsonStringRemote).TR;
        
        string versionsJsonStringLocal = "";
        
        if (File.Exists(_wordsVersionsLocalPath))
        {
            versionsJsonStringLocal = File.ReadAllText(_wordsVersionsLocalPath);
        }
        else
        {
            TextAsset jsonTextAsset = Resources.Load("versions_list") as TextAsset;

            if (jsonTextAsset)
                versionsJsonStringLocal = jsonTextAsset.text;
            
            File.WriteAllText(_wordsVersionsLocalPath, versionsJsonStringLocal);
        }
        
        _wordsVersionLocal = JsonConvert.DeserializeObject<VersionsList>(versionsJsonStringLocal).TR;
        
        if (_wordsVersionLocal != _wordsVersionRemote)
        {
            StartCoroutine(GetWordsFromRemote());
        }
    }

    private IEnumerator GetWordsFromRemote()
    {
        UnityWebRequest requestToGetJsonString = UnityWebRequest.Get(WordsJsonURL);
        yield return requestToGetJsonString.SendWebRequest();

        if (requestToGetJsonString.error != null)
        {
            Debug.LogError(requestToGetJsonString.error);
        }
        else
        {
            _wordsJsonString = requestToGetJsonString.downloadHandler.text;
            Words = JsonConvert.DeserializeObject<List<TabooData>>(_wordsJsonString);

            File.WriteAllText(_wordsLocalPath, _wordsJsonString);

            _wordsVersionLocal = _wordsVersionRemote;
        }
    }
}
