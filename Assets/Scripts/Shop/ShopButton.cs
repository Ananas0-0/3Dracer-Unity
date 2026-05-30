using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopButton : MonoBehaviour
{
    public int carPrice = 100; // Цена машинки
    public Button buyButton; // Кнопка покупки
    public TMP_Text statusText; // Текстовое поле для вывода статуса покупки
    //public GameObject cars; // Массив доступных машинок
    public int CarIndex = 0;
    //public GameObject PointSpawnCar;

    private ShopManager manager;
    private void Start()
    {
        buyButton.onClick.AddListener(BuyCar);

        PlayerPrefs.SetInt("CarPurchased_" + 0, 1);
        //Instantiate(cars, PointSpawnCar.transform);
        UpdateInfo();
        
    }

    public void SetManager(ShopManager shopManager)
    {
        manager = shopManager;
    }

    public void UpdateInfo()
    {
        int currentCarIndex = PlayerPrefs.GetInt("CurrentCar", 0);
        
        if (currentCarIndex == CarIndex)
        {
            statusText.text = "Selected";
            buyButton.GetComponent<Image>().color = new Color32(129, 255, 236, 255);
        }
        else if (PlayerPrefs.GetInt("CarPurchased_" + CarIndex, 0) == 1)
        {
            statusText.text = "Purchased";
            buyButton.GetComponent<Image>().color = new Color32(148, 255, 129, 255);
        }
        else
        {
            statusText.text = carPrice.ToString();
        }
    }

    public void BuyCar()
    {
        int money = PlayerPrefs.GetInt("Money", 0);

        if (PlayerPrefs.GetInt("CarPurchased_" + CarIndex, 0) != 1)
        {
            if (money >= carPrice)
            {
                money -= carPrice;
                PlayerPrefs.SetInt("Money", money);
                PlayerPrefs.SetInt("CarPurchased_" + CarIndex, 1);

                // Если машшина только что куплена, установим ее как выбранную
                PlayerPrefs.SetInt("CurrentCar", CarIndex);

                manager.UpdateInfoButton();
            }
            else
            {
                Debug.Log("don't much money");
            }

        }
        else
        {
            Debug.Log("Wass bought. Selecting...");
            PlayerPrefs.SetInt("CurrentCar", CarIndex);
            manager.UpdateInfoButton();
        }
    }

}