using System;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

[Serializable]
public class PurchaseInfo
{
    public bool adsRemoved;
}

public class IAPManager : MonoBehaviour
{
    [SerializeField] private PurchaseInfoSO purchaseInfoSO;
    
    private void Awake()
    {
        LoadPurchaseInfo();
        MainMenu.Instance.SetRemoveAdsPanelVisibility(!purchaseInfoSO.RemoveAdsPurchased);
    }
    
    private void LoadPurchaseInfo()
    {
        string purchaseInfoPath = Path.Combine(Application.persistentDataPath, "PurchaseInfo/PurchaseInfo.json");
        string purchaseInfoJsonString = "";
        
        if (File.Exists(purchaseInfoPath))
        {
            purchaseInfoJsonString = File.ReadAllText(purchaseInfoPath);
        }
        else
        {
            TextAsset jsonTextAsset = Resources.Load("PurchaseInfo/PurchaseInfo") as TextAsset;
            if (jsonTextAsset != null)
                purchaseInfoJsonString = jsonTextAsset.text;

            string directoryPath = Path.Combine(Application.persistentDataPath, "PurchaseInfo");
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);
            File.WriteAllText(purchaseInfoPath, purchaseInfoJsonString);
        }
        
        PurchaseInfo purchaseInfo = JsonConvert.DeserializeObject<PurchaseInfo>(purchaseInfoJsonString);
        purchaseInfoSO.RemoveAdsPurchased = purchaseInfo.adsRemoved;
    }

    private void SaveRemoveAdsPurchaseData(bool isPurchased = true)
    {
        PurchaseInfo purchaseInfo;
        string purchaseInfoPath = Path.Combine(Application.persistentDataPath, "PurchaseInfo/PurchaseInfo.json");
        string purchaseInfoJsonString = "";
        
        if (File.Exists(purchaseInfoPath))
        {
            purchaseInfoJsonString = File.ReadAllText(purchaseInfoPath);
            purchaseInfo = JsonConvert.DeserializeObject<PurchaseInfo>(purchaseInfoJsonString);
            purchaseInfo.adsRemoved = isPurchased;
            purchaseInfoJsonString = JsonConvert.SerializeObject(purchaseInfo);
        }
        else
        {
            TextAsset jsonTextAsset = Resources.Load("PurchaseInfo/PurchaseInfo") as TextAsset;
            if (jsonTextAsset != null)
                purchaseInfoJsonString = jsonTextAsset.text;

            string directoryPath = Path.Combine(Application.persistentDataPath, "PurchaseInfo");
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);
        }
        
        File.WriteAllText(purchaseInfoPath, purchaseInfoJsonString);
        purchaseInfo = JsonConvert.DeserializeObject<PurchaseInfo>(purchaseInfoJsonString);
        purchaseInfoSO.RemoveAdsPurchased = purchaseInfo.adsRemoved;
    }
    
    public void OnPurchaseSuccess(Product product)
    {
        MainMenu.Instance.SetRemoveAdsPanelVisibility(false);
        SaveRemoveAdsPurchaseData();
    }
    
    public void OnPurchaseFail(Product product, PurchaseFailureDescription purchaseFailureDescription)
    {
        MainMenu.Instance.SetRemoveAdsPanelVisibility(!product.availableToPurchase);
    }
}
