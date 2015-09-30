public class CCharInfo
{
    public int      m_nCode              = 0;         // 캐릭터 코드
    public string   m_strName;                        // 캐릭터 이름
    public string   m_strPrefab;                      // Assets 안에 있는 Prefab 텍스트명
    public int      m_nCharType          = 0;         // 캐릭터 타입 0:플레이어 1:몬스터
    public int      m_nNowHP             = 0;         // 현재 체력
    public int      m_nMaxHP             = 0;         // 최대 체력
    public int      m_nCharWidth         = 0;         // 캐릭터 넓이
    public float    m_fMoveSpeed         = 0.0f;      // 거리 1이동 하는데 걸리는 시간
    public int      m_nAttackDamage      = 0;         // 공격 데미지
    public int      m_nAttackRange       = 0;         // 캐릭터 기본 사거리
    public float    m_fAttackSpeed       = 0.0f;      // 캐릭터 공격 속도 1: 1초에 한번
    public int      m_nExp               = 0;         // 캐릭터가 죽었을시 제공되는 경험치
    public float m_fAfterAttackIdleTermTime = 0.0f;     // 공격후 Idle 유지 시간
    public int m_nPay;    // 보상(돈)
}