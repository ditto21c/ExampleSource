public class CItemInfo
{
    public int m_nIndex = 0;               // 아이템 인덱스
    public string m_strName;               // 아이템 이름
    public int m_nMainType = 0;            // 아이템 기본 타입
    public int m_nDetailType = 0;          // 아이템 상세 타입
    public float m_fDamage = 0.0f;         // 아이템 데미지
    public float m_fRange = 0.0f;          // 아이템 사거리
    public float m_fAttackSpeed = 0.0f;    // 아이템 공격 속도
    public float m_fAngle = 0.0f;          // 아이템 각도
    public float m_fCharMoveSpeed = 0.0f;  // 캐릭터 이동 속도 변화
    public int m_nWeaponSkillIndex = 0;    // 무기에 따른 스킬 인덱스
    public int m_nUseableLevel = 0;        // 무기 사용 레벨
    public int m_nPrice = 0;               // 무기 가격
    public string m_strModel;              // 무기 모델
    public string m_strSound;              // 무기 사용시 사운드
    public string m_strIcon;               // 아이콘 이름
    public int m_nFuntion;                 // 아이템 기능
    public int m_nHP;                      // HP 증가
    public int m_nMP;                      // MP 증가
}