using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSetting
{
    private static BaseSetting _instance = null;

    public static BaseSetting GetInstance
    {
        get
        {
            if(_instance == null)
            {
                _instance = new BaseSetting();
            }
            return _instance;
        }
    }

    private BaseSetting() { }

    public string[] AdsorbableTags
    {
        get
        {
            return adsorbableTags;
        }
    }

    private string[] adsorbableTags = new string[] { "adsorbable" };

    public string[] ObstaclesTags
    {
        get
        {
            return obstaclesTags;
        }
    }

    string[] obstaclesTags = new string[] { "obstacle" };

}
