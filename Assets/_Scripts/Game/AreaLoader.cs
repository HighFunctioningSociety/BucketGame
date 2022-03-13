using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AreaLoader : MonoBehaviour
{
    public int DoorID;
    public Loader.Scene sceneToLoad;
    public Direction doorDirection;
    public Collider2D loaderCollider;
    public Transform specificSpawn;

    public enum Direction
    {
        RIGHT,
        LEFT,
        UP,
        DOWN,
        CENTER,
    }

    protected abstract void LoadScene(PlayerContainer player);

    public IEnumerator FadeToNextScene(PlayerContainer player)
    {
        BlankerAnimator.blanker.FadeOut();
        while (BlankerAnimator.blanker.transitionInProgress)
        {
            yield return null;
        }
        player.SaveGame();
        Loader.Load(sceneToLoad);
    }

    protected bool VerifyNextScene()
    {
        if (sceneToLoad != Loader.Scene.MainMenu || sceneToLoad != Loader.Scene.SceneZero || sceneToLoad != Loader.Scene.Loading)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    protected void SetScene()
    {
        _GameManager.currentScene = sceneToLoad;
        _GameManager.DoorID = DoorID;
    }
}
