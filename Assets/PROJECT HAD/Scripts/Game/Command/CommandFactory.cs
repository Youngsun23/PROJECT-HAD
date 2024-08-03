using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HAD
{
    public class CommandFactory : MonoBehaviour
    {
        public static ICommand CreateCommand(CharacterBase character, int comboStep)
        {
            switch (comboStep)
            {
                case 0:
                    return new AttackCombo1Command(character);
                case 1:
                    return new AttackCombo2Command(character);
                case 2:
                    return new AttackCombo3Command(character);
                default:
                    return null;
            }
        }
    }
}
