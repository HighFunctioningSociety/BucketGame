using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Constants
{
    public static class Paths
    {
        //public static readonly string settings = Application.streamingAssetsPath + "/Settings.json";
        //public static readonly string playerSettings = Application.streamingAssetsPath + "/Player.json";
    }

    public static class Tags
    {
        //public const string board = "Board";
        //public const string metal = "Metal";
        //public const string net = "Net";
        //ublic const string stick = "Stick";
    }
    public static class Layers
    {
        public static readonly int Player = 8;//LayerMask.NameToLayer("Player");
        public static readonly int Enemy = 9;//LayerMask.NameToLayer("Enemies");
        public static readonly int Obstacles = 10;//LayerMask.NameToLayer("Obstacles");
    }
}