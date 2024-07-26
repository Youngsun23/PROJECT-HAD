using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HAD
{
    public class ReflectionAbility : AbilityBase
    {
        public override AbilityTag Tag => AbilityTag.Reflection;

        //private float reflectionAngle;
        //private float attackRadius;
        private GameObject targetProjectile;

        // 반사 스킬 발동 시 수행
        public override void Execute()
        {
            // 1. 투사체의 이동 방향을 기존의 반대 방향으로 변경
            // 2. 적의 투사체가 아닌 내 공격으로 변경
            var projCon = targetProjectile.GetComponent<ProjectileController>();
            projCon.SetLayer(LayerMask.NameToLayer(Constant.LAYER_NAME_PLAYERPROJECTILE));
            projCon.ReverseMoveDir();

            //// OverlapSphere로 360도 원형 검사
            //var colliders = Physics.OverlapSphere(Owner.transform.position, 2.5f, LayerMask.GetMask("MonProjectile"));
            //foreach(var collider in colliders)
            //{
            //    collider.transform.root.forward = -collider.transform.forward;
            //    collider.transform.root.gameObject.layer = LayerMask.NameToLayer(Constant.LAYER_NAME_PLAYERPROJECTILE);
            //}
            // <- dotAngle, 혹은 SphereCastAll로 공/콤1/콤2마다 유효 범위 다르게 계산
        }

        public void SetTargetProjectile(GameObject target)
        {
            targetProjectile = target;
        }
    }
}
