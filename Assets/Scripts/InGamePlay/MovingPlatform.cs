using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Tooltip("平臺移動的結束位置")]
    public Vector3 stopPosiiton;
    [Tooltip("平臺移動一次的時間")]
    public float moveTime = 1f;
    [Tooltip("平臺到邊界後的停留時間")]
    public float stayTime = 1f;

    private bool toStop = true;         // 是否朝結束位置移動
    private float speed;                // 移動的速度
    private Vector3 startPostion;       // 開始位置

    internal bool on = true;           // 平臺移動開關，是否允許平臺移動
    void Start()
    {
        startPostion = transform.position;
        speed = Vector3.Distance(transform.position, stopPosiiton) / moveTime;
    }
    void Update()
    {
        PlatformMoveOn(on);
    }

    /// <summary>
    /// 平臺移動控制
    /// </summary>
    /// <param name="on">平臺移動開關</param>
    void PlatformMoveOn(bool on)
    {
        if (!on) { return; }
        StartCoroutine(PlatformMove(stopPosiiton));
    }

    /// <summary>
    /// 具體平臺移動控制
    /// </summary>
    /// <param name="stopPosiiton">停止位置</param>
    /// <returns></returns>
    IEnumerator PlatformMove(Vector3 stopPosiiton)
    {
        Vector3 tempPosition = transform.position;
        if (toStop)
        {
            tempPosition = Vector3.MoveTowards(tempPosition, stopPosiiton, speed * Time.deltaTime);
            transform.position = tempPosition;
            if (transform.position == stopPosiiton)
            {
                yield return new WaitForSeconds(stayTime);
                toStop = false;
            }
        }
        else if (!toStop)
        {
            tempPosition = Vector3.MoveTowards(tempPosition, startPostion, speed * Time.deltaTime);
            transform.position = tempPosition;
            if (transform.position == startPostion)
            {
                yield return new WaitForSeconds(stayTime);
                toStop = true;
            }
        }
    }
}