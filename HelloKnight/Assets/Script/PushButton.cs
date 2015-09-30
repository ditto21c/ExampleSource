using UnityEngine;
using System.Collections;

public class PushButton : MonoBehaviour {
    public int m_nSkillCode;



    float m_fClickEventTime = 0.3f;         // 클릭시 이벤트 연출 시간
    bool m_bEventStart = false;             // 이벤트 중인가?
    float m_fEventStartTime = 0.0f;         // 이벤트 시작 시간
    SpriteRenderer m_kSpriteRenderer;
    Vector3 m_vDefaltScale;
    void Awake()
    {
        m_vDefaltScale = gameObject.transform.localScale;
    }
	// Use this for initialization
	void Start () {
        m_kSpriteRenderer = (SpriteRenderer)gameObject.GetComponent("SpriteRenderer");
	}
	
	// Update is called once per frame
	void Update () {
        updateEventClick();
	}

    // 클릭 이벤트 시작
    public void StartEventClick()
    {
        m_bEventStart = true;
        m_fEventStartTime = Time.fixedTime;
    }


    void updateEventClick()
    {
        if (m_bEventStart)
        {
            if (Time.fixedTime - m_fEventStartTime >= m_fClickEventTime)
            {
                m_bEventStart = false;
                gameObject.transform.localScale = m_vDefaltScale;
                m_kSpriteRenderer.color = new Color(m_kSpriteRenderer.color.r, m_kSpriteRenderer.color.g, m_kSpriteRenderer.color.b, 1.0f);
            }
            else
            {
                // 이미지 크기 변경
                float fT = (Time.fixedTime - m_fEventStartTime)/m_fClickEventTime;
                float fScale = Mathf.Lerp(m_vDefaltScale.x, m_vDefaltScale.x * 1.5f, fT);
                gameObject.transform.localScale = new Vector3(fScale, fScale, fScale);

                // 이미지 알파 변경
                fScale = Mathf.Lerp(1.0f, 0.0f, fT);
                m_kSpriteRenderer.color = new Color(m_kSpriteRenderer.color.r, m_kSpriteRenderer.color.g, m_kSpriteRenderer.color.b, fScale);
            }
        }
    }

    public int GetSkillCode()
    {
        return m_nSkillCode;
    }
}
