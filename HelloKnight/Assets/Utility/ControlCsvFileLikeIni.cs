/*
 * 
 * .csv 파일을 ini 파일 조작하듯이 제어
 * 
 * 
*/

using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

//using TmControlCsv = System.Collections.Generic.Dictionary<String, CEntity>;
//using KvpControlCsv = System.Collections.Generic.KeyValuePair<String, CEntity>;
//using TmControlCsvNode = System.Collections.Generic.Dictionary<String, String>;

public class CEntity
{
    Dictionary<String, String> m_tmNode = new Dictionary<String, String>();

    public bool AddNode(String strKey_, String strValue_)
    {
        if (!m_tmNode.ContainsKey(strKey_))
        {
            m_tmNode.Add(strKey_, strValue_);
            return true;
        }
        return false;
    }

    public bool GetValue(String strKey_, out String strValue_o)
    {
        return m_tmNode.TryGetValue(strKey_, out strValue_o);
    }

    public bool SetValue(String strKey_, String strValue_)
    {
        
        if (m_tmNode.ContainsKey(strKey_))
        {
            m_tmNode[strKey_] = strValue_;
            return true;
        }
        return false;
    }
};

public class CControlCsvFileLikeIni : CSVParserForUnity
{

    public enum EControlIniFileRet
    {
        Succend = 0,                //!< 성공
        Failed_NotExistIniFile,     //!< Ini 파일이 존재 x
        Failed_BadData,             //!< 잘못된 데이터다
        Failed_NoData,              //!< 데이터가 존재x
        Failed_NotAppName,          //!< appName이 아니다
    };

    Dictionary<String, CEntity> m_tmControlCsv = new Dictionary<String, CEntity>();

    protected CControlCsvFileLikeIni() { }
    public CControlCsvFileLikeIni(TextAsset kTextAsset_, String strFileName_)
        : base(kTextAsset_, strFileName_)
    {
    }

    // 데이터 로드
    public bool LoadData()
    {
        base.LoadData();
        
        m_tmControlCsv.Clear();

        // 데이터 재 정렬
        bool bClear = true; // 무결성 체크
        String strTemp;
        String preAppName = "";
        foreach(KeyValuePair<int, StData> kData in m_tmData)
        {
            if (0 == kData.Value.m_strLine.Length)
                continue;

            EControlIniFileRet ret = isRightAppName(kData.Value.m_strLine, out strTemp);
            if (EControlIniFileRet.Succend == ret)
            {
                if (!m_tmControlCsv.ContainsKey(strTemp))
                {
                    preAppName = strTemp;
                    CEntity kEntity = new CEntity();
                    m_tmControlCsv.Add(strTemp, kEntity);
                }
                else
                {
                    strTemp = String.Format("CControlCsvFileLikeIni - m_tmControlCsv Same Key{0} ]", strTemp);
                    Debug.Log(strTemp);
                    bClear = false;
                    
                }
            }
            else if (EControlIniFileRet.Failed_NotAppName == ret && 2 < preAppName.Length)
            {
                CEntity kEntity;
                if (m_tmControlCsv.TryGetValue(preAppName, out kEntity))
                {
                    if (!kEntity.AddNode(kData.Value.m_strComma[0], kData.Value.m_strComma[1]))
                    {
                        strTemp = String.Format("kEntity.AddNode Failed SameKey[ {0} ]", kData.Value.m_strComma[0]);
                        Debug.Log(strTemp);
                        bClear = false;
                    }
                }
            }
            else
            {
                strTemp = String.Format("CControlCsvFileLikeIni - LoadData Failed[ {0} ] Text [ {1} ]", ret, kData.Value.m_strLine);
                Debug.Log(strTemp);
                bClear = false;
            }

        }

        return bClear;
    }

    // 데이터 저장
    public bool SaveData()
    {
        bool bClear = true; // 무결성 체크
        CEntity kEntity;
        String strAppName = "";
        bool bAppNameStart;
        String strTemp;
        foreach (KeyValuePair<String, CEntity> kCsv in m_tmControlCsv)
        {
            strAppName = String.Format("<{0}>", kCsv.Key);

            bAppNameStart = false;
            foreach (KeyValuePair<int, StData> kData in m_tmData)
            {

                if (bAppNameStart && 0 < kData.Value.m_strLine.Length && '[' == kData.Value.m_strLine[0])
                    break;

                if (bAppNameStart)
                {
                    if (kCsv.Value.GetValue(kData.Value.m_strComma[0], out strTemp))
                    {
                        kData.Value.m_strComma[1] = strTemp ;
                    }
                }

                if (0 == String.Compare(strAppName, 0, kData.Value.m_strLine, 0, strAppName.Length))
                {
                    bAppNameStart = true;
                }
            }
        }

        base.SaveData();

        return true;
    }

    // 문자열 세팅
    public EControlIniFileRet WriteString(
        String strAppName_,
        String strKeyName_,
        String strString_
        )
    {
        CEntity kEntity;

        if (m_tmControlCsv.TryGetValue(strAppName_, out kEntity))
        {
            if (kEntity.SetValue(strKeyName_, strString_))
            {
                return EControlIniFileRet.Succend;
            }
        }

        return EControlIniFileRet.Failed_NoData;
    }

    // 문자열 얻기
    public EControlIniFileRet GetString(
         String strAppName_,
         String strKeyName_,
         out String strReturnedString_o
    )
    {
        strReturnedString_o = "";
        CEntity kEntity;
        
        if (m_tmControlCsv.TryGetValue(strAppName_, out kEntity))
        {
            if (kEntity.GetValue(strKeyName_, out strReturnedString_o))
            {
                return EControlIniFileRet.Succend;
            }
        }
        
        return EControlIniFileRet.Failed_NoData;
    }

    //--------------------------------------------
    // 숫자 얻기
    //--------------------------------------------
    public EControlIniFileRet GetInt(
        String strAppName_,
        String strKeyName_,
        out uint uValue_o
    )
    {
        uValue_o = 0;
        CEntity kEntity;
        String strTemp = "";
        if (m_tmControlCsv.TryGetValue(strAppName_, out kEntity))
        {
            if (kEntity.GetValue(strKeyName_, out strTemp))
            {
                uValue_o = (uint)Convert.ToInt32(strTemp);
                return EControlIniFileRet.Succend;
            }
        }

        return EControlIniFileRet.Failed_NoData;
        
    }

    // 재대로된 appName인지 체크한다.
    // 정상이면 []가 제거된 strAppName_o을 리턴한다.
    // strAppName_의 마지막 문자에 ?이 첨부 되어 있다.
    EControlIniFileRet isRightAppName(String strAppName_, out String strAppName_o)
    {
        strAppName_o = "";
        if (0 == strAppName_.Length)
            return EControlIniFileRet.Failed_NoData;
        
        //Debug.Log(strAppName_);
        //for(int i=0; i<strAppName_.Length; ++i)
        //    Debug.Log(strAppName_[i]);
        //Debug.Log(strAppName_[(strAppName_.Length - 1)]);

        if('[' != strAppName_[0] || ']' != strAppName_[strAppName_.Length -1] || strAppName_.Length < 3)
            return EControlIniFileRet.Failed_NotAppName;
        
        if('[' == strAppName_[0] && ']' != strAppName_[strAppName_.Length -1])
            return EControlIniFileRet.Failed_BadData;

        strAppName_o = strAppName_.Substring(1, strAppName_.Length - 2);
        return EControlIniFileRet.Succend;
    }
    


}
