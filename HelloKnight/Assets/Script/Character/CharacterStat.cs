
public struct StCharacterStat
{
    public int m_nCharIndex;       //!< 캐릭터 인덱스
    public int m_nAttackDamage;    //!< 캐릭터 공격 데미지
    public int m_nNowHP;           //!< 캐릭터 현재 HP
    public int m_nMaxHP;           //!< 캐릭터 최대 HP
    //public int m_nExp;             //!< 캐릭터 경험치
    //public int m_nCriticalRate;    //!< 캐릭터 치명타율 (5% 확률)
    //public int m_nCriticalDamage;  //!< 캐릭터 치명타 데미지 (치명타시 (공격 데미지 * m_nCriticalDamage * 0.01f)
    //public int m_nMoney;           //!< 캐릭터 돈

    public int[] m_nExStat;        //!< 추가로 얻는 능력치 (CCharData.EType 참조)

    public int m_nLevel;           //!< 캐릭터 레벨
};