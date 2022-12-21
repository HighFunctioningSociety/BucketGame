using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AreaLoader : MonoBehaviour
{
    public int DoorID;
    public SceneDirectory.Scene sceneToLoad;
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

    protected abstract void LoadScene();

    public IEnumerator FadeToNextScene()
    {
        BlankerAnimator.blanker.FadeOut();
        while (BlankerAnimator.blanker.transitionInProgress)
        {
            yield return null;
        }
        SaveSystem.SaveGame();
        Loader.Load(sceneToLoad);
    }

    protected bool VerifyNextScene()
    {
        if (sceneToLoad != SceneDirectory.Scene.MainMenu || sceneToLoad != SceneDirectory.Scene.SceneZero || sceneToLoad != SceneDirectory.Scene.Loading)
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
        _GameManager.CurrentScene = sceneToLoad;
        _GameManager.DoorID = DoorID;
    }
}
