
//--------------------------------------------
// 현재 스테이지의 단계
//--------------------------------------------
public enum EStageDetail
{
    First = 0,
    Second,
    Third,
    Boss,
    Count,
}

//--------------------------------------------
// 캐릭터 액션
//--------------------------------------------
public enum eActionIndex
{
    All = 0,
    Attack1,        //!< 공격 1
    Attack2,        //!< 공격 2
    Defend,         //!< 방어
    Die,            //!< 죽음
    Hit,            //!< 피격
    Idle,           //!< 기본상태
    Run,            //!< 움직이는 상태
};


public enum EMoveType
{
    None = 0,       // 일반적인 이동
    Compulsion,     // 어떤 작용을 받아서 강제로 이동중
};

//--------------------------------------------
// 캐릭터 상태
//--------------------------------------------
public enum ECharState
{
    Alive        = 0x00000001,      //!< 살아있음
    Die          = 0x00000002,      //!< 죽음
    Battle       = 0x00000004,      //!< 전투
    MeetEnemy    = 0x00000008,      //!< 적과 만남
    DefendAttack = 0x00000010,      //!< 몬스터 공격을 방어한 상태
    Powerful     = 0x00000020,      //!< 엄청 쎈 상태
    Event        = 0x00000040,      //!< 이벤트 시간
    Remove       = 0x00000080,      //!< 제거 되고 있는 상태
    KnockBack    = 0x00000100,      //!< 넉백 상태
    AttackTerm   = 0x00000200,      //!< 공격 후 다음 공격 까지 시간

};



//--------------------------------------------
// 캐릭터 타입
//--------------------------------------------
public enum ECharType
{
    Player = 0,    //!< 플레이어
    Monster,       //!< 몬스터
};

//--------------------------------------------
// 캐릭터 HP에 처리에 대한 결과값
//--------------------------------------------
public enum ECharHPRet
{
    None = 0,
    Die,
}

//--------------------------------------------
// 태그 리스트
//--------------------------------------------
static public class CTagList
{
    public enum ETag
    {
        eBegin = 0,
        ePlayer = eBegin,

        //!< 적 리스트 시작
        eEnemyBegin,                     
        eOrc = eEnemyBegin,
        eEnemyKnight,           
        eEnemyKnightRed,
        eEnemyKnightBlue,
        eEnemyKnightGreen,
        eEnemyKnightBlack,
        eEnemyEnd = eEnemyKnightBlack,
        //!< 적 리스트 마지막

        //!< 버튼 리스트 시작
        eButtonBegin,
        eButtonPush = eButtonBegin,
        eButtonDefend,
        eButtonAttack,
        eButtonEnd = eButtonAttack,
        //!< 버튼 리스트 마지막

        eCount,
    };

    static string[] m_strTagText = 
    {
        "Player",
        "Orc",
        "EnemyKnight",
        "EnemyKnightRed",
        "EnemyKnightBlue",
        "EnemyKnightGreen",
        "EnemyKnightBlack",
        "PushButton",
        "ButtonDefend",
        "ButtonAttack",
    };

    // 인덱스로부터 Tag 값을 얻는다.
    static public string GetTagFromIndex(ETag eTag_)
    {
        return m_strTagText[(int)eTag_];
    }

    // Tag값으로 부터 인덱스 값을 얻는다.
    // 성공시 true 리턴
    // 실패시 nIndex_o 값을 0
    static public bool GetIndexFromTagText(string strTagText_, out int nIndex_o)
    {
        nIndex_o = 0;

        for (int i = (int)ETag.eBegin; i < (int)ETag.eCount; ++i)
        {
            if (m_strTagText[i] == strTagText_)
            {
                nIndex_o = i;
                return true;
            }
        }

        return false;
    }

};

//--------------------------------------------
// 캐릭터 데이터
//--------------------------------------------
static public class CCharData
{
    public enum EType
    {
        eBegin = 0,
        eExp = eBegin,      // 캐릭터 경험치
        eDamage,            // 캐릭터 추가 공격 데미지
        eHP,                // 캐릭터 추가 HP
        eCriticalRate,      // 캐릭터 치명타율
        eCriticalDamage,    // 캐릭터 치명타 데미지
        eMoney,             // 캐릭터 돈
        eEnd = eMoney,
        eCount,
    };

    static string[] m_strTypeText = 
    {
        "Exp",
        "Damage",
        "HP",
        "CriticalRate",
        "CriticalDamage",
        "Money",
    };

    // 인덱스로부터 Type 값을 얻는다.
    static public string GetTypeFromIndex(EType eType_)
    {
        return m_strTypeText[(int)eType_];
    }

    // Type값으로 부터 인덱스 값을 얻는다.
    // 성공시 true 리턴
    // 실패시 nIndex_o 값을 0
    static public bool GetIndexFromText(string strType_, out int nIndex_o)
    {
        nIndex_o = 0;

        for (int i = (int)EType.eBegin; i < (int)EType.eCount; ++i)
        {
            if (m_strTypeText[i] == strType_)
            {
                nIndex_o = i;
                return true;
            }
        }

        return false;
    }

};



