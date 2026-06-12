public enum GameMode
{
    SinglePlayer,
    MultiPlayer
}

public static class GameSession
{
    public static GameMode CurrentMode = GameMode.SinglePlayer;
}