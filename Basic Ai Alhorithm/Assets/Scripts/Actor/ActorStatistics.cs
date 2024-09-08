using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Actor
{
    [CreateAssetMenu(menuName = "Custom/Actor/Create New Actor Statistics", fileName ="NewActorStatistics")]
    public class ActorStatistics : ScriptableObject
    {
        public float mvSpeed;
    }
}
