using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private GameObject GameUIPrefab;
    [SerializeField] private GameObject MainMenuPrefab;
    [SerializeField] private GameObject OptionsPanelPrefab;
    [SerializeField] private GameObject AboutPanelPrefab;

    [HideInInspector] public GameObject GameplayUI;
    [HideInInspector] public GameObject MainMenuUI;
    [HideInInspector] public GameObject OptionsPanel;
    [HideInInspector] public GameObject AboutPanel;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        MainMenuUI = Instantiate(MainMenuPrefab, transform);
        OptionsPanel = Instantiate(OptionsPanelPrefab, transform);
        AboutPanel = Instantiate(AboutPanelPrefab, transform);
        GameplayUI = Instantiate(GameUIPrefab, transform);

        MainMenuUI.SetActive(true);
        OptionsPanel.SetActive(false);
        AboutPanel.SetActive(false);
        GameplayUI.SetActive(false);
    }
}
