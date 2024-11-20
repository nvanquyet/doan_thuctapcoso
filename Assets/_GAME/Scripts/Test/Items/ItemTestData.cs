
using UnityEngine;
namespace ShootingGame.Data
{
    [CreateAssetMenu(fileName = "ItemData", menuName = "Test/ItemTestData")]
    public class ItemTestData : BaseIntKeyData<StatContainerData>
    {
        protected override string Path => throw new System.NotImplementedException();
    }
}
