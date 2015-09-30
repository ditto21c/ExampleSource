using UnityEngine;
using System;
using System.Collections;

using TdStateTime = System.Collections.Generic.Dictionary<ECharState, StStateTime>;

public struct StStateTime
{
    public ECharState m_eCharState;    //!< 현재 캐릭터 상태
    public float m_fStartTime;     //!< 시작 시간
    public float m_fContinueTime;  //!< 유지 시간
};

public class CState
{
    ECharState m_eCharState;           // 현재 캐릭터 상태
    //float fDefendAttackStartTime;    // 몬스터 공격을 방어한 상태 시작 시간
    //float fDefendAttackContinueTime; // 몬스터 공격을 방어한 상태 유지 시간

    //float fPowerfulStartTime;        // 엄청 쎈 상태 시작 시간
    //float fPowerfulContinueTime;     // 엄청 쎈 상태 유지 시간

    TdStateTime m_tmStateTime = new TdStateTime();

    public CState() { }
    ~CState() { }

    public void Realese()
    {
        m_eCharState = ECharState.Alive;
        //fDefendAttackStartTime = 0;
        //fDefendAttackContinueTime = 0;
        //fPowerfulStartTime = 0;
        //fPowerfulContinueTime = 0;

        m_tmStateTime.Clear();
    }

    public void AddCharState(ECharState eCharState_)
    {
        if (0 < (eCharState_ & ECharState.DefendAttack))
        {
            StStateTime stStateTime;
            stStateTime.m_eCharState = ECharState.DefendAttack;
            stStateTime.m_fStartTime = Time.fixedTime;
            stStateTime.m_fContinueTime = 1.0f;
            addStateTime(stStateTime);
            
        }

        if (0 < (eCharState_ & ECharState.Powerful))
        {
            StStateTime stStateTime;
            stStateTime.m_eCharState = ECharState.Powerful;
            stStateTime.m_fStartTime = Time.fixedTime;
            stStateTime.m_fContinueTime = 5.0f;
            addStateTime(stStateTime);
        }

        if (0 < (eCharState_ & ECharState.Battle))
        {
            StStateTime stStateTime;
            stStateTime.m_eCharState = ECharState.Battle;
            stStateTime.m_fStartTime = Time.fixedTime;
            stStateTime.m_fContinueTime = 5.0f;
            addStateTime(stStateTime);
        }

        if (0 < (eCharState_ & ECharState.KnockBack))
        {
            StStateTime stStateTime;
            stStateTime.m_eCharState = ECharState.KnockBack;
            stStateTime.m_fStartTime = Time.fixedTime;
            stStateTime.m_fContinueTime = 0.5f;
            addStateTime(stStateTime);
        }

        if (0 < (eCharState_ & ECharState.AttackTerm))
        {
            StStateTime stStateTime;
            stStateTime.m_eCharState = ECharState.AttackTerm;
            stStateTime.m_fStartTime = Time.fixedTime;
            stStateTime.m_fContinueTime = 0.5f;
            addStateTime(stStateTime);
        }

        if (0 == (eCharState_ & (ECharState.Die | ECharState.Battle | ECharState.MeetEnemy | ECharState.DefendAttack | ECharState.Powerful | ECharState.KnockBack | ECharState.AttackTerm))
            && (0 < (eCharState_ & ECharState.Event))    )
        {
            StStateTime stStateTime;
            stStateTime.m_eCharState = ECharState.Event;
            stStateTime.m_fStartTime = Time.fixedTime;
            stStateTime.m_fContinueTime = float.MaxValue;
            addStateTime(stStateTime);
        }

        m_eCharState = (m_eCharState | eCharState_);
    }
    //!< 상태 시간 넣기
    void addStateTime(StStateTime stStateTime_)
    {
        StStateTime stStateTime;
        if (m_tmStateTime.TryGetValue(stStateTime_.m_eCharState, out stStateTime))
        {
            stStateTime.m_fStartTime = stStateTime_.m_fStartTime;
            stStateTime.m_fContinueTime = stStateTime_.m_fContinueTime;
        }
        else
        {
            m_tmStateTime.Add(stStateTime_.m_eCharState, stStateTime_);
        }
    }
    //!< 상태 시간 빼기
    void eraseStateTime(ECharState eCharState_)
    {
        m_tmStateTime.Remove(eCharState_);
    }
    //!< 상태 시간 얻기
    bool GetStartStateTime(ECharState eCharState_, out float fTime_o)
    {
        StStateTime stStateTime;
        if (m_tmStateTime.TryGetValue(eCharState_, out stStateTime))
        {
            fTime_o = stStateTime.m_fStartTime;
            return true;
        }
        fTime_o = 0;
        return false;
    }
    bool GetContinueStateTime(ECharState eCharState_, out float fTime_o)
    {
        StStateTime stStateTime;
        if (m_tmStateTime.TryGetValue(eCharState_, out stStateTime))
        {
            fTime_o = stStateTime.m_fContinueTime;
            return true;
        }
        fTime_o = 0;
        return false;
    }
    
    public void SubCharState(ECharState eCharState_)
    {
        if (0 < (eCharState_ & ECharState.DefendAttack))
        {
            eraseStateTime(ECharState.DefendAttack);
        }

        if (0 < (eCharState_ & ECharState.Powerful))
        {
            eraseStateTime(ECharState.Powerful);
        }

        if (0 < (eCharState_ & ECharState.Battle))
        {
            eraseStateTime(ECharState.Battle);
        }

        if (0 < (eCharState_ & ECharState.KnockBack))
        {
            eraseStateTime(ECharState.KnockBack);
        }

        if (0 < (eCharState_ & ECharState.AttackTerm))
        {
            eraseStateTime(ECharState.AttackTerm);
        }

        m_eCharState = m_eCharState ^ (m_eCharState & eCharState_);
    }

    public bool IsCharState(ECharState eCharState_)
    {
        if (0 < (m_eCharState & eCharState_))
            return true;
        return false;
    }

    public float GetStartTime(ECharState eCharState_)
    {
        float fStartTime = 0.0f;
        if (0 < (eCharState_ & ECharState.DefendAttack))
        {
            //fStartTime = fDefendAttackStartTime;
            GetStartStateTime(ECharState.DefendAttack, out fStartTime);
        }
        else if (0 < (eCharState_ & ECharState.Powerful))
        {
            //fStartTime = fPowerfulStartTime = 0;
            eraseStateTime(ECharState.Powerful);
        }
        else if (0 < (eCharState_ & ECharState.Battle))
        {
            GetStartStateTime(ECharState.Battle, out fStartTime);
        }
        else if (0 < (eCharState_ & ECharState.KnockBack))
        {
            GetStartStateTime(ECharState.KnockBack, out fStartTime);
        }
        else if (0 < (eCharState_ & ECharState.AttackTerm))
        {
            GetStartStateTime(ECharState.AttackTerm, out fStartTime);
        }
        return fStartTime;
    }
    public float GetContinueTime(ECharState eCharState_)
    {
        float fContinueTime = 0.0f;
        if (0 < (eCharState_ & ECharState.DefendAttack))
        {
            //fContinueTime = fDefendAttackContinueTime;
            GetContinueStateTime(ECharState.DefendAttack, out fContinueTime);
        }
        else if (0 < (eCharState_ & ECharState.Powerful))
        {
            //  fContinueTime = fPowerfulContinueTime = 0;
            eraseStateTime(ECharState.Powerful);
        }
        else if (0 < (eCharState_ & ECharState.Battle))
        {
            GetContinueStateTime(ECharState.Battle, out fContinueTime);
        }
        else if (0 < (eCharState_ & ECharState.KnockBack))
        {
            GetContinueStateTime(ECharState.KnockBack, out fContinueTime);
        }
        else if (0 < (eCharState_ & ECharState.AttackTerm))
        {
            GetContinueStateTime(ECharState.AttackTerm, out fContinueTime);
        }
        return fContinueTime;
    }
    public bool IsTimeOver(ECharState eCharState_)
    {
        bool bTimeOver = false;
        float fStartTime = 0.0f;
        float fContinueTIme = 0.0f;
        if (0 < (eCharState_ & ECharState.DefendAttack))
        {
            GetStartStateTime(ECharState.DefendAttack, out fStartTime);
            GetContinueStateTime(ECharState.DefendAttack, out fContinueTIme);

            if (Time.fixedTime - fStartTime > fContinueTIme)
                bTimeOver = true;
        }
        else if (0 < (eCharState_ & ECharState.Powerful))
        {
            GetStartStateTime(ECharState.Powerful, out fStartTime);
            GetContinueStateTime(ECharState.Powerful, out fContinueTIme);
            
            if (Time.fixedTime - fStartTime > fContinueTIme)
                bTimeOver = true;
        }
        else if (0 < (eCharState_ & ECharState.Battle))
        {
            GetStartStateTime(ECharState.Battle, out fStartTime);
            GetContinueStateTime(ECharState.Battle, out fContinueTIme);

            if (Time.fixedTime - fStartTime > fContinueTIme)
                bTimeOver = true;
        }
        else if (0 < (eCharState_ & ECharState.KnockBack))
        {
            GetStartStateTime(ECharState.KnockBack, out fStartTime);
            GetContinueStateTime(ECharState.KnockBack, out fContinueTIme);

            if (Time.fixedTime - fStartTime > fContinueTIme)
                bTimeOver = true;
        }
        else if (0 < (eCharState_ & ECharState.AttackTerm))
        {
            GetStartStateTime(ECharState.AttackTerm, out fStartTime);
            GetContinueStateTime(ECharState.AttackTerm, out fContinueTIme);

            if (Time.fixedTime - fStartTime > fContinueTIme)
                bTimeOver = true;
        }
        return bTimeOver;
    }

    public string GetTextState()
    {
        string strText = "상태 : ";

        if (0 < (m_eCharState & ECharState.Alive))
            strText += "Alive ";
        if (0 < (m_eCharState & ECharState.Die))
            strText += "Die ";
        if (0 < (m_eCharState & ECharState.Battle))
            strText += "Battle ";
        if (0 < (m_eCharState & ECharState.MeetEnemy))
            strText += "MeetEnemy ";
        if (0 < (m_eCharState & ECharState.DefendAttack))
            strText += "DefendAttack ";
        if (0 < (m_eCharState & ECharState.Powerful))
            strText += "Powerful ";
        if (0 < (m_eCharState & ECharState.KnockBack))
            strText += "KnockBack ";
        if (0 < (m_eCharState & ECharState.AttackTerm))
            strText += "AttackTerm ";
        return strText;
    }
};