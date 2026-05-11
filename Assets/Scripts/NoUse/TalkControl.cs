using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TalkControl : MonoBehaviour
{
    public bool talkToArtist, talkToProgrammer;
    public string myText;
    private void OnTriggerEnter(Collider other)
    {
        if (talkToArtist == true)
        {
            if (other.gameObject.tag == "Player")
            {
                var TalkBox = other.GetComponent<PlayerController>().TalkBox;
                TalkBox.SetActive(true);
                var text = TalkBox.GetComponentInChildren<Text>();
                text.text = myText;
                text.text = myText.Replace("\\n", "\n");
            }
        }
        if (talkToProgrammer == true)
        {
            if (other.gameObject.tag == "Player2")
            {
                var TalkBox = other.GetComponent<PlayerController2>().TalkBox;
                TalkBox.SetActive(true);
                var text = TalkBox.GetComponentInChildren<Text>();
                text.text = myText;
                text.text = myText.Replace("\\n", "\n");
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (talkToArtist == true)
        {
            if (other.gameObject.tag == "Player")
            {
                var TalkBox = other.GetComponent<PlayerController>().TalkBox;
                TalkBox.SetActive(false);
                var text = TalkBox.GetComponentInChildren<Text>();
                text.text = null;
            }
        }
        if (talkToProgrammer == true)
        {
            if (other.gameObject.tag == "Player2")
            {
                var TalkBox = other.GetComponent<PlayerController2>().TalkBox;
                TalkBox.SetActive(false);
                var text = TalkBox.GetComponentInChildren<Text>();
                text.text = null;
            }
        }
    }
}
