using UnityEngine;

namespace HAD
{
    public class PushAbility : AbilityBase
    {
        public override AbilityTag Tag => AbilityTag.Push | AbilityTag.Attack;
        public override string Name => "Push";
        private GameObject targetMon;


        public override void Execute()
        {
            Vector3 pushDir = (Owner.transform.position - targetMon.transform.position).normalized;
            targetMon.transform.Translate(pushDir);
        }

        public void SetTargetMon(GameObject target)
        {
            targetMon = target;
        }
    }
}
