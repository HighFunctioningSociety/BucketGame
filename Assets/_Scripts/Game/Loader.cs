using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{
    public enum Scene
    {
        MainMenu,
        SceneZero,
        Loading,
        TestEnvironment,
        Station_1,
        Station_2,
        Station_3,
        Station_4,
        Station_5,
        Station_BossRoom,
        MineEntrance,
        Winnar,
        Town_HubArea,
        Town_Shop,
        ScampLordArena,
        SewerEntrance,
        Mine1,
        Mine2,
        Mine3,
    }

    private static Action onLoaderCallback;

    public static void Load(Scene scene)
    {
        _GameManager.gm.SceneChangeEvent.Invoke();

        onLoaderCallback = () =>
        {
            SceneManager.LoadScene(scene.ToString());
        };

        SceneManager.LoadScene(Scene.Loading.ToString());
    }

    public static void LoaderCallback()
    {
        if (onLoaderCallback != null)
        {
            onLoaderCallback();
            onLoaderCallback = null;
        }
    }
}
