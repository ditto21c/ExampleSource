using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;

public class CItemDB
{
    // 엑셀 파일 로드 [4/29/2012 JK]
    public void LoadExcel()
    {

        CLoadExcel excelFarser = new CLoadExcel();
        excelFarser.LoadExcel("ItemDB.xlsx");
        excelFarser.ActiveSheet("ItemInfo");

        int iRow = 0;
        string strTemp;


        while (true)
        {
            int iCol = 0;

            string str = excelFarser.GetData(iCol++, iRow);
            if (str.Length <= 0)
            {
                break; ;
            }
            CItemInfo info = new CItemInfo();
            int nIndex = CConvert.ToInt32(str);
            info.m_nIndex = nIndex;
            info.m_strName = excelFarser.GetData(iCol++, iRow);
            info.m_nMainType = CConvert.ToInt32(excelFarser.GetData(iCol++, iRow));
            info.m_nDetailType = CConvert.ToInt32(excelFarser.GetData(iCol++, iRow));
            info.m_fDamage = CConvert.ToFloat(excelFarser.GetData(iCol++, iRow));
            info.m_fRange = CConvert.ToFloat(excelFarser.GetData(iCol++, iRow));
            info.m_fAttackSpeed = CConvert.ToFloat(excelFarser.GetData(iCol++, iRow));
            info.m_fAngle = CConvert.ToFloat(excelFarser.GetData(iCol++, iRow));
            info.m_fCharMoveSpeed = CConvert.ToFloat(excelFarser.GetData(iCol++, iRow));
            info.m_nWeaponSkillIndex = CConvert.ToInt32(excelFarser.GetData(iCol++, iRow));
            info.m_nUseableLevel = CConvert.ToInt32(excelFarser.GetData(iCol++, iRow));
            info.m_nPrice = CConvert.ToInt32(excelFarser.GetData(iCol++, iRow));
            info.m_strModel = excelFarser.GetData(iCol++, iRow);
            info.m_strSound = excelFarser.GetData(iCol++, iRow);
            info.m_strIcon = excelFarser.GetData(iCol++, iRow);

            // 아이템 기능
            int iTemp = 0, nCharCount = 0;
            char[] cTemp = new char[128];
            strTemp = excelFarser.GetData(iCol++, iRow);
            while (iTemp < strTemp.Length)
            {
                if (strTemp[iTemp] != ';')
                {
                    cTemp[nCharCount++] = strTemp[iTemp++];
                }
                else
                {
                    cTemp[nCharCount] = '0';
                    nCharCount = 0;
                    switch (Convert.ToInt32(cTemp))
                    {
                        case 1:
                            info.m_nFuntion |= 0x0000000000000001;
                            break;
                        case 2:
                            info.m_nFuntion |= 0x0000000000000001;
                            break;
                    }
                }
            }
            info.m_nHP = CConvert.ToInt32(excelFarser.GetData(iCol++, iRow));
            info.m_nMP = CConvert.ToInt32(excelFarser.GetData(iCol++, iRow));
            

            iRow++;
            if (!m_mapItem.ContainsKey(nIndex))
            {
                m_mapItem.Add(nIndex, info);
            }
            else
            {
                MessageBox.Show("중복된 인덱스값[" + Convert.ToInt32(nIndex) + "]");
                m_mapItem.Clear();
                break;
            }
        }

        excelFarser.CleanUp();
    }

    // 바이너리 저장
    public void Save()
    {
        string strPath = Path.OutPath;
        strPath += "ItemDB.bytes";
        FileStream fs = new FileStream(strPath, FileMode.Create);
        BinaryWriter bw = new BinaryWriter(fs);

        // *주의
        // 바이너리 세이브 순서와 로드 순서가 같아야된다. [5/13/2012 JK]
        bw.Write(m_mapItem.Count);             // 갯수 저장
        foreach (KeyValuePair<int, CItemInfo> kvp in m_mapItem)
        {
            CItemInfo info = kvp.Value;
            bw.Write(info.m_nIndex);                     // 아이템 인덱스
            bw.Write(info.m_strName);                    // 아이템 이름
            bw.Write(info.m_nMainType);                  // 아이템 기본 타입
            bw.Write(info.m_nDetailType);                // 아이템 상세 타입
            bw.Write(info.m_fDamage);                    // 아이템 데미지
            bw.Write(info.m_fRange);                     // 아이템 사거리
            bw.Write(info.m_fAttackSpeed);               // 아이템 공격 속도
            bw.Write(info.m_fAngle);                     // 아이템 각도
            bw.Write(info.m_fCharMoveSpeed);             // 캐릭터 이동 속도 변화
            bw.Write(info.m_nWeaponSkillIndex);          // 무기에 따른 스킬 인덱스
            bw.Write(info.m_nUseableLevel);              // 무기 사용 레벨
            bw.Write(info.m_nPrice);                     // 무기 가격
            bw.Write(info.m_strModel);                   // 무기 모델
            bw.Write(info.m_strSound);                   // 무기 사용시 사운드
            bw.Write(info.m_strIcon);                    // 아이콘 이름
            bw.Write(info.m_nFuntion);                   // 아이템 기능
            bw.Write(info.m_nHP);                        // HP 증가
            bw.Write(info.m_nMP);                        // MP 증가
        }

        fs.Close();
        bw.Close();
    }

    // 바이너리 로드
    public void Load()
    {
        FileStream fs = new FileStream("ItemDB.bin", FileMode.Open);
        BinaryReader br = new BinaryReader(fs);

        // *주의
        // 바이너리 세이브 순서와 로드 순서가 같아야된다. [5/13/2012 JK]
        // 바이너리 리드
        int iCount = br.ReadInt32();        // 갯수 읽기
        for (int i = 0; i < iCount; ++i)
        {
            CItemInfo info = new CItemInfo();


            info.m_nIndex = br.ReadInt32();                 // 아이템 인덱스         
            info.m_strName = br.ReadString();               // 아이템 이름    
            info.m_nMainType = br.ReadInt32();              // 아이템 기본 타입    
            info.m_nDetailType = br.ReadInt32();            // 아이템 상세 타입    
            info.m_fDamage = br.ReadSingle();               // 아이템 데미지    
            info.m_fRange = br.ReadSingle();                // 아이템 사거리    
            info.m_fAttackSpeed = br.ReadSingle();          // 아이템 공격 속도    
            info.m_fAngle = br.ReadSingle();                // 아이템 각도    
            info.m_fCharMoveSpeed = br.ReadSingle();        // 캐릭터 이동 속도 변화    
            info.m_nWeaponSkillIndex = br.ReadInt32();      // 무기에 따른 스킬 인덱스    
            info.m_nUseableLevel = br.ReadInt32();          // 무기 사용 레벨    
            info.m_nPrice = br.ReadInt32();                 // 무기 가격    
            info.m_strModel = br.ReadString();              // 무기 모델    
            info.m_strSound = br.ReadString();              // 무기 사용시 사운드    
            info.m_strIcon = br.ReadString();               // 아이콘 이름    
            info.m_nFuntion = br.ReadInt32();               // 아이템 기능    
            info.m_nHP = br.ReadInt32();                    // HP 증가    
            info.m_nMP = br.ReadInt32();                    // MP 증가    
           

            m_mapItem.Add(info.m_nIndex, info);
        }

        fs.Close();
        br.Close();

    }

    public Dictionary<int, CItemInfo> m_mapItem = new Dictionary<int, CItemInfo>();
 
}