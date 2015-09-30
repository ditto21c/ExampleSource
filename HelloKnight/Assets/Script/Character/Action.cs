public class CAction
{
    public eActionIndex nActionState;       // 현재 액션 상태
    public float fActionStartTime;          // 액션 시작 시간
    public float fActionContinueTime;       // 액션 유지 시간

    public string GetTextAction()
    {
        string strText = "액션 : ";

        switch (nActionState)
        {
            case eActionIndex.Attack1:
                strText += "Attack1 ";
                break;
            case eActionIndex.Attack2:
                strText += "Attack2 ";
                break;
            case eActionIndex.Defend:
                strText += "Defend ";
                break;
            case eActionIndex.Die:
                strText += "Die ";
                break;
            case eActionIndex.Hit:
                strText += "Hit ";
                break;
            case eActionIndex.Idle:
                strText += "Idle ";
                break;
            case eActionIndex.Run:
                strText += "Run ";
                break;
        };

        return strText;
    }

};
