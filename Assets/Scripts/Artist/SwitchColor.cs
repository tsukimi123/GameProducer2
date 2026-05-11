using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchColor : MonoBehaviour
{
    public ArtistSkill artist;
    public void SwitchToRed()
    {
        artist.penColor = new Color(255, 0, 0, 255);
    }
    public void SwitchToBlue()
    {
        artist.penColor = new Color(0, 0, 255, 255);
    }
    public void SwitchToYellow()
    {
        artist.penColor = new Color(255, 255, 0, 255);
    }
    public void SwitchToGreen()
    {
        artist.penColor = new Color(0, 255, 0, 255);
    }
    public void SwitchToAqua()
    {
        artist.penColor = new Color(0, 255, 255, 255);
    }
    public void SwitchToPink()
    {
        artist.penColor = new Color(255, 0, 2255, 255);
    }
}
