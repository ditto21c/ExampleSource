using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;

using TdCharInfo = System.Collections.Generic.Dictionary<int, StCharInfo>;

class CCharacterDB
{
    // 바이너리 로드
	public void Load(TextAsset kTa_)
	{
        //FileStream fs = new FileStream("Assets\\Resources\\CharDB.bytes", FileMode.Open);
		Stream kStream = new MemoryStream (kTa_.bytes);
		BinaryReader br = new BinaryReader(kStream);
		
		// *주의
        // 바이너리 세이브 순서와 로드 순서가 같아야된다. [5/13/2012 JK]
        // 바이너리 리드
        int iCount = br.ReadInt32();        // 갯수 읽기
        for (int i = 0; i < iCount; ++i )
        {
            StCharInfo charInfo = new StCharInfo();

            charInfo.m_nCode = br.ReadInt32();          // 캐릭터 코드
            charInfo.m_strName = br.ReadString();       // 캐릭터 이름
            charInfo.m_strPrefab = br.ReadString();     // Assets 안에 있는 Prefab 텍스트명
            charInfo.m_eCharType = (ECharType)br.ReadInt32();      // 캐릭터 타입 0:플레이어 1:몬스터
            charInfo.m_nNowHP = br.ReadInt32();         // 현재 체력
            charInfo.m_nMaxHP = br.ReadInt32();         // 최대 체력
            charInfo.m_fCharWidth = (float)br.ReadInt32();     // 캐릭터 넓이
            charInfo.m_fCharWidth *= 0.01f;
            charInfo.m_fMoveSpeed = br.ReadSingle();    // 이동 속도(s) 목표지점까지 몇초만에 도착하는지
            charInfo.m_nAttackDamage = br.ReadInt32();  // 공격 데미지
            charInfo.m_fAttackRange = (float)br.ReadInt32();   // 캐릭터 기본 사거리
            charInfo.m_fAttackRange *= 0.01f;
            charInfo.m_fAttackSpeed = br.ReadSingle();  // 캐릭터 공격 속도 1: 1초에 한번
            charInfo.m_nExp = br.ReadInt32();           // 캐릭터가 죽었을시 제공되는 경험치
            charInfo.m_fAfterAttackIdleTermTime = br.ReadSingle(); // 공격후 Idle 유지 시간
            charInfo.m_nPay = br.ReadInt32(); // 보상 (돈)

            m_mapCharacter.Add(charInfo.m_nCode, charInfo);
        }

        //fs.Close();
        br.Close();
        kStream.Close();

    }

    public TdCharInfo m_mapCharacter = new TdCharInfo();

 
}
