using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpiritUI : MonoBehaviour
{
    [SerializeField] private RectTransform[] meters;
    [SerializeField] private Image[] images;
    [SerializeField] private Image[] animations;

    // Start is called before the first frame update
    void Start()
    {
        if (meters == null)
        {
            Debug.LogError("Meter UI not properly assembled");
        }
    }

    public void SetProgression(float _curProgress, float _maxProgress, int _curSpirit, int _maxSpirit)
    {
        float _progress = _curProgress / _maxProgress;

        if (_curSpirit < _maxSpirit)
        {
            images[_curSpirit].color = new Color(1, 1, 1, _progress / 2);
        }
    }


    public void SetSpirit(int _curSpirit, int _maxSpirit)
    {
        for (int i = 0; i < _maxSpirit; i++)
        {
            if (i < _curSpirit)
            {
                images[i].color = new Color(1, 1, 1, 1);
            }
            else if (i > _curSpirit)
            {
                images[i].color = new Color (1, 1, 1, 0);
            }
        }
    }


    public void SetSpiritAnimation(int _curSpirit)
    {
        if (_curSpirit == 0)
            DeactivateSpiritAnimations();

        if (_curSpirit == 1)
        {
            animations[0].enabled = true;
            animations[1].enabled = false;
            animations[2].enabled = false;
        }

        if (_curSpirit == 2)
        {
            animations[0].enabled = false;
            animations[1].enabled = true;
            animations[2].enabled = false;
        }

        if (_curSpirit == 3)
        {
            animations[0].enabled = false;
            animations[1].enabled = false;
            animations[2].enabled = true;
        }
    }

    private void DeactivateSpiritAnimations()
    {
        foreach (Image animation in animations)
        {
            animation.enabled = false;
        }
    }
}
