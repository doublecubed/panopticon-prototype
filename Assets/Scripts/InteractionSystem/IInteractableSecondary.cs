namespace InteractionSystem
{
    public interface IInteractableSecondary
    {
        public InteractableInfo GetInfoSecondary(InteractionContext context);
        public void InteractSecondary(InteractionContext context);
    }
}
