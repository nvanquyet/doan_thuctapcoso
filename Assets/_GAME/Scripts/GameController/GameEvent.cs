using VawnWuyest;
using VawnWuyest.Data;

public sealed class GameEvent
{
    public struct OnStatChange : IEventParam { }

    public struct OnWaveClear : IEventParam { }
}
