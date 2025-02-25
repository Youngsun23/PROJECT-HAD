namespace HAD
{
    public interface IInteractable
    {
        bool IsAutomaticInteraction { get; }
        string Message {  get; }

        void Interact(CharacterBase actor);
        void InteractEnable(); 
        void InteractDisenable();
    }
}
