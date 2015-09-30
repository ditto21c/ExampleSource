using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class InputData
{
    public int nTouchCount = 0;
    public Hashtable tmTouchPosition = new Hashtable();

};
public class ScInput : MonoBehaviour
{
    public GameObject kText;
   
    // Use this for initialization
    void Start()
    {
        
    }


    
    // Update is called once per frame
    void Update()
    {
        // 종료 처리
        if (Input.GetKey(KeyCode.Escape)
            || Input.GetKey(KeyCode.Home)
            || Input.GetKey(KeyCode.Menu))
        {
            //Application.Quit();
            SendMessage("ApplicationQuit");
        }
        
        // 터치
        if (Input.GetMouseButtonDown(0))
        {
            InputData kInputData = new InputData();
            kInputData.nTouchCount = Input.touchCount;
            kInputData.tmTouchPosition.Add(0, Input.mousePosition);
            SendMessage("InputTouch", kInputData, SendMessageOptions.RequireReceiver);
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            SendMessage("KeyDownDefend");
        }



    }

    // 게임 플레이 시작.
    public void GamePlayStart()
    {
    }
    // 게임 플레이 끝.
    public void GamePlayEnd()
    {
    }



}

