using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

    

public class SceneGame : MonoBehaviour {
    public GameObject kText;
    public GameObject kTextBossEventTime;
    public GameObject kTextTarCharStat;

    public GameObject kReStartButton;

    // 기본 데이터 경로
    const String m_strFilePath = "Assets\\Resources\\";
    

    // DB
	public TextAsset kTaCharDB;
	public TextAsset kTaStageDB;
	public TextAsset kTaSkillDB;

    // .ini 파일 조작
    //CControlIniFile m_kControlIniFile = new CControlIniFile(m_strFilePath);

    // .cvs
    public TextAsset kData;
    CControlCsvFileLikeIni m_kCsvParser;

    // 프리팹 리스트
    public GameObject[] kPrefabs;   //!< 몬스터 프리팹
    public GameObject kPlayer;      //!< 플레이어 프리팹

	CCharacterDB kCharDB;           //!< 캐릭터 DB
    CStageDB kStageDB;              //!< 스테이지 DB
    CSkillDB kSkillDB;              //!< 스킬 DB

    Dictionary<int, GameObject> m_tmCharacter = new Dictionary<int, GameObject>();  //!< 몬스터 리스트
    Character m_kPlayer; //!< 플레이어 인스턴스

    public float fStartXPos;        //!< 몬스터 시작위치 X
    public float fStartYPos;        //!< 몬스터 시작위치 Y

    Vector3 m_vPlayerStartPos = new Vector3(-2.08f, 1.5f, 0);
    

    int nNowCharIndex;              //!< 현재 
    int nPlayerCharIndex = 1;       //!< 현재 플레이어 캐릭터 인덱스

    public int m_nMaxStage = 0;     //!< 최대 스테이지
    public int m_nNowStage = 1;     //!< 현재 스테이지
    public EStageDetail m_eNowStateDetail = EStageDetail.First; //!< 현재 스테이지의 단계
    float m_fBossStageStartTime = 0.0f;
    // 게임 최초 시작 지점
    void Awake()
    {
        
        loadTable();
        
        m_kPlayer = (Character)kPlayer.GetComponent("Character");
        // 플레이어 데이터 세팅
        StCharInfo kCharInfo;
        if (kCharDB.m_mapCharacter.TryGetValue(1, out kCharInfo))
        {
            m_kPlayer.SetCharInfo(kCharInfo);
            m_kPlayer.SetCharIndex(nPlayerCharIndex);
        }

        loadData();

        


    }
	// Use this for initialization
	void Start () {
        //m_nNowStateDetail = EStageDetail.First;
        
        // 몬스터 데이터 세팅
        nNowCharIndex = nPlayerCharIndex + 1;
        setStage(m_nNowStage);

        kReStartButton.SetActive(false);
        //GameObject kGameObj = GameObject.FindGameObjectWithTag(kStageInfo.
	}

    void initilize()
    {

        m_eNowStateDetail = EStageDetail.First;
        m_nNowStage = 1;
        nNowCharIndex = nPlayerCharIndex + 1;
        setStage(m_nNowStage);

        m_kPlayer.Initilize();
        m_kPlayer.SetPosition(m_vPlayerStartPos);

        kReStartButton.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        updatePlayerAIMove();
        updateMonstrerAIMove();
        updateTarget();
        updatePlayerAIAction();
        updateMonsterAIAction();
        updateAttackDecision();
        updateStageClear();
        updateRenderInfo();
	}

    // 플레이어 이동 AI
    void updatePlayerAIMove()
    {
        // 재자리로 돌아감
        if (0 == m_tmCharacter.Count)
        {
            if (m_kPlayer.IsIdle() && !m_kPlayer.IsMeetEnemy())
            {
                // 전투 상태일때 제자리로 이동
                if (m_vPlayerStartPos != m_kPlayer.GetPosition())
                {
                    m_kPlayer.MoveStart(m_vPlayerStartPos);
                }
            }
        }
        // 가장 가까운 몬스터로 이동
        else
        {
            //if (m_kPlayer.IsIdle() && !m_kPlayer.IsActionAttack() && !m_kPlayer.IsMeetEnemy())
            //{
            //    Character kMonster;
            //    Vector3 vPlayerPos = m_kPlayer.GetPosition();
            //    Vector3 vGoalPos = new Vector3();
            //    float fTempDist = float.MaxValue;

            //    foreach (KeyValuePair<int, GameObject> kvp in m_tmCharacter)
            //    {
            //        GameObject kGameObject = kvp.Value;
            //        kMonster = (Character)kGameObject.GetComponent("Character");
            //        float fDist = Vector3.Distance(kMonster.GetPosition(), vPlayerPos);
            //        if (fDist < fTempDist)
            //        {
            //            fTempDist = fDist;
            //            vGoalPos = kMonster.GetPosition();
            //        }
            //    }

            //    m_kPlayer.MoveStart(vGoalPos);
            //}
        }

        
    }

    // 몬스터 이동 AI
    void updateMonstrerAIMove()
    {
        Character kMonster;
        Vector3 vPlayerPos = m_kPlayer.GetPosition();
        float fPlayerWidth = m_kPlayer.GetCharWidth();
        float fMonsterWidth;
        Vector3 vMonsterMoveEnd;
        Vector3 vEndPos;

        foreach (KeyValuePair<int, GameObject> kvp in m_tmCharacter)
        {
            GameObject kGameObject = kvp.Value;
            kMonster = (Character)kGameObject.GetComponent("Character");
            if (kMonster.IsIdle() 
                && !kMonster.IsMeetEnemy() 
                && !kMonster.IsAttackTerm()
                && !kMonster.IsKnockBack()
                )
            {
                fMonsterWidth = kMonster.GetCharWidth();
                vMonsterMoveEnd = kMonster.GetPosition();
                vEndPos = vMonsterMoveEnd;
                vEndPos.x = vPlayerPos.x + fPlayerWidth + fMonsterWidth;
                if (vMonsterMoveEnd != vEndPos)
                {
                    vMonsterMoveEnd = vEndPos;
                    kMonster.MoveStart(vMonsterMoveEnd);
                }
            }
        }

    }

    // 캐릭터 타겟 세팅
    bool updateTargetClear()
    {
        GameObject kGameObj;
        if (m_tmCharacter.TryGetValue(m_kPlayer.GetTarCharIndex(), out kGameObj))
        {
            Vector3 vPlayerPos = m_kPlayer.GetPosition();
            Vector3 vMonsterPos;
            Character kMonster;
            kMonster = (Character)kGameObj.GetComponent("Character");
            vMonsterPos = kMonster.GetPosition();
            vMonsterPos.x -= kMonster.GetCharWidth();

            float fDist = Vector3.Distance(vMonsterPos, vPlayerPos);

            if (m_kPlayer.GetAttackRange() < fDist)
            {
                m_kPlayer.ClearMeetEnemy();
                m_kPlayer.ClearTarCharIndex();
                return true;
            }
        }
        return false;

    }
    void updateTarget()
    {
        if (0 != m_kPlayer.GetTarCharIndex() && !updateTargetClear())
        {
            return;
        }
         
        
        // 가장 가까운 적 선택
        Vector3 vPlayerPos = m_kPlayer.GetPosition();
        Character kMonster;
        Vector3 vMonsterPos;
        float fDist = float.MaxValue;
        int nEnemyCharIndex = 0;
        foreach(KeyValuePair<int, GameObject> kvp in m_tmCharacter)
        {
            GameObject kGameObject = kvp.Value;
            kMonster = (Character)kGameObject.GetComponent("Character");
            if (!kMonster.IsDie())
            {
                vMonsterPos = kMonster.GetPosition();
                vMonsterPos.x -= kMonster.GetCharWidth();

                if (Vector3.Distance(vMonsterPos, vPlayerPos) < fDist)
                {
                    fDist = Vector3.Distance(vMonsterPos, vPlayerPos);

                    if(fDist < m_kPlayer.GetAttackRange())
                        nEnemyCharIndex = kMonster.GetCharIndex();
                }
            }
        }

        if (0 != nEnemyCharIndex)
        {
            m_kPlayer.SetTarCharIndex(nEnemyCharIndex);
            m_kPlayer.MeetEnemy();
            m_kPlayer.MoveStop();
        }
    }

    // 플레이어 공격 AI
    void updatePlayerAIAction()
    {
        if (m_kPlayer.IsMeetEnemy() && m_kPlayer.IsReadyActionAttack() && !m_kPlayer.IsDefend() && !m_kPlayer.IsAttackTerm() && 0 != m_kPlayer.GetTarCharIndex())
        {
            m_kPlayer.ActionAttack();
        }
    }

    // 몬스터 액션 AI
    void updateMonsterAIAction()
    {
        Vector3 vPlayerPos = m_kPlayer.GetPosition();
        vPlayerPos.x += m_kPlayer.GetCharWidth();        // 대상이 캐릭터라서 넓이값을 더한다.
        Character kMonster;
        Vector3 kMonsterPos;
        foreach (KeyValuePair<int, GameObject> kvp in m_tmCharacter)
        {
            GameObject kGameObject = kvp.Value;
            kMonster = (Character)kGameObject.GetComponent("Character");
            kMonsterPos = kMonster.GetPosition();
           
            if (Vector3.Distance(vPlayerPos, kMonsterPos) <= kMonster.GetAttackRange() && kMonster.IsReadyActionAttack() && !kMonster.IsAttackTerm() && !m_kPlayer.IsDie())
            {
                kMonster.ActionAttack();
            }
        }
    }
    // 공격 판정
    void updateAttackDecision()
    {
        Vector3 vPlayerPos = m_kPlayer.GetPosition();
        Character kMonster;
        Vector3 kMonsterPos;

        // 플레이어 공격 이벤트 프레임
        if (EEventFrame.Damage == m_kPlayer.GetNowEventFrame())
        {
            foreach (KeyValuePair<int, GameObject> kvp in m_tmCharacter)
            {
                GameObject kGameObject = kvp.Value;
                kMonster = (Character)kGameObject.GetComponent("Character");
                kMonsterPos = kMonster.GetPosition();
                kMonsterPos.x -= kMonster.GetCharWidth();    // 대상이 몬스터라서 몬스터 넓이값을 뺀다.
                if (Vector3.Distance(vPlayerPos, kMonsterPos) <= m_kPlayer.GetAttackRange())
                {
                    kMonster.Hit(m_kPlayer.GetDamage(), m_kPlayer.IsDefendAttack());
                }
            }
            m_kPlayer.NowEventFrameReset();
        }

        // 몬스터 공격 이벤트 프레임
        vPlayerPos.x += m_kPlayer.GetCharWidth();        // 대상이 캐릭터라서 넓이값을 더한다.
        foreach (KeyValuePair<int, GameObject> kvp in m_tmCharacter)
        {
            GameObject kGameObject = kvp.Value;
            kMonster = (Character)kGameObject.GetComponent("Character");
            if (EEventFrame.Damage == kMonster.GetNowEventFrame())
            {
                kMonsterPos = kMonster.GetPosition();
                if (Vector3.Distance(vPlayerPos, kMonsterPos) <= kMonster.GetAttackRange())
                {
                    m_kPlayer.Hit(kMonster.GetDamage(), kMonster.IsDefendAttack());
                }
            }
            kMonster.NowEventFrameReset();
        }

        // 플레이어 죽었는지 체크
        if (m_kPlayer.IsDie())
        {
            kReStartButton.SetActive(true);
        }
    }
   


    // 스테이지 클리어 체크
    void updateStageClear()
    {
        if (m_nMaxStage < m_nNowStage)
        {
            return;
        }
        GameObject kGameObject;
        Character kMonster;
        bool bAllMonsterDie = true;
        
        foreach (KeyValuePair<int, GameObject> kvp in m_tmCharacter)
        {
            kGameObject = kvp.Value;
            kMonster = (Character)kGameObject.GetComponent("Character");
            if (!kMonster.IsDie())
                bAllMonsterDie = false;

            // 최초 죽음 처리
            if (kMonster.IsDie() && !kMonster.IsRemoveState())
            {
                // 보상 처리
                m_kPlayer.AddExp(kMonster.GetMonsterExp());
                m_kPlayer.AddMoney(kMonster.GetMonsterPay());

                writeCharData(CCharData.EType.eExp, m_kPlayer.GetExp());
                writeCharData(CCharData.EType.eMoney, m_kPlayer.GetMoney());

                kMonster.RemoveState();

                m_kPlayer.ClearMeetEnemy();
                m_kPlayer.ClearTarCharIndex();
            }
        }
               
        // 보스전일때 예외 처리
        if (bAllMonsterDie && EStageDetail.Boss == m_eNowStateDetail)
        {
            StStateInfo kStageInfo;
            if (kStageDB.m_tmStage.TryGetValue(m_nNowStage, out kStageInfo))
            {
                if (0 == kStageInfo.m_anCharCode[(int)m_eNowStateDetail] &&
                        ((Time.fixedTime - m_fBossStageStartTime) < kStageInfo.m_fTermTime)
                    )
                {
                    bAllMonsterDie = false;
                }
            }
        }

        if (bAllMonsterDie)
        {
            // 스테이지 클리어
            if (EStageDetail.Boss == m_eNowStateDetail)
            {
                m_nNowStage++;
                m_eNowStateDetail = EStageDetail.First;
            }
            else
                m_eNowStateDetail++;

            if (m_nNowStage <= m_nMaxStage)
                setStage(m_nNowStage);
            else
            {
                // 게임 클리어
            }
        }
    }
    
    // 스테이지 세팅
    void setStage(int nStage_)
    {
        StStateInfo kStageInfo;
        GameObject kGameObj;

        foreach (KeyValuePair<int, GameObject> kvp in m_tmCharacter)
        {
            kGameObj = kvp.Value;
            Destroy(kGameObj);
        }
        m_tmCharacter.Clear();

        if (kStageDB.m_tmStage.TryGetValue(nStage_, out kStageInfo))
        {
            if (EStageDetail.Boss == m_eNowStateDetail)
            {
                m_fBossStageStartTime = Time.fixedTime;
                if (0 < kStageInfo.m_anCharCode[(int)m_eNowStateDetail])
                {
                    addMonster(kStageInfo.m_anCharCode[(int)m_eNowStateDetail]);
                }
            }
            else
            {
                int nCharCode = kStageInfo.m_anCharCode[(int)m_eNowStateDetail];
                addMonster(nCharCode);
            }
        }
    }
    // 몬스터 추가
    void addMonster(int nCharCode_)
    {
        GameObject kGameObj;
        StCharInfo kCharInfo;

        if (kCharDB.m_mapCharacter.TryGetValue(nCharCode_, out kCharInfo) && 0 < kPrefabs.Length)
        {
            for (int k = 0; k < kPrefabs.Length; k++)
            {
                if (kPrefabs[k].name == kCharInfo.m_strPrefab)
                {
                    kGameObj = GameObject.Instantiate(kPrefabs[k]);
                    kGameObj.transform.Translate(fStartXPos, fStartYPos, 0);
                    Character kChar = (Character)kGameObj.GetComponent("Character");
                    kChar.SetCharInfo(kCharInfo);
                    kChar.SetCharIndex(nNowCharIndex);
                    m_tmCharacter.Add(nNowCharIndex++, kGameObj);

                    if (int.MaxValue == nNowCharIndex) 
                        nNowCharIndex = nPlayerCharIndex + 1;

                    break;
                }
            }
        }
    }

    void updateRenderInfo()
    {
        updateRenderInfoBossEventTime();
        updateRenderInfoTarChar();
    }
    // 보스전일때 예외 처리
    void updateRenderInfoBossEventTime()
    {
        string strTemp = "";
        strTemp = String.Format("{0} Stage ", m_nNowStage);

        if (EStageDetail.Boss == m_eNowStateDetail)
        {
            StStateInfo kStageInfo;
            if (kStageDB.m_tmStage.TryGetValue(m_nNowStage, out kStageInfo))
            {
                if (0 == kStageInfo.m_anCharCode[(int)m_eNowStateDetail] &&
                        ((Time.fixedTime - m_fBossStageStartTime) < kStageInfo.m_fTermTime)
                    )
                {
                    float fTime = kStageInfo.m_fTermTime - (Time.fixedTime - m_fBossStageStartTime);
                    strTemp += String.Format("Boss Event Time : {0}(s)", (int)fTime);
                }
            }
        }
        Text text = (Text)kTextBossEventTime.GetComponent("Text");
        text.text = strTemp;
    }
    // 타겟 정보 세팅
    void updateRenderInfoTarChar()
    {
        string strTemp = "";
        Text text = (Text)kTextTarCharStat.GetComponent("Text");
        GameObject kGameObject;
        int nTarIndex = m_kPlayer.GetTarCharIndex();
        if (m_tmCharacter.TryGetValue(nTarIndex, out kGameObject))
        {
            Character kChar = (Character)kGameObject.GetComponent("Character");

            strTemp = String.Format("CharIndex:{0} Damage:{1} HP:{2}/{3} {4} {5}", nTarIndex, kChar.GetDamage(), kChar.GetNowHP(), kChar.GetMaxHP(), kChar.GetTextAction(), kChar.GetTextState());
        }
        text.text = strTemp;
    }
    // 어플 종료
    void ApplicationQuit()
    {
        Application.Quit();
    }

    void InputTouch(InputData kInputData)
    {
        if (kInputData.tmTouchPosition.ContainsKey(0))
        {
            Vector3 vTouchPosition = (Vector3)kInputData.tmTouchPosition[0];
            Ray kRay = Camera.main.ScreenPointToRay(vTouchPosition);

            string strTemp = "InputTouch_0";
            strTemp += (" PosX:" + Convert.ToString(vTouchPosition.x) + " PosY:" + Convert.ToString(vTouchPosition.x));
            Text text = (Text)kText.GetComponent("Text");
            text.text = strTemp;

            RaycastHit rayCastHit;
            if (Physics.Raycast(kRay, out rayCastHit))
            {
                strTemp = strTemp + " " + rayCastHit.collider.name;
                text.text = strTemp;

                if(CTagList.GetTagFromIndex(CTagList.ETag.eButtonAttack) == rayCastHit.collider.tag)
                {
                    PushButton kPushButton = (PushButton)rayCastHit.collider.GetComponent("PushButton");
                    kPushButton.StartEventClick();

                    if (m_kPlayer.IsReadyActionAttack())
                    {
                        m_kPlayer.ActionAttack();
                    }
                }
                else if (CTagList.GetTagFromIndex(CTagList.ETag.eButtonDefend) == rayCastHit.collider.tag)
                {
                    PushButton kPushButton = (PushButton)rayCastHit.collider.GetComponent("PushButton");
                    kPushButton.StartEventClick();

                    KeyDownDefend();
                }
                else if (CTagList.GetTagFromIndex(CTagList.ETag.eButtonPush) == rayCastHit.collider.tag)
                {
                    PushButton kPushButton = (PushButton)rayCastHit.collider.GetComponent("PushButton");
                    kPushButton.StartEventClick();

                    addMonster(kPushButton.GetSkillCode());
                }
                else
                {
                    // 적 클릭 체크
                    int nIndex = 0;
                    if(CTagList.GetIndexFromTagText(rayCastHit.collider.tag, out nIndex) &&
                        ((int)CTagList.ETag.eEnemyBegin <= nIndex && nIndex <= (int)CTagList.ETag.eEnemyEnd)
                        )
                    {
                        Character kChar = (Character)rayCastHit.collider.GetComponent("Character");
                        strTemp += String.Format("CharIndex:{0} Damage:{1} HP:{2}/{3} {4} {5}", kChar.GetCharIndex(), kChar.GetDamage(), kChar.GetNowHP(), kChar.GetMaxHP(), kChar.GetTextAction(), kChar.GetTextState());
                        text.text = strTemp;
                    }
                }
                
            }
        }
    }
        
    void KeyDownDefend()
    {
        Debug.Log("KeyDownDefend");
        if (m_kPlayer.IsIdle())
        {
            m_kPlayer.ActionDefend();
        }
    } 
    // 테이블 로드
    void loadTable()
    {
        // 캐릭터
        kCharDB = new CCharacterDB();
		kCharDB.Load(kTaCharDB);

        // 스테이지
        kStageDB = new CStageDB();
		kStageDB.Load(kTaStageDB);
        m_nMaxStage = kStageDB.m_tmStage.Count;
        
        // 스킬
        kSkillDB = new CSkillDB();
		kSkillDB.Load(kTaSkillDB);
    }

    // 파일 읽어서 데이터 세팅
    void loadData()
    {
        m_kCsvParser = new CControlCsvFileLikeIni(kData, "\\Resources\\" + "Config.csv");
        m_kCsvParser.LoadData();

        
        uint uValue = 0;
        CControlCsvFileLikeIni.EControlIniFileRet eRet = m_kCsvParser.GetInt("Character", "CriticalRate", out uValue);

        if (CControlCsvFileLikeIni.EControlIniFileRet.Succend != eRet)
        {
            Debug.Log(string.Format("Failed loadData Character CriticalRate {0}", eRet));
        }
        else
            m_kPlayer.SetExStat(CCharData.EType.eCriticalRate, (int)uValue);

        eRet = m_kCsvParser.GetInt("Character", "CriticalDamage", out uValue);

        if (CControlCsvFileLikeIni.EControlIniFileRet.Succend != eRet)
        {
            Debug.Log(string.Format("Failed loadData Character CriticalDamage {0}", eRet));
        }
        else
            m_kPlayer.SetExStat(CCharData.EType.eCriticalDamage, (int)uValue);

        int nTemp;
        int nValue;
        for (int i = (int)CCharData.EType.eBegin; i < (int)CCharData.EType.eCount; ++i)
        {
            nValue = PlayerPrefs.GetInt(CCharData.GetTypeFromIndex((CCharData.EType)i));
            if ((int)CCharData.EType.eCriticalRate == i || (int)CCharData.EType.eCriticalDamage == i)
            {
                if (m_kPlayer.GetExStat((CCharData.EType)i, out nTemp))
                    nValue += nTemp;
            }
            m_kPlayer.SetExStat((CCharData.EType)i, nValue);
        }

        m_kPlayer.SetExp(0);
        writeCharData(CCharData.EType.eExp, 0);
    }
    
    // 플레이어 데이터 저장
    void writeCharData(CCharData.EType eType_, int nValue_)
    {
        setPlayerPrefs(CCharData.GetTypeFromIndex(eType_), nValue_);
    }
    void setPlayerPrefs(string strKey_, int nValue_)
    {
        // try to save the string (it will fail in a webplayer build)
        try
        {
            PlayerPrefs.SetInt(strKey_, nValue_);
            PlayerPrefs.Save();
        }
        // handle the error
        catch (System.Exception err)
        {
            Debug.Log("Got: " + err);
        }
    }
   

    public void ReStart()
    {
        initilize();

        m_kPlayer.SetExp(0);
        writeCharData(CCharData.EType.eExp, 0);
    }
}
