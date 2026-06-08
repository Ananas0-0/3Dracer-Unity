using UnityEngine;
using TMPro;
using System.Net;
using System.Net.Sockets;
using Mirror;
using UnityEngine.UI;

public class HostPanelUI : MonoBehaviour
{
    [Header("IP")]
    [SerializeField] private TMP_Text ipText;

    [Header("Player Slots")]
    [SerializeField] private CarPreviewSlot[] previewSlots;
    private int currentPlayerCount = 0;

    private void Start()
    {
        string localIP = GetLocalIPAddress();
        ipText.text = "Your IP: " + localIP;

        // NetworkManager.singleton.StartHost();
    }

    string GetLocalIPAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
                return ip.ToString();
        }
        return "Not found";
    }

    public void AddPlayerSlot(int carIndex)
    {
        if (currentPlayerCount >= previewSlots.Length)
            return;

        previewSlots[currentPlayerCount].ShowCar(carIndex);
        currentPlayerCount++;
    }

    public void RemoveLastPlayerSlot()
    {
        if (currentPlayerCount <= 0)
            return;

        currentPlayerCount--;
        previewSlots[currentPlayerCount].ClearSlot();
    }

    public void CreateHost() { NetworkManager.singleton.StartHost(); }

    private void OnDisable() { ClearAllSlots(); }

    public void ClearAllSlots()
    {
        foreach (var slot in previewSlots) { slot.ClearSlot(); }

        currentPlayerCount = 0;
    }
    public void StopHost()
    {
        NetworkManager.singleton.StopHost();
        ClearAllSlots();
    }
}