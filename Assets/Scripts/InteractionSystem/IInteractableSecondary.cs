namespace InteractionSystem
{
    public interface IInteractableSecondary
    {
        public void InteractSecondary(InteractionContext context);
        public string SecondaryInteractionPrompt();
    }
}
