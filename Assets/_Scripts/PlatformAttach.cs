using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformAttach : MonoBehaviour
{
    public GameObject parentObject;
    private void OnTriggerEnter2D(Collider2D _colInfo)
    {
        if (_colInfo.gameObject.GetComponent<PlayerContainer>() != null)
        {
            PlayerContainer _player = _colInfo.gameObject.GetComponent<PlayerContainer>();
            _player.rb.interpolation = RigidbodyInterpolation2D.None;
            _player.transform.parent = parentObject.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D _colInfo)
    {
        if (_colInfo.gameObject.GetComponent<PlayerContainer>() != null)
        {
            PlayerContainer _player = _colInfo.gameObject.GetComponent<PlayerContainer>();
            _player.transform.parent = null;
            _player.rb.interpolation = RigidbodyInterpolation2D.Interpolate;
            DontDestroyOnLoad(_player);
        }
    }
}
