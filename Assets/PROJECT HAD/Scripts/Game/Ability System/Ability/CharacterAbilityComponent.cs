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
            newAbility.Owner = character;

            // GetUI 해서 함수 호출? Character 연결해서 abillist 검사하는 private 함수 만들어서 SwitchAbilityListUI() 내부에서 호출?
            // AbilityListUI.Instance.RegisterAbility(newAbility);
            var abilityUI = UIManager.Singleton.GetUI<AbilityListUI>(UIList.AbilityList);
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