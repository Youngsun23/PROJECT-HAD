using System.Collections.Generic;
using UnityEngine;

namespace HAD
{
    public class CharacterAbilityComponent : MonoBehaviour
    {
        public List<AbilityBase> abilities = new List<AbilityBase>();
        private CharacterBase character;

        private void Awake()
        {
            character = GetComponent<CharacterBase>(); 
        }

        public void AddAbility(AbilityBase newAbility)
        {
            abilities.Add(newAbility);
            newAbility.SetOwner(character);

            var abilityUI = UIManager.Singleton.GetUI<AbilityListUI>(UIList.AbilityList);
            abilityUI.Hide();
            abilityUI.RegisterAbility(newAbility);
        }

        public void RemoveAbility(AbilityBase ability)
        {
            abilities.Remove(ability);
        }

        public bool GetAbility<T>(AbilityTag tag, out T result) where T : AbilityBase
        {
            result = abilities.Find(x => x.Tag.HasFlag(tag)) as T;
            return result != null;
        }
    }
}