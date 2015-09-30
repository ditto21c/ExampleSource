using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CMoveHavior
{
    public Vector3 vStartPos = new Vector3();
    public float fStartTime;
    public float fContinueTime;
};

public class CTile : CMoveHavior
{
    public GameObject kTile;
    
};

public class Tile : MonoBehaviour {
    public GameObject g_Tile;

    Vector3 m_vTileStartPos = new Vector3(4.9f, 1.0f, 0);
    Vector3 m_vTileEndPos = new Vector3(-4.1f, 1.0f, 0);

    //GameObject[] m_agoTile = new GameObject[8];
    //CTile[] m_akTile;// = new CTile[8];
    Dictionary<int, CTile> m_tmTile = new Dictionary<int, CTile>();

    // 이동 거리 및 시간
    float m_fDist;
    const float m_fTime = 10.0f;
    int m_nCount = 9;

    void Awake()
    {
        
    }
    // Use this for initialization
    void Start()
    {
        //m_akTile = new CTile[8];
        //m_akTile = new StTile[8];
        m_fDist = Vector3.Distance(m_vTileEndPos, m_vTileStartPos);
        // 타일별 이동 정보 세팅
        
        Vector3 vCreateStartPos = m_vTileEndPos;
        vCreateStartPos.x += 1.0f;
        for (int i = 0; i < m_nCount; ++i)
        {
            CTile kTime = new CTile();

            kTime.kTile = GameObject.Instantiate(g_Tile);
            kTime.kTile.transform.position = vCreateStartPos;
            //m_akTile[i].kTile = Instantiate(g_Tile);
            //m_akTile[i].kTile.transform.position = vCreateStartPos;
            kTime.vStartPos = vCreateStartPos;
            kTime.fContinueTime = m_fTime * Vector3.Distance(m_vTileEndPos, vCreateStartPos) / m_fDist;
            vCreateStartPos.x += 1.0f;

            kTime.fStartTime = Time.fixedTime;

            m_tmTile.Add(i, kTime);
        }

        for (int i = 0; i < 8; ++i)
        {
            ////m_akTile[i].fStartTime = Time.fixedTime;
        }
	}
	
	// Update is called once per frame
	void Update () {
        updateMove();
	}

    void updateMove()
    {
        for (int i = 0; i < m_nCount; ++i)
        {
            CTile kTile;
            if (m_tmTile.TryGetValue(i, out kTile))
            {
                float fRate = (Time.fixedTime - kTile.fStartTime) / kTile.fContinueTime;
                if (1.0f < fRate)
                {
                    kTile.kTile.transform.position = m_vTileStartPos;
                    //m_akTile[i].kTile.transform.position = m_vTileStartPos;
                    kTile.vStartPos = m_vTileStartPos;
                    kTile.fContinueTime = m_fTime * Vector3.Distance(m_vTileEndPos, m_vTileStartPos) / m_fDist;
                    kTile.fStartTime = Time.fixedTime;
                }
                else
                {
                    kTile.kTile.transform.position = Vector3.Lerp(kTile.vStartPos, m_vTileEndPos, fRate);
                    //m_akTile[i].kTile.transform.position = Vector3.Lerp(m_akTile[i].vStartPos, m_vTileEndPos, fRate);
                }
            }
        }
    }
}
