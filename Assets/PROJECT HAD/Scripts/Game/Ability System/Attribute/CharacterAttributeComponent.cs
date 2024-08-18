using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HAD
{
    public enum AttributeTypes
    {
        // 예시
        HealthPoint,
        MagicArrowCount,
        AttackPower,
        MagicPower,
        SpecialAttackPower,
        DashCooltime,
        MoveSpeed,

        EndField,
    }

    public class CharacterAttributeComponent : MonoBehaviour
    {
        public Dictionary<AttributeTypes, CharacterAttribute> attributes = new Dictionary<AttributeTypes, CharacterAttribute>();

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
        }

        public void SetAttributeDefaultValue(AttributeTypes type, float defaultValue)
        {
            attributes[type].DefaultValue = defaultValue;
        }

        public void SetAttributeBuffedValue(AttributeTypes type, float buffedValue)
        {
            attributes[type].BuffedValue = buffedValue;
        }

        // CurrentValue Increase/Decrease 함수를 만드는 게 나을 듯
        // InCrease, Decrease 함수를 두 개 만드는 것과, Change 함수 하나에 인수 +-로 구분하는 것 중 무엇이 낫지?
        public void IncreaseAttributeCurrentValue(AttributeTypes type, float increaseAmount)
        {
            attributes[type].CurrentValue += increaseAmount;
        }
        public void DecreaseAttributeCurrentValue(AttributeTypes type, float decreaseAmount)
        {
            attributes[type].CurrentValue -= decreaseAmount;
        }
        
    }
}
