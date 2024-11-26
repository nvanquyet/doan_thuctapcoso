using System.Collections.Generic;
using ShootingGame;
using ShootingGame.Data;

public sealed class GameEvent
{

    public struct OnWaveClear : IEventParam { }
    public struct OnNextWave : IEventParam {
        public List<int> allIDItem;
    }

    public struct OnPlayerDead : IEventParam { }
}
