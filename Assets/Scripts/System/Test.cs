using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Test : MonoBehaviourPun
{
    [SerializeField] private float AllMass, targetMass;
    public TMP_Text text;
    public GameObject door;
    public Vector3 EndPosition;
    public float speed;
    public AudioSource AS;
    private bool AudioPlay;
    private void Update()
    {
        text.text = AllMass+ " / " + targetMass;
        if (AllMass >= targetMass)
        {
            door.transform.position = Vector3.MoveTowards(door.transform.position, EndPosition, speed * Time.deltaTime);
            if (!AudioPlay)
            {
                AS.Play();
                AudioPlay = true;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Rigidbody RB = other.GetComponent<Rigidbody>();
        if (RB != null)
        {
            AllMass += RB.mass;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Rigidbody RB = other.GetComponent<Rigidbody>();
        if (RB != null)
        {
            AllMass -= RB.mass;
        }
    }
}