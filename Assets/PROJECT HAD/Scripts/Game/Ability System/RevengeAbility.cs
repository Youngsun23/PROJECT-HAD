using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HAD
{
    public class RevengeAbility : AbilityBase
    {
        public override AbilityTag Tag => AbilityTag.Revenge;
        private IActor attacker;
        private float originDamage;

        public override void Execute()
        {
            Owner.RevengeAttack(attacker, originDamage);
        }

        public void SetAttacker(IActor actor)
        {
            attacker = actor;
        }

        public void SetOriginDamage(float damage)
        {
            originDamage = damage;
        }
    }
}
