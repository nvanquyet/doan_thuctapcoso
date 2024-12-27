using ShootingGame;
using UnityEngine;

public partial class UserData {
    public static void SetOwnerCharacter(int id, bool isOwner = true)
    {
        if (id < 0 || id >= GameData.Instance.Players.GetAllValue().Length)
        {
            Debug.LogError("Invalid character id");
            return;
        }
        PlayerPrefs.SetInt($"OwnerCharacter_{id}", isOwner ? 1 : 0);
    }

    public static bool GetOwnerCharacter(int id, bool firstCheck = false)
    {
        if (id < 0 || id >= GameData.Instance.Players.GetAllValue().Length)
        {
            Debug.LogError("Invalid character id");
            return false;
        }
        if (id == 0) return true;
        var result = PlayerPrefs.GetInt($"OwnerCharacter_{id}", 0) == 1;
        if (firstCheck)
        {
            var player = GameData.Instance.Players.GetValue(id);
            if (player != null && player.IsOwn != result) player.IsOwn = result;
        }
        return result;
    }
}
