using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HAD
{
    public interface IInteractable
    {
        bool IsAutomaticInteraction { get; }

        void Interact(CharacterBase actor);
    }
}
