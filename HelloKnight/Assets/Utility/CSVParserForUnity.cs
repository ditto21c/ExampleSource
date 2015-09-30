/*
 * 
 * .csv 파일을 읽고 데이터를 저장
 * 
 * 
*/
 
using UnityEngine;
using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

using TdData = System.Collections.Generic.Dictionary<int, StData>;
using KvpData = System.Collections.Generic.KeyValuePair<int, StData>;

public struct StData
{
    public int m_nLine;             //!< 몇번째 라인인지
    public string m_strLine;        //!< 라인의 문자열
    public string[] m_strComma;     //!< 라인의 (,) 단위 문자열 
}

public class CSVParserForUnity
{
    protected TextAsset m_kTextAsset;             //!< .csv 파일
    public string m_strTextAssetFileName;      //!< .csv 파일 이름 
    protected TdData m_tmData = new TdData();     //!< 데이터 리스트

    static char cLine = '\n';           //!< .csv 파일 줄 단위 체크
    static char cComma = ',';           //!< .csv 파일 , 단위 체크
    
    protected CSVParserForUnity()  {}
    public CSVParserForUnity(TextAsset kTextAsset_, string strFileName_)
    {
        m_kTextAsset = kTextAsset_;
        m_strTextAssetFileName = strFileName_;
    }

    public bool LoadData()
    {
        // 라인별로 텍스트 저장
        string[] strData = m_kTextAsset.text.Split(cLine);
        
        int nIndex = 0;
        foreach (string strLine in strData )
        {
            if (0 < strLine.Length)
            {
                StData kData;
                kData.m_nLine = nIndex++;
                kData.m_strLine = strLine.Substring(0, strLine.Length - 1);

                kData.m_strComma = strLine.Split(cComma);
                kData.m_strComma[kData.m_strComma.Length - 1] = kData.m_strComma[kData.m_strComma.Length - 1].Substring(0, kData.m_strComma[kData.m_strComma.Length-1].Length - 1);

                m_tmData.Add(kData.m_nLine, kData);
            }
        }

        //Debug.Log(strData[0]);

        return true;
    }

    public bool SaveData()
    {
        string strSaveData = "";

        foreach (KvpData kData in m_tmData)
        {
            if (0 < kData.Value.m_strLine.Length)
            {
                if ('[' == kData.Value.m_strLine[0])
                {
                    strSaveData += kData.Value.m_strLine + "\r\n";
                }
                else
                {
                    for (int i = 0; i < kData.Value.m_strComma.Length; ++i)
                    {

                        if (1 == i)
                        {
                            string strTemp = kData.Value.m_strComma[i];
                            int n = Convert.ToInt32(strTemp);
                            n++;
                            strTemp = Convert.ToString(n);

                            strSaveData += strTemp;
                            strSaveData += "\r\n";
                        }
                        else
                        {
                            strSaveData += kData.Value.m_strComma[i];
                            strSaveData += ",";
                        }
                    }
                }
            }
            //strSaveData += string.Format("{0}\n", kData.Value.m_strLine);
        }
        
       // Stream kStream = new MemoryStream (m_kTextAsset.bytes);
       // StreamWriter kStreamReader = new StreamWriter(kStream);
       // kStreamReader.Write(strSaveData);
       //// File.WriteAllText(getPath() + m_strTextAssetFileName, strSaveData);
       // kStreamReader.Close();

        
        
#if UNITY_EDITOR
        AssetDatabase.Refresh();
#endif
      

        //Debug.Log(strSaveData);

        return true;
    }

    // CVS 파일 경로 얻기
    public string getPath()
    {
#if UNITY_EDITOR
        return Application.dataPath;
#elif UNITY_ANDROID
        return Application.persistentDataPath;
#elif UNITY_IPHONE
        return GetiPhoneDocumentsPath();
#else
        return Application.dataPath;
#endif
    }




};

