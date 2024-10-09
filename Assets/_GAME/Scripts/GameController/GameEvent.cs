using System.Collections.Generic;
using ShootingGame;
using ShootingGame.Data;

public sealed class GameEvent
{
    public struct OnStatChange : IEventParam { 
        public EquiqmentStat CurrentStat;
    }

    public struct OnWaveClear : IEventParam { }
    public struct OnNextWave : IEventParam {
        public List<int> allWeaponIds;
    }

    public struct OnPlayerDead : IEventParam { }
}
