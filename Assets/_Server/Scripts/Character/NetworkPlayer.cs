using System.Collections;
using System.Collections.Generic;

using Unity.Netcode;
using UnityEngine;
namespace Server
{
    [RequireComponent(typeof(NetworkPlayerMovement))]
    public class NetworkPlayer : NetworkBehaviour
    {
        [SerializeField] private NetworkPlayerMovement m_Movement;
    }

}