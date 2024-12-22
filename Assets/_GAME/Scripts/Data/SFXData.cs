using System.Collections.Generic;
using UnityEngine;

public enum AudioEvent
{
    //Define audio type
    ButtonClick,
    VictoryMusic,
    LoseMusic,

    HomeMusic,
    InGameMusic,

    PlayerMovement,
    PlayerShoot,
    PlayerHit,
}
[System.Serializable]
public struct SoundData
{
    public AudioEvent Event;
    public AudioClip Clip;
}


[CreateAssetMenu(fileName = "SFX Data", menuName = "_GAME/SFX Data")]
public class SFXData : ScriptableObject
{
    [SerializeField] private List<SoundData> sounds;
    Dictionary<AudioEvent, AudioClip> data;

    public AudioClip GetAudioClip(AudioEvent Event)
    {
        if (data == null || data.Count == 0)
        {
            data = new();
            foreach (var s in sounds) data.Add(s.Event, s.Clip);
        }
        return data[Event];
    }
}
