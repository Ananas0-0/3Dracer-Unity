using Mirror;
using UnityEngine;

public class RaceState : NetworkBehaviour
{
    public static RaceState Instance;

    [SyncVar] public double raceStartTime; // NetworkTime когда гонка началась
    [SyncVar] public bool raceStarted;

    // Пауза
    [SyncVar] private double pausedAt;
    [SyncVar] private double pauseAccum;

    [SyncVar(hook = nameof(OnPausedChanged))]
    public bool paused;

    void Awake() => Instance = this;

    public override void OnStartServer()
    {
        Time.timeScale = 1f; // ✅ сброс на сервере

        raceStartTime = NetworkTime.time + 5.0;
        raceStarted = false;

        paused = false;
        pausedAt = 0;
        pauseAccum = 0;
    }

    [ServerCallback]
    void Update()
    {
        if (!raceStarted && NetworkTime.time >= raceStartTime)
            raceStarted = true;
    }

    // ==== Пауза (сервер) ====
    [Server]
    public void SetPaused(bool value)
    {
        if (paused == value) return;

        if (value)
        {
            pausedAt = NetworkTime.time;
            paused = true;
        }
        else
        {
            if (pausedAt > 0)
                pauseAccum += NetworkTime.time - pausedAt;

            pausedAt = 0;
            paused = false;
        }
    }

    // ==== Пауза (всем клиентам) ====
    void OnPausedChanged(bool oldValue, bool newValue)  { Time.timeScale = newValue ? 0f : 1f;  }

    public override void OnStartClient() { Time.timeScale = 1f; }

    // ==== Таймеры ====
    public double TimeToStart()
    {
        double t = raceStartTime - NetworkTime.time - PauseDeltaNow();
        return t < 0 ? 0 : t;
    }

    public double ElapsedRaceTime()
    {
        double t = NetworkTime.time - raceStartTime - pauseAccum - PauseDeltaNow();
        return t < 0 ? 0 : t;
    }

    double PauseDeltaNow() { return (paused && pausedAt > 0) ? (NetworkTime.time - pausedAt) : 0; }
}