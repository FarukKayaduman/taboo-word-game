using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private const string GameScene = "GameScene";

    private void Awake()
    {
        StartCoroutine(LoadGameScene());
    }

    private static IEnumerator LoadGameScene()
    {
        var operation = SceneManager.LoadSceneAsync(GameScene, LoadSceneMode.Additive);
        operation.allowSceneActivation = false;
        
        yield return new WaitUntil(() => !operation.isDone);

        operation.allowSceneActivation = true;
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(GameScene));
    }
}
