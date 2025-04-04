using UnityEngine;
using UnityEngine.Serialization;

namespace InteractionSystem
{

    [CreateAssetMenu(fileName = "Interaction", menuName = "Scriptable Objects/Interaction")]
    public class Interaction : ScriptableObject
    {
        public string Name;
        public string Description; // optional, might be needed
        public Sprite Icon; // optional, might be needed
        public string Prompt; // to be displayed on the HUD

        public bool RequiresInHand;
        public bool RequiresInWorld;

        public InteractionCategory Category; // primary, secondary, tetriary etc
        public InteractionTargeting InteractionTargeting;
        
        public bool VicinityBased;
        public float InteractionDistance; // zero or infinity means no distance needed
        public float InteractionRadius; // zero means raycast, positive value means spherecast
    }
}