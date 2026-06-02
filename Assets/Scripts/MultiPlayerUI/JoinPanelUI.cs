using Mirror;
using TMPro;
using UnityEngine;

public class JoinPanelUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField ipInputField;

    public void ConnectToHost()
    {
        string ip = ipInputField.text;

        if (string.IsNullOrWhiteSpace(ip))
        {
            Debug.LogWarning("IP is empty!");
            return;
        }

        NetworkManager.singleton.networkAddress = ip;
        NetworkManager.singleton.StartClient();
    }
}