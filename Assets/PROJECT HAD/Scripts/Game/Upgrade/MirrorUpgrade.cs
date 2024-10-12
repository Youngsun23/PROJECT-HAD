using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HAD
{
    public enum MirrorUpgradeNames
    {
        Shawdow,
        Vitality,
        DeathResistance,
        Reflexes,

        EndField,
    }

    public class MirrorUpgrade
    {
        // 업그레이드 클래스(이름, 단계별 효과|무슨 스탯을 얼마 상승)
        // 강화단계-상승치 딕셔너리로 묶어두고 효과 적용 함수에서 강화단계 받아와 상승치만큼을 userdata에 mirrorbuffed~로 저장해주기?
        // 강화단계는 얘의 필드로 두고, 효과 적용 함수에서 변화값만 userdata/attribute 쪽에 넘겨주고 싶은데
        // 강화단계는 UpgradeDataDTO로 따로 두고 mirror 쪽 단계 세팅에 써주고
        private string name;
        private int upgradeStep;
        private Dictionary<int, float> stepTable = new Dictionary<int, float>();

        // 업그레이드들 목록으로 보유, 

        // 업그레이드 실행 함수(-재화+강화단계 / userdata 업데이트)
    }
}
