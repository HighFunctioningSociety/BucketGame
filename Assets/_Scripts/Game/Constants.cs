using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Constants
{
    public static class Paths
    {
        public static readonly string SavePath = Application.persistentDataPath + "/gameState.bin";
        //public static readonly string playerSettings = Application.streamingAssetsPath + "/Player.json";
    }

    public static class Tags
    {
        //public const string board = "Board";
        //public const string metal = "Metal";
        //public const string net = "Net";
        //public const string stick = "Stick";
    }
    public static class Layers
    {
        public static readonly int Player = 8;//LayerMask.NameToLayer("Player");
        public static readonly int Enemy = 9;//LayerMask.NameToLayer("Enemies");
        public static readonly int Obstacles = 10;//LayerMask.NameToLayer("Obstacles");
    }

    public static class Values
    {
        public static readonly int DefaultPlayerHealth = 5;
        public static readonly int DefaultMaxMeter = 3;
    }

    public static class Scenes
    {
        public static SceneDirectory.Scene OnErrorScene = SceneDirectory.Scene.Town_HubArea;
    }
}