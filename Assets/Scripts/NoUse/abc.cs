using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class abc : MonoBehaviour
{
    // Start is called before the first frame update
    public enum PlayerState { Normal,WallJump,Drawing,Flying}
    public PlayerState currentState;
    float timer = 0;
    public bool isWallHump;
    HingeJoint HJ;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch(currentState)
        {
            case PlayerState.Normal:

                
                if (isWallHump)
                {
                    currentState = PlayerState.WallJump;
                    Debug.LogWarning("牆跳");
                    
                }

                break;
            case PlayerState.WallJump:

                timer += Time.deltaTime;
                Debug.Log("計算時間" + (int)timer);
                if(timer>3.0f)
                {
                    currentState = PlayerState.Normal;
                    timer = 0;
                    isWallHump = false;
                }

                break;
            case PlayerState.Drawing:

                break;
            case PlayerState.Flying:

                break;
        }
        HingeJoint newHing = new HingeJoint();
        newHing.limits = HJ.limits;
        HJ = new HingeJoint(); 
    }
}
