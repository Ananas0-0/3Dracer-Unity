using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MoneyManager : MonoBehaviour
{
   [SerializeField] private TMP_Text currentMoneyText;
   public Button AddMoneyButton;
   private int currentMoney;

   public static MoneyManager instance = null;
   //public int money;


   private void Start() {
      if (instance == null) {
         instance = this;
      }

      AddMoneyButton.onClick.AddListener(delegate { AddMoney(100); });
      UpdateMoney();
   }

   public void UpdateMoney() {
      currentMoney = PlayerPrefs.GetInt("Money", 0);
      currentMoneyText.text = currentMoney.ToString();
   }

   public void AddMoney(int money) {
      currentMoney = currentMoney + money;
      Debug.Log("Will added money:" + money);

      PlayerPrefs.SetInt("Money", currentMoney);
      currentMoneyText.text = currentMoney.ToString();
   }

}
