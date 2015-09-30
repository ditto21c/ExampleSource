using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;


class CSkillDB
{
    // 엑셀 파일 로드 [6/302012 JK]
    public void LoadExcel()
    {

        CLoadExcel excelFarser = new CLoadExcel();
        excelFarser.LoadExcel("SkillDB.xlsx");
        excelFarser.ActiveSheet("SkillInfo");

        int iRow = 0;
        char[] strTemp = new char[1024];


        while (true)
        {
            int iCol = 0;

            string str = excelFarser.GetData(iCol++, iRow);
            if (str.Length <= 0)
            {
                break; ;
            }
            
            CSkillInfo Info = new CSkillInfo();
            // 스킬 고유 코드
            Info.m_nSkillCode = CConvert.ToInt32(str);                                  // 스킬 고유 코드
            Info.m_strName = excelFarser.GetData(iCol++, iRow);                         // 스킬 이름
            Info.m_fRange = CConvert.ToFloat(excelFarser.GetData(iCol++, iRow));        // 스킬 적용 범위
            Info.m_fRange *= 0.01f;
            Info.m_nDamage = CConvert.ToInt32(excelFarser.GetData(iCol++, iRow));       // 스킬 데미지량
            Info.m_nHeal = CConvert.ToInt32(excelFarser.GetData(iCol++, iRow));          // 스킬 힐량
            Info.m_fContinueTime = CConvert.ToFloat(excelFarser.GetData(iCol++, iRow));  // 스킬 적용되는 시간
            Info.m_strPrefab = excelFarser.GetData(iCol++, iRow);                        // 스킬 사용시 사용할 Prefab
            

            iRow++;
            if (!m_tmSkill.ContainsKey(Info.m_nSkillCode))
            {
                m_tmSkill.Add(Info.m_nSkillCode, Info);
            }
            else
            {
                MessageBox.Show("중복된 인덱스값[" + Convert.ToInt32(Info.m_nSkillCode) + "]");
                m_tmSkill.Clear();
                break;
            }
        }

        excelFarser.CleanUp();

        
    }

    // 바이너리 저장
    public void Save()
    {
        string strPath = Path.OutPath;
        strPath += "SkillDB.bytes";
        FileStream fs = new FileStream(strPath, FileMode.Create);
        BinaryWriter bw = new BinaryWriter(fs);

        // *주의
        // 바이너리 세이브 순서와 로드 순서가 같아야된다. [5/13/2012 JK]
        bw.Write(m_tmSkill.Count);             // 갯수 저장
        foreach (KeyValuePair<int, CSkillInfo> kvp in m_tmSkill)
        {
            CSkillInfo Info = kvp.Value;
            bw.Write(Info.m_nSkillCode);            // 스킬 고유 코드
            bw.Write(Info.m_strName);              // 스킬 이름
            bw.Write(Info.m_fRange);               // 스킬 적용 범위
            bw.Write(Info.m_nDamage);         // 스킬 데미지량
            bw.Write(Info.m_nHeal);           // 스킬 힐량
            bw.Write(Info.m_fContinueTime);        // 스킬 적용되는 시간
            bw.Write(Info.m_strPrefab);           // 스킬 사용시 사용할 Prefab
            

        }


        fs.Close();
        bw.Close();
    }

    // 바이너리 로드
    public void Load()
    {
        FileStream fs = new FileStream("SkillDB.bin", FileMode.Open);
        BinaryReader br = new BinaryReader(fs);

        // *주의
        // 바이너리 세이브 순서와 로드 순서가 같아야된다. [5/13/2012 JK]
        // 바이너리 리드
        int iCount = br.ReadInt32();        // 갯수 읽기
        for (int i = 0; i < iCount; ++i)
        {
            CSkillInfo Info = new CSkillInfo();


            Info.m_nSkillCode = br.ReadInt32();               // 스킬 고유 코드
            Info.m_strName = br.ReadString();                // 스킬 이름
            Info.m_fRange = br.ReadSingle();                 // 스킬 적용 범위
            Info.m_nDamage = br.ReadInt32();            // 스킬 데미지량
            Info.m_nHeal = br.ReadInt32();              // 스킬 힐량
            Info.m_fContinueTime = br.ReadSingle();          // 스킬 적용되는 시간
            Info.m_strPrefab = br.ReadString();             // 스킬 사용시 사용할 Prefab



            m_tmSkill.Add(Info.m_nSkillCode, Info);
        }
                

        fs.Close();
        br.Close();

    }

    public Dictionary<int, CSkillInfo> m_tmSkill = new Dictionary<int, CSkillInfo>();
    

}

