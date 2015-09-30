using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CImageInfo : CMoveHavior
{
    public GameObject goBackImage;
};

public class BackImage : MonoBehaviour {
    Vector3 m_vStartPos = new Vector3(1.5f, 3.0f, 0);
    Vector3 m_vEndPos = new Vector3(-8.7f, 3.0f, 0);
    
    // 이동 거리 및 시간
    float m_fDist;
    const float m_fTime = 60.0f;

    public GameObject[] g_goBackImage;

    Dictionary<int, CImageInfo> m_tmBackImage = new Dictionary<int, CImageInfo>();


	// Use this for initialization
	void Start () {
        m_fDist = Vector3.Distance(m_vEndPos, m_vStartPos);
        Vector3 vCreateStartPos = m_vStartPos;
        
        m_tmBackImage.Clear();
        for(int i=0; i<g_goBackImage.Length; ++i)
        {
            CImageInfo kImageInfo = new CImageInfo();
            kImageInfo.fStartTime = Time.fixedTime;
            kImageInfo.vStartPos = vCreateStartPos;
            kImageInfo.fContinueTime = m_fTime * Vector3.Distance(m_vEndPos, kImageInfo.vStartPos) / m_fDist;
            kImageInfo.goBackImage = GameObject.Instantiate(g_goBackImage[Random.Range(0, g_goBackImage.Length)]);
            vCreateStartPos.x += (m_vStartPos.x - m_vEndPos.x);

            m_tmBackImage.Add(i, kImageInfo);
        }
	}
	
	// Update is called once per frame
	void Update () {
        foreach (KeyValuePair<int, CImageInfo> kvp in m_tmBackImage)
        {
            if (Time.fixedTime - kvp.Value.fStartTime <= kvp.Value.fContinueTime && 0 < kvp.Value.fContinueTime)
            {
                float fRate = (Time.fixedTime - kvp.Value.fStartTime) / kvp.Value.fContinueTime;
                kvp.Value.goBackImage.transform.position = Vector3.Lerp(kvp.Value.vStartPos, m_vEndPos, fRate);
            }
            else
            {
                kvp.Value.fStartTime = Time.fixedTime;
                kvp.Value.vStartPos = m_vStartPos;
                kvp.Value.vStartPos.x += (m_vStartPos.x - m_vEndPos.x);
                kvp.Value.fContinueTime = m_fTime * Vector3.Distance(m_vEndPos, kvp.Value.vStartPos) / m_fDist;

            }
        }

	}
}
