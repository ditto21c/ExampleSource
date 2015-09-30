using UnityEngine;
using System;
using System.Collections.Generic;
//using System.Windows.Forms;
using System.IO;

using TdStageInfo = System.Collections.Generic.Dictionary<int, StStateInfo>;
class CStageDB
{
    // 바이너리 로드
	public void Load(TextAsset kTa_)
    {
		//FileStream fs = new FileStream("Assets\\Resources\\StageDB.bytes", FileMode.Open);
        //BinaryReader br = new BinaryReader(fs);
		Stream kStream = new MemoryStream (kTa_.bytes);
		BinaryReader br = new BinaryReader(kStream);

        // *주의
        // 바이너리 세이브 순서와 로드 순서가 같아야된다. [5/13/2012 JK]
        // 바이너리 리드
        int iCount = br.ReadInt32();        // 갯수 읽기
        for (int i = 0; i < iCount; ++i)
        {
            StStateInfo kInfo = new StStateInfo();

            kInfo.m_nStageIndex = br.ReadInt32();          // 캐릭터 코드
            // 스테이지 별로 나오는 캐릭터
            kInfo.m_anCharCode = new int[(int)EStageDetail.Count];
            for (int k = 0; k < (int)EStageDetail.Count; ++k)
            {
                kInfo.m_anCharCode[k] = br.ReadInt32();     
            }
            kInfo.m_fTermTime = br.ReadSingle();

            m_tmStage.Add(kInfo.m_nStageIndex, kInfo);
        }

        //fs.Close();
        br.Close();
        kStream.Close();
    }

    public TdStageInfo m_tmStage = new TdStageInfo();

}
