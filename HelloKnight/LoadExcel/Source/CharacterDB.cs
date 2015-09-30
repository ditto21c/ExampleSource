using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;




class CCharacterDB
{
    // 엑셀 파일 로드 [4/29/2012 JK]
    public void LoadExcel()
    {
        
        CLoadExcel excelFarser = new CLoadExcel();
        excelFarser.LoadExcel("CharacterDB.xlsx");
        excelFarser.ActiveSheet("CharInfo");
        
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
            CCharInfo charInfo = new CCharInfo();

            charInfo.m_nCode = CConvert.ToInt32(str);                                           // 캐릭터 코드
            charInfo.m_strName = excelFarser.GetData(iCol++, iRow);                             // 캐릭터 이름
            charInfo.m_strPrefab = excelFarser.GetData(iCol++, iRow);                           // Assets 안에 있는 Prefab 텍스트명
            charInfo.m_nCharType = CConvert.ToInt32(excelFarser.GetData(iCol++, iRow));         // 캐릭터 타입 0:플레이어 1:몬스터
            charInfo.m_nMaxHP = CConvert.ToInt32(excelFarser.GetData(iCol++, iRow));            // 최대 체력
            charInfo.m_nNowHP = charInfo.m_nMaxHP;                                              // 현재 체력
            charInfo.m_nCharWidth = CConvert.ToInt32(excelFarser.GetData(iCol++, iRow));          // 캐릭터 넓이
            charInfo.m_fMoveSpeed = CConvert.ToFloat(excelFarser.GetData(iCol++, iRow));          // 이동 속도(s) 목표지점까지 몇초만에 도착하는지
            charInfo.m_nAttackDamage = CConvert.ToInt32(excelFarser.GetData(iCol++, iRow));       // 공격 데미지
            charInfo.m_nAttackRange = CConvert.ToInt32(excelFarser.GetData(iCol++, iRow));      // 캐릭터 기본 사거리
            charInfo.m_fAttackSpeed = CConvert.ToFloat(excelFarser.GetData(iCol++, iRow));      // 캐릭터 공격 속도 1: 1초에 한번
            charInfo.m_nExp = CConvert.ToInt32(excelFarser.GetData(iCol++, iRow));              // 캐릭터가 죽었을시 제공되는 경험치
            charInfo.m_fAfterAttackIdleTermTime = CConvert.ToFloat(excelFarser.GetData(iCol++, iRow)); // 공격후 Idle 유지 시간
            charInfo.m_nPay = CConvert.ToInt32(excelFarser.GetData(iCol++, iRow));  // 보상 (돈)

            iRow++;
            if (!m_mapCharacter.ContainsKey(charInfo.m_nCode))
            {
                m_mapCharacter.Add(charInfo.m_nCode, charInfo);
            }
            else
            {
                MessageBox.Show("중복된 인덱스값[" + Convert.ToString(charInfo.m_nCode) + "]");
                m_mapCharacter.Clear();
                break;
            }
        }

        excelFarser.CleanUp();
    }

    // 바이너리 저장
    public void Save()
    {
        string strPath = Path.OutPath;
        strPath += "CharDB.bytes";
        FileStream fs = new FileStream(strPath, FileMode.Create);
        BinaryWriter bw = new BinaryWriter(fs);

        // *주의
        // 바이너리 세이브 순서와 로드 순서가 같아야된다. [5/13/2012 JK]
        bw.Write(m_mapCharacter.Count);             // 갯수 저장
        foreach (KeyValuePair<int, CCharInfo> kvp in m_mapCharacter)
        {
            CCharInfo charInfo = kvp.Value;
            bw.Write(charInfo.m_nCode);         // 캐릭터 코드
            bw.Write(charInfo.m_strName);       // 캐릭터 이름
            bw.Write(charInfo.m_strPrefab);     // Assets 안에 있는 Prefab 텍스트명
            bw.Write(charInfo.m_nCharType);     // 캐릭터 타입 0:플레이어 1:몬스터
            bw.Write(charInfo.m_nNowHP);        // 현재 체력
            bw.Write(charInfo.m_nMaxHP);        // 최대 체력
            bw.Write(charInfo.m_nCharWidth);    // 캐릭터 넓이
            bw.Write(charInfo.m_fMoveSpeed);    // 이동 속도(s) 목표지점까지 몇초만에 도착하는지
            bw.Write(charInfo.m_nAttackDamage); // 공격 데미지
            bw.Write(charInfo.m_nAttackRange);  // 캐릭터 기본 사거리
            bw.Write(charInfo.m_fAttackSpeed);  // 캐릭터 공격 속도 1: 1초에 한번
            bw.Write(charInfo.m_nExp);          // 캐릭터가 죽었을시 제공되는 경험치
            bw.Write(charInfo.m_fAfterAttackIdleTermTime); // 공격후 Idle 유지 시간
            bw.Write(charInfo.m_nPay);  // 보상
        }

        fs.Close();
        bw.Close();
    }

    // 바이너리 로드
    public void Load()
    {
        FileStream fs = new FileStream("CharDB.bin", FileMode.Open);
        BinaryReader br = new BinaryReader(fs);

        // *주의
        // 바이너리 세이브 순서와 로드 순서가 같아야된다. [5/13/2012 JK]
        // 바이너리 리드
        int iCount = br.ReadInt32();        // 갯수 읽기
        for (int i = 0; i < iCount; ++i )
        {
            CCharInfo charInfo = new CCharInfo();

            charInfo.m_nCode = br.ReadInt32();          // 캐릭터 코드
            charInfo.m_strName = br.ReadString();       // 캐릭터 이름
            charInfo.m_strPrefab = br.ReadString();       // Assets 안에 있는 Prefab 텍스트명
            charInfo.m_nCharType = br.ReadInt32();      // 캐릭터 타입 0:플레이어 1:몬스터
            charInfo.m_nNowHP = br.ReadInt32();         // 현재 체력
            charInfo.m_nMaxHP = br.ReadInt32();         // 최대 체력
            charInfo.m_nCharWidth = br.ReadInt32();     // 캐릭터 넓이
            charInfo.m_fMoveSpeed = br.ReadSingle();    // 이동 속도(s) 목표지점까지 몇초만에 도착하는지
            charInfo.m_nAttackDamage = br.ReadInt32();  // 공격 데미지
            charInfo.m_nAttackRange = br.ReadInt32();   // 캐릭터 기본 사거리
            charInfo.m_fAttackSpeed = br.ReadSingle();  // 캐릭터 공격 속도 1: 1초에 한번
            charInfo.m_nExp = br.ReadInt32();           // 캐릭터가 죽었을시 제공되는 경험치
            charInfo.m_fAfterAttackIdleTermTime = br.ReadSingle(); // 공격후 Idle 유지 시간
            charInfo.m_nPay = br.ReadInt32();   // 보상(돈)
            m_mapCharacter.Add(charInfo.m_nCode, charInfo);
        }

        fs.Close();
        br.Close();

    }

    public Dictionary<int, CCharInfo> m_mapCharacter = new Dictionary<int, CCharInfo>();

 
}
