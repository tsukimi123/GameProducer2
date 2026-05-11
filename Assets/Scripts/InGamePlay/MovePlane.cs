using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlane : MonoBehaviourPun
{
    public string targetName, secondName;
    public bool needAudio;
    public GameObject plane, target;
    public Vector3 planePosition,StartPosition, EndPosition;
    public float speed, stayTime;
    [SerializeField]private bool toEnd = false, isLoop = false;
    public bool Loop, delay, setTrue, needEnd;
    public float a = 0f;
    public AudioSource AS;
    private void Start()
    {
        planePosition = plane.transform.position;
    }
    private void Update()
    {
        if (toEnd == true)
        {
            if (setTrue == true)
            {
                if (needEnd == true)
                {
                    if (planePosition == EndPosition)
                    {
                        target.SetActive(true);
                    }
                }
                else
                    target.SetActive(true);
            }
            if (delay == true)
            {
                a += Time.deltaTime;
                if (a >= stayTime)
                {
                    planePosition = Vector3.MoveTowards(planePosition, EndPosition, speed * Time.deltaTime);
                }
            }
            else if (isLoop == false)
            {
                planePosition = Vector3.MoveTowards(planePosition, EndPosition, speed * Time.deltaTime);
            }
            else if (isLoop == true)
            {
                StartCoroutine(PlatformMove());
            }
        }
        else if (toEnd == false)
        {
            planePosition = Vector3.MoveTowards(planePosition, StartPosition, speed * Time.deltaTime);
        }
        plane.transform.position = planePosition;
        if (needAudio)
        {
            AS.Play();
            needAudio = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        ArtWall artWall = other.GetComponent<ArtWall>();
        if (artWall != null)
        {
            string name = artWall.myName;
            if (name == targetName || name == secondName)
            {
                toEnd = true;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        ArtWall artWall = other.GetComponent<ArtWall>();
        if (artWall != null)
        {
            string name = artWall.myName;
            if (name == targetName || name == secondName)
            {
                toEnd = false;
            }
        }
    }
    IEnumerator PlatformMove()
    {
        if (Loop == true)
        {
            planePosition = Vector3.MoveTowards(planePosition, EndPosition, speed * Time.deltaTime);
            if (planePosition == EndPosition)
            {
                yield return new WaitForSeconds(stayTime);
                Loop = false;
            }
        }
        else if (Loop == false)
        {
            planePosition = Vector3.MoveTowards(planePosition, StartPosition, speed * Time.deltaTime);
            if (planePosition == StartPosition)
            {
                yield return new WaitForSeconds(stayTime);
                Loop = true;
            }
        }
    }
}
