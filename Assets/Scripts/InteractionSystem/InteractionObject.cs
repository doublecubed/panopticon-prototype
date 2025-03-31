using UnityEngine;

namespace InteractionSystem
{
    [CreateAssetMenu(fileName = "InteractionObject", menuName = "Scriptable Objects/InteractionObject")]
    public class InteractionObject : ScriptableObject
    {
        [Header("Basic Info")] public string Name;
        public string Description;

        [Header("Image")] public Sprite Icon;

        [Header("Prompts")] public string PickupPrompt;
        public string DropPrompt;
        public string AttachPrompt;
        public string ActivatePrompt;
        public string UsePrompt;
        public string UseOnPrompt;

    }

}