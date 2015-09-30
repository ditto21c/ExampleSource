using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;

class CStageDB
{
    // 엑셀 파일 로드 [4/29/2012 JK]
    public void LoadExcel()
    {

        CLoadExcel excelFarser = new CLoadExcel();
        excelFarser.LoadExcel("StageDB.xlsx");
        excelFarser.ActiveSheet("StageInfo");

        int iRow = 0;
        int iCol;

        while (true)
        {
            iCol = 0;

            string str = excelFarser.GetData(iCol++, iRow);
            if (str.Length <= 0)
            {
                break;
            }
            CStateInfo Info = new CStateInfo();

            Info.m_nStageIndex = CConvert.ToInt32(str);                                           // 몇번째 스테이지 인지
            for (int i = 0; i < 3; ++i)
            {
                Info.m_anCharCode[i] = CConvert.ToInt32(excelFarser.GetData(iCol++, iRow));         // 스테이지 별로 나오는 캐릭터
            }
            Info.m_fTermTime = CConvert.ToFloat(excelFarser.GetData(iCol++, iRow));                 // 보스 나오기 전 텀 시간
            Info.m_anCharCode[3] = CConvert.ToInt32(excelFarser.GetData(iCol++, iRow)); 
                        
            iRow++;

            if (!m_tmStage.ContainsKey(Info.m_nStageIndex))
            {
                m_tmStage.Add(Info.m_nStageIndex, Info);
            }
            else
            {
                MessageBox.Show("중복된 인덱스값[" + Convert.ToString(Info.m_nStageIndex) + "]");
                break;
            }
        }

        excelFarser.CleanUp();
    }

    // 바이너리 저장
    public void Save()
    {
        string strPath = Path.OutPath;
        strPath += "StageDB.bytes";
        FileStream fs = new FileStream(strPath, FileMode.Create);
        BinaryWriter bw = new BinaryWriter(fs);

        // *주의
        // 바이너리 세이브 순서와 로드 순서가 같아야된다. [5/13/2012 JK]
        bw.Write(m_tmStage.Count);             // 갯수 저장
        foreach (KeyValuePair<int, CStateInfo> kvp in m_tmStage)
        {
            CStateInfo kInfo = kvp.Value;
            bw.Write(kInfo.m_nStageIndex);           // 캐릭터 코드
            for (int i = 0; i < 4; ++i)
            {
                bw.Write(kInfo.m_anCharCode[i]);      // 스테이지 별로 나오는 캐릭터
            }
            bw.Write(kInfo.m_fTermTime);           // 보스 나오기 전 텀 시간
        }

        fs.Close();
        bw.Close();
    }

    // 바이너리 로드
    public void Load()
    {
        FileStream fs = new FileStream("StageDB.bin", FileMode.Open);
        BinaryReader br = new BinaryReader(fs);

        // *주의
        // 바이너리 세이브 순서와 로드 순서가 같아야된다. [5/13/2012 JK]
        // 바이너리 리드
        int iCount = br.ReadInt32();        // 갯수 읽기
        for (int i = 0; i < iCount; ++i)
        {
            CStateInfo kInfo = new CStateInfo();

            kInfo.m_nStageIndex = br.ReadInt32();          // 캐릭터 코드
            for (int k = 0; k < 4; ++k)
            {
                kInfo.m_anCharCode[k] = br.ReadInt32();     // 스테이지 별로 나오는 캐릭터
            }
            kInfo.m_fTermTime = br.ReadSingle();

            m_tmStage.Add(kInfo.m_nStageIndex, kInfo);
        }

        fs.Close();
        br.Close();
    }

    public Dictionary<int, CStateInfo> m_tmStage = new Dictionary<int, CStateInfo>();

}
