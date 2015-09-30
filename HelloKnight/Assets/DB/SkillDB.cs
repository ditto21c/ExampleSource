using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;

using TdSkillInfo = System.Collections.Generic.Dictionary<int, StSkillInfo>;

class CSkillDB
{
    // 바이너리 로드
	public void Load(TextAsset kTa_)
	{
		//FileStream fs = new FileStream("Assets\\Resources\\SkillDB.bytes", FileMode.Open);

        //BinaryReader br = new BinaryReader(fs);
		Stream kStream = new MemoryStream (kTa_.bytes);
		BinaryReader br = new BinaryReader(kStream);

        // *주의
        // 바이너리 세이브 순서와 로드 순서가 같아야된다. [5/13/2012 JK]
        // 바이너리 리드
        int iCount = br.ReadInt32();        // 갯수 읽기
        for (int i = 0; i < iCount; ++i)
        {
            StSkillInfo Info = new StSkillInfo();

            Info.m_nSkillCode = br.ReadInt32();               // 스킬 고유 코드
            Info.m_strName = br.ReadString();                // 스킬 이름
            Info.m_fRange = br.ReadSingle();                 // 스킬 적용 범위
            Info.m_nDamage = br.ReadInt32();            // 스킬 데미지량
            Info.m_nHeal = br.ReadInt32();              // 스킬 힐량
            Info.m_fContinueTime = br.ReadSingle();          // 스킬 적용되는 시간
            Info.m_strPrefab = br.ReadString();             // 스킬 사용시 사용할 Prefab

            m_tmSkill.Add(Info.m_nSkillCode, Info);
        }

        //fs.Close();
        br.Close();
        kStream.Close();

    }

    public TdSkillInfo m_tmSkill = new TdSkillInfo();
    

}

