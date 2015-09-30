using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class Character : MonoBehaviour {

    //------------------------------------------------------------------------
    // 캐릭터 애니메이션 스프라이트 이미지
    //------------------------------------------------------------------------
    AniIdle sAniIdle;           //!< 기본 Idle 애니
    AniAttack sAniAttack;       //!< 공격 애니
    AniRun sAniRun;             //!< 달리기
    AniDie sAniDie;             //!< 죽음
    AniDefence sAniDefence;     //!< 방어
    AniHit sAniHit;             //!< 피격

    //------------------------------------------------------------------------
    // 캐릭터 정보
    //------------------------------------------------------------------------
    StCharInfo m_kCharInfo = new StCharInfo();           //!< 캐릭터 테이블 정보
    StCharacterStat m_kCharStat = new StCharacterStat(); //!< 캐릭터 능력치
    CAction m_kAction = new CAction();                   //!< 캐릭터 액션
    CState m_kCharState = new CState();                  //!< 캐릭터 상태
    EEventFrame m_eNowEventFrame = EEventFrame.None;     //!< 현재 적용된 이벤트

    //------------------------------------------------------------------------
    // 캐릭터 위치 이동
    //------------------------------------------------------------------------
    Vector3 m_vStartPos;      // 이동 시작 지점
    Vector3 m_vEndPos;        // 이동 종료 지점
    float m_fStartMoveTime;   // 이동 시작 시간
    float m_fEndMoveTime;     // 이동 종료 시간

    //------------------------------------------------------------------------
    // 캐릭터 타겟
    //------------------------------------------------------------------------
    int m_nTarCharIndex = 0;    //!< 대상 캐릭터 인덱스

    //------------------------------------------------------------------------
    // 캐릭터 화면에 출력할 텍스트(디버깅)
    //------------------------------------------------------------------------
    public GameObject kTextCharState;
    public GameObject kTextCharAction;
    public GameObject kTextCharInfo;

    void Awake()
    {
        sAniIdle = (AniIdle)gameObject.GetComponent("AniIdle");
        sAniAttack = (AniAttack)gameObject.GetComponent("AniAttack");
        sAniRun = (AniRun)gameObject.GetComponent("AniRun");
        sAniDie = (AniDie)gameObject.GetComponent("AniDie");
        sAniDefence = (AniDefence)gameObject.GetComponent("AniDefence");
        sAniHit = (AniHit)gameObject.GetComponent("AniHit");

        m_kCharStat.m_nExStat = new int[(int)CCharData.EType.eCount];
    }

    // 추가로 얻는 능력치 세팅
    public void SetExStat(CCharData.EType eType_, int nValue_)
    {
        m_kCharStat.m_nExStat[(int)eType_] = nValue_;
    }
    public bool GetExStat(CCharData.EType eType_, out int nValue_o)
    {
        nValue_o = m_kCharStat.m_nExStat[(int)eType_];
        return true;
    }

	// Use this for initialization
	void Start () {

        Initilize();
	}

    public void Initilize()
    {
        m_kCharState.Realese();
        ActionIdle();
        m_nTarCharIndex = 0;
        setNowHP(GetMaxHP());
    }
   
	// Update is called once per frame
	void Update () {

        updateCharState();
        UpdateAction();
        UpdateScale();
        UpdateRotation();
        UpdatePosition();
               

        sAniIdle.UpdateAni();
        sAniHit.UpdateAni();
        sAniAttack.UpdateAni();
        sAniDefence.UpdateAni();
        
        sAniRun.UpdateAni();
        sAniDie.UpdateAni();
	}
    void updateCharState()
    {
        if (m_kCharState.IsCharState(ECharState.DefendAttack))
        {
            if (m_kCharState.IsTimeOver(ECharState.DefendAttack))
            {
                m_kCharState.SubCharState(ECharState.DefendAttack);
            }
        }

        if (m_kCharState.IsCharState(ECharState.Powerful))
        {
            if (m_kCharState.IsTimeOver(ECharState.Powerful))
            {
                m_kCharState.SubCharState(ECharState.Powerful);
            }
        }

        if (m_kCharState.IsCharState(ECharState.Battle))
        {
            if (m_kCharState.IsTimeOver(ECharState.Battle))
            {
                m_kCharState.SubCharState(ECharState.Battle);
            }
        }

        if (m_kCharState.IsCharState(ECharState.KnockBack))
        {
            if (m_kCharState.IsTimeOver(ECharState.KnockBack))
            {
                m_kCharState.SubCharState(ECharState.KnockBack);
            }
        }
        if (m_kCharState.IsCharState(ECharState.AttackTerm))
        {
            if (m_kCharState.IsTimeOver(ECharState.AttackTerm))
            {
                m_kCharState.SubCharState(ECharState.AttackTerm);
            }
        }
        
        // 정보 표시
        if (ECharType.Player == m_kCharInfo.m_eCharType)
        {
            Text text = (Text)kTextCharState.GetComponent("Text");
            text.text = m_kCharState.GetTextState();

            string strText = "Damage:";
            strText += Convert.ToString(m_kCharStat.m_nAttackDamage);

            strText += "NowHP:";
            strText += Convert.ToString(m_kCharStat.m_nNowHP);

            strText += " MaxHP:";
            strText += Convert.ToString(m_kCharStat.m_nMaxHP);

            strText += " Exp:";
            strText += Convert.ToString(m_kCharStat.m_nExStat[(int)CCharData.EType.eExp]);

            strText += " CriticalRate:";
            strText += Convert.ToString(m_kCharStat.m_nExStat[(int)CCharData.EType.eCriticalRate]);

            strText += " CriticalDamage:";
            strText += Convert.ToString(m_kCharStat.m_nExStat[(int)CCharData.EType.eCriticalDamage]);

            strText += " Money:";
            strText += Convert.ToString(m_kCharStat.m_nExStat[(int)CCharData.EType.eMoney]);

            text = (Text)kTextCharInfo.GetComponent("Text");
            text.text = strText;

        }
        
    }
    void UpdateAction()
    {
        switch (m_kAction.nActionState)
        {
            case eActionIndex.Die:
                {
                    
                }
                break;
            case eActionIndex.Hit:
                {
                    if (Time.fixedTime - m_kAction.fActionStartTime >= m_kAction.fActionContinueTime)
                        ActionIdle();
                }
                break;
            case eActionIndex.Attack1:
            case eActionIndex.Attack2:
                {
                    if (Time.fixedTime - m_kAction.fActionStartTime >= m_kAction.fActionContinueTime)
                    {
                        m_kCharState.AddCharState(ECharState.AttackTerm);
                        ActionIdle();
                    }
                }
                break;
            case eActionIndex.Defend:
                {
                    if (Time.fixedTime - m_kAction.fActionStartTime >= m_kAction.fActionContinueTime)
                        ActionIdle();
                }
                break;
            
        };

        // 정보 표시
        if (ECharType.Player == m_kCharInfo.m_eCharType)
        {
            Text text = (Text)kTextCharAction.GetComponent("Text");
            text.text = m_kAction.GetTextAction();
        }
    }
    void UpdateScale()
    {
    }
    void UpdateRotation()
    {
    }
    //----------------------------------------------------------------
    // 이동
    //----------------------------------------------------------------
    public void MoveStart(Vector3 vEndPos_)
    {
        // 위치
        m_vStartPos = gameObject.transform.position;
        m_vEndPos = vEndPos_;

        // 시간
        m_fStartMoveTime = Time.fixedTime;
        if (0.0f < m_kCharInfo.m_fMoveSpeed)
        {
            float fDist = Vector3.Distance(m_vStartPos, m_vEndPos);
            m_fEndMoveTime = m_fStartMoveTime + fDist / m_kCharInfo.m_fMoveSpeed;
        }
        else
        {
            string str = "MoveSpeed is Zero CharCode : " + Convert.ToString(m_kCharInfo.m_nCode);
            Debug.Log(str);
        }

        ActionRun();
    }
    void moveKnockBack()
    {
        m_vStartPos = gameObject.transform.position;
        m_vEndPos = gameObject.transform.position;
        // 시간
        m_fStartMoveTime = Time.fixedTime;
        
        switch(m_kCharInfo.m_eCharType)
        {
            case ECharType.Monster:
                m_vEndPos.x += 1.0f;
                break;
            case ECharType.Player:
                m_vEndPos.x -= 1.0f;
                break;
            default:
                Debug.Log(string.Format("Not Be CharType:{0}", m_kCharInfo.m_eCharType));
                break;
        };
        
    }
    public void MoveStop()
    {
        m_vStartPos = gameObject.transform.position;
        m_vEndPos = gameObject.transform.position;

        ActionIdle();
    }
    public void SetPosition(Vector3 vPos_)
    {
        gameObject.transform.position = vPos_;
    }
    public Vector3 GetPosition()
    {
        return gameObject.transform.position;
    }
    // 이동
    void UpdatePosition()
    {
        if (m_kCharState.IsCharState(ECharState.KnockBack))
            updatePositionKnockBack();
        else
            updatePositionNormal();
    }
    // 일반 이동
    void updatePositionNormal()
    {
        if (eActionIndex.Run == m_kAction.nActionState && m_vStartPos != m_vEndPos && m_fEndMoveTime - m_fStartMoveTime != 0)
        {
            float fTime = (Time.fixedTime - m_fStartMoveTime) / (m_fEndMoveTime - m_fStartMoveTime);

            Vector3 vNowPos = Vector3.Lerp(m_vStartPos, m_vEndPos, fTime);
            gameObject.transform.position = vNowPos;

            if (vNowPos == m_vEndPos)
                MoveStop();
        }
    }
    // 넉백시 이동
    void updatePositionKnockBack()
    {
        float fTime = (Time.fixedTime - m_fStartMoveTime)  / 0.5f  ;
        if (1.0f < fTime)
            fTime = 1.0f;

        Vector3 vNowPos = Vector3.Lerp(m_vStartPos, m_vEndPos, fTime);
        if (fTime <= 0.5f)
        {
            
        }
        else
        {
            
        }

        gameObject.transform.position = vNowPos;
        if (vNowPos == m_vEndPos)
            MoveStop();
    }
    //----------------------------------------------------------------
    // 액션
    //----------------------------------------------------------------
    public void AniStop()
    {
        sAniIdle.AniStop();
        sAniAttack.AniStop();
        sAniDie.AniStop();
        sAniRun.AniStop();
        m_eNowEventFrame = EEventFrame.None;
    }

    public void ActionIdle()
    {
        if (IsDie())
            return;

        AniStop();
        sAniIdle.AniPlay();
        //sAniRun.AniPlay();
        m_kAction.nActionState = eActionIndex.Idle;
        m_kAction.fActionStartTime = Time.fixedTime;

        //string str = "ActionIdle" + Convert.ToString(Time.fixedTime);
        //Debug.Log(str);
        
    }
    // 공격
    public void ActionAttack()
    {
        if (IsDie())
            return;

        AniStop();

        if (m_kCharState.IsCharState(ECharState.DefendAttack))
        {
            //sAniAttack.AniPlay();
            m_kAction.nActionState = eActionIndex.Attack2;
            m_kAction.fActionStartTime = Time.fixedTime;
            m_kAction.fActionContinueTime = m_kCharInfo.m_fAttackSpeed;
        }
        else
        {
            m_kAction.nActionState = eActionIndex.Attack1;
            m_kAction.fActionStartTime = Time.fixedTime;
            m_kAction.fActionContinueTime = m_kCharInfo.m_fAttackSpeed;
        }
        sAniAttack.AniFrameTime(m_kCharInfo.m_fAttackSpeed);
        sAniAttack.AniPlay();
        //Debug.Log(Convert.ToString( m_kCharInfo.m_fAttackSpeed) );

    }
    public void ActionRun()
    {
        if (IsDie())
            return;

        AniStop();
        sAniRun.AniPlay();
        
        m_kAction.nActionState = eActionIndex.Run;
        m_kAction.fActionStartTime = Time.fixedTime;

        //string str = "ActionRun" + Convert.ToString(Time.fixedTime);
        //Debug.Log(str);
    }

    public void ActionHit(bool bDefendAttack_)
    {
        if (IsDie())
            return;

        //AniStop();
        //sAniHit.AniPlay();

        //m_kAction.nActionState = eActionIndex.Hit;
        //m_kAction.fActionStartTime = Time.fixedTime;
        //m_kAction.fActionContinueTime = 0.1f;

        if (ECharType.Monster == m_kCharInfo.m_eCharType)
        {
            // 30%
            int nRan = UnityEngine.Random.Range(0, 10);
            if (bDefendAttack_ || nRan < 1)
            {
                m_kAction.nActionState = eActionIndex.Hit;
                m_kAction.fActionStartTime = Time.fixedTime;
                m_kAction.fActionContinueTime = 0.1f;

                ClearMeetEnemy();
                ClearTarCharIndex();
                m_kCharState.AddCharState(ECharState.KnockBack);
                moveKnockBack();
            }
        }
        //string str = "ActionHit" + Convert.ToString(Time.fixedTime);
        //Debug.Log(str);

    }
    public void ActionDie()
    {
        AniStop();
        sAniDie.AniPlay();
        m_kAction.nActionState = eActionIndex.Die;
        m_kAction.fActionStartTime = Time.fixedTime;
        m_kAction.fActionContinueTime = 1.2f;

        m_kCharState.AddCharState(ECharState.Die);
        m_kCharState.SubCharState(ECharState.Alive);
    }
    
    public void ActionDefend()
    {
        if (IsDie())
            return;

        AniStop();
        sAniDefence.AniPlay();

        m_kAction.nActionState = eActionIndex.Defend;
        m_kAction.fActionStartTime = Time.fixedTime;
        m_kAction.fActionContinueTime = 0.5f;
    }

    public string GetTextAction()
    {
        return m_kAction.GetTextAction();
    }
    //----------------------------------------------------------------
    // 캐릭터 현재 이벤트
    //----------------------------------------------------------------
    public EEventFrame GetNowEventFrame()
    {
        return m_eNowEventFrame;
    }
    public void NowEventFrameReset()
    {
        m_eNowEventFrame = EEventFrame.None;
    }
    void DamageEventFrame()
    {
        m_eNowEventFrame = EEventFrame.Damage;
    }
    void HitEventFrame()
    {
        m_eNowEventFrame = EEventFrame.Hit;
    }
    //----------------------------------------------------------------
    // 상태
    //----------------------------------------------------------------
    // 공격 가능 상태 체크
    public bool IsReadyActionAttack()
    {
        if ( (eActionIndex.Idle == m_kAction.nActionState || eActionIndex.Defend == m_kAction.nActionState || eActionIndex.Run == m_kAction.nActionState )
            && !IsKnockBack()
            )
            return true;
        return false;
    }
    public bool IsAttackTerm()
    {
        if (m_kCharState.IsCharState(ECharState.AttackTerm))
            return true;
        return false;
    }
    // 넉백 시간인가?
    public bool IsKnockBack()
    {
        if (m_kCharState.IsCharState(ECharState.KnockBack))
            return true;
        
        return false;
    }
    // 공격 상태인가?
    public bool IsActionAttack()
    {
        if (eActionIndex.Attack1 == m_kAction.nActionState || eActionIndex.Attack2 == m_kAction.nActionState)
            return true;
        return false;
    }
    
    

    public bool IsIdle()
    {
        if (eActionIndex.Idle == m_kAction.nActionState)
            return true;
        return false;
    }
    public bool IsDefend()
    {
        if (eActionIndex.Defend == m_kAction.nActionState)
            return true;
        return false;
    }
    public float GetNowActionStartTime()
    {
        return m_kAction.fActionStartTime;
    }
    // 상대방으로 부터 공격 당함    
    public void Hit(int nDamage_, bool bDefendAttack_)
    {
        switch (m_kAction.nActionState)
        {
            case eActionIndex.Defend:
                m_kCharState.AddCharState(ECharState.DefendAttack);
                break;
            case eActionIndex.Die:
                break;
            default:
                if (ECharHPRet.Die == subHP(nDamage_))
                    ActionDie();
                else
                    ActionHit(bDefendAttack_);
                break;
        };
    }
    //----------------------------------------------------------------
    // 캐릭터 상태
    //----------------------------------------------------------------
    // 전투 상태 체크
    public bool IsBattleState()
    {
        return m_kCharState.IsCharState(ECharState.Battle);
    }
    // 적과 만났는가?
    public void MeetEnemy()
    {
        m_kCharState.AddCharState(ECharState.Battle | ECharState.MeetEnemy);
    }
    public bool IsMeetEnemy()
    {
        return m_kCharState.IsCharState(ECharState.MeetEnemy);
    }
    public void ClearMeetEnemy()
    {
        m_kCharState.SubCharState(ECharState.MeetEnemy);
    }
    public bool IsDie()
    {
        if (m_kCharState.IsCharState(ECharState.Die))
            return true;
        return false;
    }
    public bool IsRemoveState()
    {
        if (m_kCharState.IsCharState(ECharState.Remove))
            return true;
        return false;
    }
    // 제거 되는 상태로 들어감
    public void RemoveState()
    {
        m_kCharState.AddCharState(ECharState.Remove);
    }
    public bool IsDefendAttack()
    {
        return m_kCharState.IsCharState(ECharState.DefendAttack);
    }
    public string GetTextState()
    {
        return m_kCharState.GetTextState();
    }
    //----------------------------------------------------------------
    // 캐릭터 능력치
    //----------------------------------------------------------------
    public void SetCharInfo(StCharInfo kCharInfo_)
    {
        m_kCharInfo = kCharInfo_;

        m_kCharStat.m_nAttackDamage = m_kCharInfo.m_nAttackDamage;
        m_kCharStat.m_nNowHP = m_kCharInfo.m_nNowHP;
        m_kCharStat.m_nMaxHP = m_kCharInfo.m_nMaxHP;
    }
    public void SetCharIndex(int nCharIndex_)
    {
        m_kCharStat.m_nCharIndex = nCharIndex_;
    }
    public int GetCharIndex()
    {
        return m_kCharStat.m_nCharIndex;
    }
    // 공격 사거리
    public float GetAttackRange()
    {
        return m_kCharInfo.m_fAttackRange;
    }
    // 캐릭터 지름
    public float GetCharWidth()
    {
        return m_kCharInfo.m_fCharWidth;
    }
    // 캐릭터 데미지
    public int GetDamage()
    {
        return m_kCharStat.m_nAttackDamage;
    }
    void setDamage(int nDamage_)
    {
        m_kCharStat.m_nAttackDamage = nDamage_;
    }
    

    // 캐릭터 HP
    // HP 회복
    void addHP(int nHP_)
    {
        m_kCharStat.m_nNowHP += nHP_;
        if (m_kCharStat.m_nMaxHP < m_kCharStat.m_nNowHP)
        {
            m_kCharStat.m_nNowHP = m_kCharStat.m_nMaxHP;
        }
    }
    // HP 감소
    ECharHPRet subHP(int nHP_)
    {
        ECharHPRet eCharHPRet = ECharHPRet.None;
        m_kCharStat.m_nNowHP -= nHP_;
        if (m_kCharStat.m_nNowHP <= 0)
        {
            m_kCharStat.m_nNowHP = 0;
            eCharHPRet = ECharHPRet.Die;
        }
        return eCharHPRet;
    }
    void setNowHP(int nHP_)
    {
        m_kCharStat.m_nNowHP = nHP_;
    }
    public int GetNowHP()
    {
        return m_kCharStat.m_nNowHP;
    }
    public int GetMaxHP()
    {
        return m_kCharStat.m_nMaxHP;
    }
    public void SetExMaxHP(int nHP_)
    {
        m_kCharStat.m_nExStat[(int)CCharData.EType.eHP] = nHP_;

        m_kCharStat.m_nNowHP += nHP_;
        m_kCharStat.m_nMaxHP += nHP_;
    }
    void levelUpHPSet()
    {
        m_kCharStat.m_nNowHP = m_kCharInfo.m_nNowHP + m_kCharStat.m_nExStat[(int)CCharData.EType.eHP] + (m_kCharStat.m_nLevel - 1) * 2;
        m_kCharStat.m_nMaxHP = m_kCharInfo.m_nMaxHP + m_kCharStat.m_nExStat[(int)CCharData.EType.eHP] + (m_kCharStat.m_nLevel - 1) * 2;
    }

    // 몬스터가 주는 경험치 얻기
    public int GetMonsterExp()
    {
        return m_kCharInfo.m_nExp;
    }
    ///--------------------------------------
    // 플레이어 경험치 세팅
    ///--------------------------------------
    // 기존 경험치에 추가
    public void AddExp(int nExp_)
    {
        m_kCharStat.m_nExStat[(int)CCharData.EType.eExp] += nExp_;

        int nTempLevel = m_kCharStat.m_nExStat[(int)CCharData.EType.eExp] / 10;
        if (m_kCharStat.m_nLevel < nTempLevel)
        {
            m_kCharStat.m_nLevel = nTempLevel;

            setDamage(m_kCharInfo.m_nAttackDamage + m_kCharStat.m_nLevel - 1);
            levelUpHPSet();
        }
    }
    // 새로 경험치 세팅
    public void SetExp(int nExp_)
    {
        m_kCharStat.m_nExStat[(int)CCharData.EType.eExp] = nExp_;

        m_kCharStat.m_nLevel = m_kCharStat.m_nExStat[(int)CCharData.EType.eExp] / 10;
        if (0 == m_kCharStat.m_nLevel)
            m_kCharStat.m_nLevel = 1;

        setDamage(m_kCharInfo.m_nAttackDamage + m_kCharStat.m_nLevel - 1);
        levelUpHPSet();
    }
    // 현재 경험치 얻기
    public int GetExp()
    {
        return m_kCharStat.m_nExStat[(int)CCharData.EType.eExp];
    }
    // 치명타률 세팅
    public void SetCriticalRate(int nCriticalRate_)
    {
        m_kCharStat.m_nExStat[(int)CCharData.EType.eCriticalRate] = nCriticalRate_;
    }
    // 치명타 데미지
    public void SetCriticalDamage(int nCriticalDamage_)
    {
        m_kCharStat.m_nExStat[(int)CCharData.EType.eCriticalDamage] = nCriticalDamage_;
    }
    // 몬스터 보상(돈)
    public int GetMonsterPay()
    {
        return m_kCharInfo.m_nPay;
    }
    // 플레이어 돈 추가
    public void AddMoney(int nMoney_)
    {
        m_kCharStat.m_nExStat[(int)CCharData.EType.eMoney] += nMoney_;
    }
    // 플레이어 돈 얻기
    public int GetMoney()
    {
        return m_kCharStat.m_nExStat[(int)CCharData.EType.eMoney];
    }
    // 플레이어 돈 세팅
    public void SetMoney(int nMoney_)
    {
        m_kCharStat.m_nExStat[(int)CCharData.EType.eMoney] = nMoney_;
    }

    //------------------------------------------------------------------------
    // 캐릭터 타겟
    //------------------------------------------------------------------------
    // 타겟의 캐릭터 인덱스 세팅
    public void SetTarCharIndex(int nIndex_)
    {
        m_nTarCharIndex = nIndex_;
    }
    // 타겟의 캐릭터 인덱스 얻기
    public int GetTarCharIndex()
    {
        return m_nTarCharIndex;
    }
    // 타겟 캐릭터 인덱스 없앰
    public void ClearTarCharIndex()
    {
        m_nTarCharIndex = 0;
    }

    void OnTriggerEnter(Collider other)
    {
        
        //// 적과 만났을시 이벤트 발생
        //bool bRightMeet = false;
        //// 몬스터 일시 플레이어 체크
        //if (ECharType.Monster == m_kCharInfo.m_eCharType && other.gameObject.tag == CTagList.GetTagFromIndex(CTagList.ETag.ePlayer) )
        //    bRightMeet = true;
        //// 플레이어 일시 몬스터 체크
        //else if(ECharType.Player == m_kCharInfo.m_eCharType)
        //{
        //    // 태그 리스트를 돌면서 체크
        //    for (int i = (int)CTagList.ETag.eEnemyBegin; i < (int)CTagList.ETag.eEnemyEnd; ++i)
        //    {
        //        if (    other.gameObject.tag == CTagList.GetTagFromIndex((CTagList.ETag)i) )
        //        {
        //            bRightMeet = true;
        //            break;
        //        }
        //    }
        //}
         
        //if (bRightMeet)
        //{
        //    MoveStop();
        //    m_kCharState.AddCharState(ECharState.MeetEnemy | ECharState.Battle);
        //}
         
    }
}
