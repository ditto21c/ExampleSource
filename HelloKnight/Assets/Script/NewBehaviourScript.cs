using UnityEngine;
using System.Collections;

public enum EEventFrame
{
    None = 0,
    Damage,
    Hit,
};

public class NewBehaviourScript : MonoBehaviour {
    public Sprite[] kSprite;                // 스프라이트 이미지
    public EEventFrame[] eEventFrame;       // 프레임 이벤트
    public int mAllFrame = 0;               // 전체 프레임수      
    public int mPerFrame = 0;               // 1초당 프레임 수
    int mPreFrame = 0;
    public float fStartTime = 0;            // 애니 시작 시간
    public bool bLoopAni = false;           // 애니메이션을 루프 시킬것인가?
    public bool bPlayingAni = false;        // 애니메이션 중인가?
	// Use this for initialization   클래스 상속시 호출 되지 않는다
	void Start () {
        
        
	}
	
	// Update is called once per frame
	void Update () {

       
       
	}
    public void Initilize()
    {
        mAllFrame = kSprite.Length;
        mPerFrame = mAllFrame * 2;
    }

    public void UpdateAni()
    {
        if (bPlayingAni)
        {
            SpriteRenderer k = (SpriteRenderer)gameObject.GetComponent("SpriteRenderer");
            float fTime = Time.time - fStartTime;
            int nFrame = (int)(fTime * (float)mPerFrame);
            if (nFrame < mAllFrame)
            {
                // 프레임 이벤트 중복 체크
                if (nFrame != mPreFrame)
                {
                    mPreFrame = nFrame;
                    FrameEvent(nFrame);
                }
                k.sprite = kSprite[nFrame];
            }
            else
            {
                if (bLoopAni)
                {
                    fStartTime = Time.time;
                }
                else
                {
                    bPlayingAni = false;
                }
                
            }

            
        }
        
    }

    // 몇초동안 프레임이 돌아 갈건지 세팅 
    // 세팅시 AniPlay 전에 세팅 해줘야 된다.
    public void AniFrameTime(float fTime_)
    {
        if (0.0f < fTime_)
        {
            float fAllFrame = mAllFrame;
            fAllFrame = (fAllFrame / fTime_);
            mPerFrame = (int)fAllFrame;
        }
    }

    public void AniPlay()
    {
        bPlayingAni = true;
        fStartTime = Time.time;
    }
    public void AniStop()
    {
        bPlayingAni = false;
    }
    void FrameEvent(int nFrame_)
    {
        if (eEventFrame.Length == 0 || eEventFrame.Length <= nFrame_)
            return;
        switch (eEventFrame[nFrame_])
        {
            case EEventFrame.Damage:
                SendMessage("DamageEventFrame");
                break;
            case EEventFrame.Hit:
                SendMessage("HitEventFrame");
                break;
        }
    }
}
