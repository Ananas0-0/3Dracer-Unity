using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NetworkFinishLineMP : NetworkBehaviour
{
    [Header("MP UI (на сцене)")]
    [SerializeField] private GameObject mpResultPanel;
    [SerializeField] private TMP_Text placeText;
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private GameObject restartButtonObject; // чтобы скрыть у клиентов

    private readonly List<uint> finishOrder = new();

    [ServerCallback]
    private void OnTriggerEnter(Collider other)
    {
        if (GameSession.CurrentMode != GameMode.MultiPlayer) return;

        NetworkIdentity id = other.GetComponentInParent<NetworkIdentity>();
        if (id == null) return;

        if (finishOrder.Contains(id.netId)) return;
        finishOrder.Add(id.netId);

        int place = finishOrder.Count;

        // время берём с сервера (если RaceState есть)
        double raceTime = 0;
        if (RaceState.Instance != null)
            raceTime = RaceState.Instance.ElapsedRaceTime();

        if (id.connectionToClient != null)
            TargetShowResult(id.connectionToClient, place, (float)raceTime);
    }

    [TargetRpc]
    private void TargetShowResult(NetworkConnectionToClient target, int place, float raceTime)
    {
        // панель только этому игроку
        if (mpResultPanel != null)
            mpResultPanel.SetActive(true);

        if (placeText != null)
            placeText.text = "Place: " + place;

        if (timeText != null)
            timeText.text = "Time: " + FormatTime(raceTime);

        // рестарт доступен всем
        if (restartButtonObject != null)
            restartButtonObject.SetActive(true);

        StopLocalCar();
    }

    private void StopLocalCar()
    {
        if (NetworkClient.localPlayer == null) return;

        var input = NetworkClient.localPlayer.GetComponentInChildren<PlayerCarHandler>(true);
        if (input != null) input.enabled = false;

        var rb = NetworkClient.localPlayer.GetComponentInChildren<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
        }
    }

    private string FormatTime(float t)
    {
        int minutes = Mathf.FloorToInt(t / 60f);
        int seconds = Mathf.FloorToInt(t % 60f);
        return $"{minutes:00}:{seconds:00}";
    }
}