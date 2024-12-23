using ShootingGame;
using ShootingGame.Data;
using UnityEngine;
[CreateAssetMenu(fileName = "ProjectileData", menuName = "_GAME/ProjectileData")]
public class ProjectileData : BaseIntKeyData<Projectile>
{
    protected override string Path => "Assets/_GAME/Prefabs/Objects/Projectile/New";
}
