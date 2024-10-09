using ShootingGame;
using ShootingGame.Data;

public sealed class GameEvent
{
    public struct OnStatChange : IEventParam { }

    public struct OnWaveClear : IEventParam { }

    public struct OnPlayerDead : IEventParam { }
}
