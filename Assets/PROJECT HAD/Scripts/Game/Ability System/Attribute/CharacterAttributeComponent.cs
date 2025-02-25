using System.Collections.Generic;
using UnityEngine;

namespace HAD
{
    public enum AttributeTypes 
    {
        HealthPoint,
        MagicArrowCount,
        AttackDamage,
        MagicDamage,
        SpecialAttackDamage,
        DashCooltime,
        MoveSpeed,

        EndField,
    }

    public class CharacterAttributeComponent : MonoBehaviour
    {
        public Dictionary<AttributeTypes, CharacterAttribute> attributes = new Dictionary<AttributeTypes, CharacterAttribute>();

        public void RegisterEvent(AttributeTypes type, System.Action<float, float> onChanedEvent = null, System.Action<float> onChangedBuffed = null)
        {
            attributes[type].OnChangedEvent += onChanedEvent;
            attributes[type].OnChangedBuffed += onChangedBuffed;
        }

        private void Awake()
        {
            for (int i = 0; i < (int)AttributeTypes.EndField; i++)
            {
                attributes.Add((AttributeTypes)i, new CharacterAttribute());
            }
        }

        public CharacterAttribute GetAttribute(AttributeTypes type)
        {
            return attributes[type];
        }

        public void SetAttribute(AttributeTypes type, float defaultValue, float buffedValue = 0)
        {
            attributes[type].CurrentValue = defaultValue;
            attributes[type].DefaultValue = defaultValue;
            attributes[type].BuffedValue = buffedValue;
        }

        public void SetAttributeCurrentValue(AttributeTypes type, float currentValue)
        {
            attributes[type].CurrentValue = currentValue;

            attributes[type].OnChangedEvent?.Invoke(attributes[type].DefaultValue + attributes[type].BuffedValue, attributes[type].CurrentValue);
        }

        public void SetAttributeDefaultValue(AttributeTypes type, float defaultValue)
        {
            attributes[type].DefaultValue = defaultValue;
        }

        public void SetAttributeBuffedValue(AttributeTypes type, float buffedValue)
        {
            attributes[type].BuffedValue = buffedValue;

            attributes[type].OnChangedBuffed?.Invoke(attributes[type].BuffedValue);
        }
    }
}
