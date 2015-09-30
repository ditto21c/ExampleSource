public class CStateInfo
{
    public int m_nStageIndex = 0;               // 몇번째 스테이지 인지
    public int[] m_anCharCode = new int[4];     // 스테이지 별로 나오는 캐릭터
    public float m_fTermTime = 0.0f;            // 보스 나오기 전 텀 시간(s)
}