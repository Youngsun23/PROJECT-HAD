using UnityEngine;

namespace HAD
{
    public class ReflectionAbility : AbilityBase
    {
        public override AbilityTag Tag => AbilityTag.Reflection;
        public override string Name => "Reflection";
        private GameObject targetProjectile;


        public override void Execute()
        {
            var projCon = targetProjectile.GetComponent<ProjectileController>();
            projCon.ReverseMoveDir();
        }

        public void SetTargetProjectile(GameObject target)
        {
            targetProjectile = target;
        }
    }
}
