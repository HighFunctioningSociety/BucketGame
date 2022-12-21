 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapController : MonoBehaviour
{
    //TODO: Create a room class that contains a PNG, Room ID, and Potentially XYZ Coordinate
    // Find a good way for the area loader and this script to coexist
    public GameObject MapUI;
    public GameObject MapParentObject;
    public Vector3 OriginalPosition;
    public Vector3 OriginalScale;
    public static bool MapActive;
    private int _inputAmplifier = 10;

    // Scales used to determine how small and large the map can become
    private readonly float _maxScale = 2.5f;
    private readonly float _minScale = 0.65f;

    // Offsets used to decide how far the map can travel
    private readonly float _maxXOffset = 1000f;
    private readonly float _minXOffset = -1000f;
    private readonly float _maxYOffset = 600f;
    private readonly float _minYOffset = -600f;

    private void Start()
    {
        Inputs._inputs.OnMapCallback += ToggleMap;
        MapUI.SetActive(false);
        OriginalPosition = MapParentObject.transform.position;
        OriginalScale = MapParentObject.transform.localScale;
    }

    private void Update()
    {
        if (MapActive)
        {
            MapUpdateLoop();
        }
    }

    private void MapUpdateLoop()
    {
        Vector3 mapPosition = MapParentObject.transform.localPosition;
        Vector3 mapScale = MapParentObject.transform.localScale;

        float scaler = 1;
        if (Inputs.leftTrigger && !Inputs.rightTrigger)
            scaler = 0.95f;
        else if (!Inputs.leftTrigger && Inputs.rightTrigger)
            scaler = 1.05f;

        MapParentObject.transform.localPosition = new Vector3(Mathf.Clamp(Inputs.Horizontal * _inputAmplifier + mapPosition.x, _minXOffset, _maxXOffset),
                                                         Mathf.Clamp(Inputs.Vertical * _inputAmplifier + mapPosition.y, _minYOffset, _maxYOffset),
                                                         0);

        MapParentObject.transform.localScale = new Vector3(Mathf.Clamp(mapScale.x * scaler, _minScale, _maxScale), 
                                                           Mathf.Clamp(mapScale.y * scaler, _minScale, _maxScale), 
                                                           Mathf.Clamp(mapScale.z * scaler, _minScale, _maxScale));
    }

    private void ToggleMap()
    {
        if (!PauseMenu.GamePaused)
        {
            MapActive = !MapActive;
            _GameManager.RelinquishPlayerInput(MapActive);
            _GameManager.PlayerContainer.rb.velocity *= new Vector3(0, 1, 1);
            Time.timeScale = MapActive ? 0 : 1;
            MapUI.SetActive(MapActive && !_GameManager.FirstUpdate);
        }
    }

    [System.Serializable]
    public class MineMapTiles
    {
        public GameObject MineMap;
        public GameObject MineRoom_0;
        public GameObject MineRoom_1;
        public GameObject MineRoom_2;
        public GameObject MineRoom_3;
        public GameObject MineRoom_4;
        public GameObject MineRoom_5;
        public GameObject MineRoom_6;
        public GameObject ScampLordArena;
    }
    public MineMapTiles MineMap;

    public void UpdateMap(SceneDirectory.Scene scene)
    {
        switch (scene)
        {
            // City Locations
            case SceneDirectory.Scene.Town_HubArea:
                break;

            // Mine Locations
            case SceneDirectory.Scene.Mine_0:
                _GameManager.gm.GameData.Mine_0_Discovered = true;
                MineMap.MineRoom_0.SetActive(true);
                break;
            case SceneDirectory.Scene.Mine_1:
                _GameManager.gm.GameData.Mine_1_Discovered = true;
                MineMap.MineRoom_1.SetActive(true);
                break;
            case SceneDirectory.Scene.Mine_2:
                _GameManager.gm.GameData.Mine_2_Discovered = true;
                MineMap.MineRoom_2.SetActive(true);
                break;
            case SceneDirectory.Scene.Mine_3:
                _GameManager.gm.GameData.Mine_3_Discovered = true;
                MineMap.MineRoom_3.SetActive(true);
                break;
            case SceneDirectory.Scene.ScampLordArena:
                _GameManager.gm.GameData.ScampLordArena_Discovered = true;
                MineMap.ScampLordArena.SetActive(true);
                break;

            // Sewer Locations
            case SceneDirectory.Scene.SewerEntrance:
                break;
        }
    }

    public void LoadMapData(GameState gameData)
    {
        MineMap.MineRoom_0.SetActive(gameData.Mine_0_Discovered);
        MineMap.MineRoom_1.SetActive(gameData.Mine_1_Discovered);
        MineMap.MineRoom_2.SetActive(gameData.Mine_2_Discovered);
        MineMap.MineRoom_3.SetActive(gameData.Mine_3_Discovered);
        MineMap.MineRoom_4.SetActive(gameData.ScampLordArena_Discovered);
        MineMap.MineRoom_5.SetActive(gameData.Mine_4_Discovered);
        MineMap.ScampLordArena.SetActive(gameData.ScampLordArena_Discovered);
    }
}
