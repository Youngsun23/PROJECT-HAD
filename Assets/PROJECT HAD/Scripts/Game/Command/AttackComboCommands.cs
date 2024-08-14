using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace HAD
{
    public abstract class AttackComboCommands : ICommand
    {
        protected readonly CharacterBase character;

        protected AttackComboCommands(CharacterBase character) 
        {
            this.character = character;
        }

        public abstract Task Execute();

        public static T Create<T>(CharacterBase character) where T : AttackComboCommands
        {
            return (T) System.Activator.CreateInstance(typeof(T), character);
        }
    }
}
