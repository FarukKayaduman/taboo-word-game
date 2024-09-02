using UnityEngine;
using AdMobController;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private PurchaseInfoSO purchaseInfoSO;
    [SerializeField] private GameObject removeAdsPanel;

    public static MainMenu Instance;
    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        
        if (!PlayerPrefs.HasKey("PassLimit"))
            PlayerPrefs.SetInt("PassLimit", 3);

        if (!PlayerPrefs.HasKey("GameTime"))
            PlayerPrefs.SetInt("GameTime", 90);

        if (!PlayerPrefs.HasKey("TeamAName"))
            PlayerPrefs.SetString("TeamAName", "Takım A");

        if (!PlayerPrefs.HasKey("TeamBName"))
            PlayerPrefs.SetString("TeamBName", "Takım B");
    }
    
    public void SetRemoveAdsPanelVisibility(bool isEnabled)
    {
        removeAdsPanel.SetActive(isEnabled);
    }

    public void OnNewGameButtonClicked()
    {
        UIManager.Instance.GameplayUI.SetActive(true);
        GoogleAdMobController.Instance.ShowInterstitialAd();
    }

    public void OnOptionsButtonClicked()
    {
        UIManager.Instance.OptionsPanel.SetActive(true);
    }
}
