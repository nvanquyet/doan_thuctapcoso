using System.Collections;
using UnityEngine;

namespace ShootingGame
{

    public enum TypeAttack
    {
        Melee,
        Ranged
    }
    public class Boss : AInteractable<BoxCollider2D>
    {
        [SerializeField] private BossAttacker _bossAttacker;
        [SerializeField] private ADefender _bossDefender;
        [SerializeField] private BossMovement _bossMovement;

        [SerializeField] private TypeAttack typeAttack;
        public override void ExitInteract(Interface.IInteract target)
        {
           
        }

        public override void OnInteract(Interface.IInteract target)
        {

        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}