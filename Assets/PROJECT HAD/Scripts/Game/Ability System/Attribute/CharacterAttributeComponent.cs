using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HAD
{
    public enum AttributeTypes // 재화 관리는 따로 해야겠지?
    {
        // 예시
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

            // 이벤트 추가
            attributes[type].OnChangedEvent?.Invoke(attributes[type].DefaultValue + attributes[type].BuffedValue, attributes[type].CurrentValue);
        }

        public void SetAttributeDefaultValue(AttributeTypes type, float defaultValue)
        {
            attributes[type].DefaultValue = defaultValue;
        }

        public void SetAttributeBuffedValue(AttributeTypes type, float buffedValue) // 
        {
            attributes[type].BuffedValue = buffedValue;

            attributes[type].OnChangedBuffed?.Invoke(attributes[type].BuffedValue);
        }

        // 이 클래스는 데이터 저장소로만 작동하고, 변경에는 사용 X -> 주석화?
        // currentHP만 buff 개념이 아니라서 +- 따로 두고 싶은데...
        // 아니면 얘만 PlayerCharacter의 필드로 두고 거기서 GetSet하고, UserData에 저장만 해주면 안되나?

        //public void IncreaseCurrentHP(float value)
        //{
        //    attributes[AttributeTypes.HealthPoint].CurrentValue += value;
        //}
        //public void DecreaseCurrentHP(float value)
        //{
        //    attributes[AttributeTypes.HealthPoint].CurrentValue -= value;
        //}

        // 으아아아 GameData-~Component-UserData
        // Default-Buffed-Current-Max 죄다 헷갈려
        // 여기에 증감 함수 두고
        // 유저 데이터 쪽에는 Max 수치는 뭐 강화할 때 뭐 강화했음 정보만 저장하고(Max 수치 자체가 아니라)
        // Current 수치는 게임 저장할 때(방 이동 시 이전 방에서의 모든 상태 저장함)만 유저 데이터에 갱신해주는 방식은 안되나
    }
}
