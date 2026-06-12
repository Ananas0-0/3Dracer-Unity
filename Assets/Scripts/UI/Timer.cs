using UnityEngine;
using TMPro;
using Mirror;

public class Timer : MonoBehaviour
{
    private TMP_Text timerText;
    private bool localControlsEnabled;

    void Start()
    {
        timerText = GetComponent<TMP_Text>();

        // SP: стартуем как раньше, но без FindObjectsOfType
        if (!NetworkClient.active)
        {
            StartCoroutine(SinglePlayerCountdown());
        }
    }

    void Update()
    {
        // MP: время берём только из RaceState/NetworkTime
        if (NetworkClient.active)
        {
            if (RaceState.Instance == null)
                return;

            double tts = RaceState.Instance.TimeToStart();

            if (tts > 0)
            {
                timerText.color = Color.red;
                int sec = Mathf.CeilToInt((float)tts);
                timerText.text = $"00:0{Mathf.Clamp(sec, 0, 9)}";

                SetLocalCarFrozen(true);
                localControlsEnabled = false;
            }
            else
            {
                timerText.color = Color.white;

                double elapsed = RaceState.Instance.ElapsedRaceTime();
                int minutes = Mathf.FloorToInt((float)(elapsed / 60.0));
                int seconds = Mathf.FloorToInt((float)(elapsed % 60.0));
                timerText.text = $"{minutes:00}:{seconds:00}";

                SetLocalCarFrozen(false);

                if (!localControlsEnabled)
                {
                    SetLocalInputEnabled(true);
                    localControlsEnabled = true;
                }
            }
        }
    }

    // ===== SP =====

    System.Collections.IEnumerator SinglePlayerCountdown()
    {
        // В SP можно оставить твою логику, но без постоянного ToggleCarKinematic в Update()
        SetAllCarsFrozen(true);
        SetAllInputsEnabled(false);

        for (int i = 5; i >= 0; i--)
        {
            timerText.color = Color.red;
            timerText.text = "00:0" + i;
            yield return new WaitForSeconds(1f);
        }

        timerText.color = Color.white;

        SetAllCarsFrozen(false);
        SetAllInputsEnabled(true);

        float startTime = Time.time;
        while (true)
        {
            float elapsed = Time.time - startTime;
            int minutes = Mathf.FloorToInt(elapsed / 60f);
            int seconds = Mathf.FloorToInt(elapsed % 60f);
            timerText.text = $"{minutes:00}:{seconds:00}";
            yield return null;
        }
    }

    // ===== helpers =====

    void SetLocalInputEnabled(bool enabled)
    {
        if (NetworkClient.localPlayer == null) return;

        var input = NetworkClient.localPlayer.GetComponentInChildren<PlayerCarHandler>(true);
        if (input != null) input.enabled = enabled;
    }

    void SetLocalCarFrozen(bool frozen)
    {
        if (NetworkClient.localPlayer == null) return;

        var rb = NetworkClient.localPlayer.GetComponentInChildren<Rigidbody>();
        if (rb != null) rb.isKinematic = frozen;
    }

    void SetAllInputsEnabled(bool enabled)
    {
        foreach (var p in FindObjectsOfType<PlayerCarHandler>()) p.enabled = enabled;
        foreach (var ai in FindObjectsOfType<AICarHandler>()) ai.enabled = enabled;
    }

    void SetAllCarsFrozen(bool frozen)
    {
        foreach (var p in FindObjectsOfType<PlayerCarHandler>())
        {
            var rb = p.GetComponent<Rigidbody>();
            if (rb) rb.isKinematic = frozen;
        }

        foreach (var ai in FindObjectsOfType<AICarHandler>())
        {
            var rb = ai.GetComponent<Rigidbody>();
            if (rb) rb.isKinematic = frozen;
        }
    }
}