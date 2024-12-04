using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HAD
{
    public interface IInteractable
    {
        bool IsAutomaticInteraction { get; }

        string Message {  get; }

        void Interact(CharacterBase actor);
        void InteractEnable(); // Auto 아닌 애들용 (안 쓰는 애는 비워두면 되는 거 아닌가?)
        void InteractDisenable();
    }
}
