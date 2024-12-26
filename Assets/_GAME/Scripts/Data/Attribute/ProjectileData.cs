using ShootingGame;
using ShootingGame.Data;
using UnityEngine;

[System.Serializable]
public struct ProjectileStruct
{
    public Projectile Prefab;
    public bool IsOwner;
}

[CreateAssetMenu(fileName = "ProjectileData", menuName = "_GAME/ProjectileData")]
public class ProjectileData : BaseIntKeyData<ProjectileStruct>
{
    protected override string Path => "Assets/_GAME/Prefabs/Objects/Projectile/New";
}
