using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private GameObject GameUIPrefab;
    [SerializeField] private GameObject MainMenuPrefab;
    [SerializeField] private GameObject OptionsPanelPrefab;
    
    [HideInInspector] public GameObject GameplayUI;
    [HideInInspector] public GameObject MainMenuUI;
    [HideInInspector] public GameObject OptionsPanel;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        MainMenuUI = Instantiate(MainMenuPrefab, transform);
        OptionsPanel = Instantiate(OptionsPanelPrefab, transform);
        GameplayUI = Instantiate(GameUIPrefab, transform);

        MainMenuUI.SetActive(true);
        OptionsPanel.SetActive(false);
        GameplayUI.SetActive(false);
    }
}
