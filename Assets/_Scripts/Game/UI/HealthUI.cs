using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public Image[] healthVines;
    public Image[] hearts;
    public Image[] heartLights;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    private readonly float _curHealth;
    private readonly float _maxHealth;

    private void Start()
    {
        if (fullHeart == null)
            Debug.LogError("STATUS INDICATOR: No full heart sprite referenced");
        if (emptyHeart == null)
            Debug.LogError("STATUS INDICATOR: No empty sprite referenced");
        if (hearts == null)
            Debug.LogError("STATUS INDICATOR: No Image referenced");
    }

    public void SetHealth(int _cur, int _max)
    {
        if (_curHealth == _cur && _maxHealth == _max)
        {
            return;
        }

        if (_cur > _max)
        {
            _cur = _max;
        }

        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < _cur)
            {
                hearts[i].sprite = fullHeart;
                heartLights[i].color = new Color (1, 1, 1, 0.25f);
            }
            else
            {
                hearts[i].sprite = emptyHeart;
                heartLights[i].color = new Color(1, 1, 1, 0);
            }

            if (i < _max)
            {
                hearts[i].enabled = true;
                heartLights[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
                heartLights[i].enabled = false;
            }
        }

        SetVineLength(_max);
    }

    private void SetVineLength(int _max)
    {
        switch (_max)
        {
            case 6:
            case 7:
                healthVines[0].enabled = false;
                healthVines[1].enabled = true;
                healthVines[2].enabled = false;
                healthVines[3].enabled = false;
                break;
            case 8:
            case 9:
                healthVines[0].enabled = false;
                healthVines[1].enabled = false;
                healthVines[2].enabled = true;
                healthVines[3].enabled = false;
                break;
            case 10:
                healthVines[0].enabled = false;
                healthVines[1].enabled = false;
                healthVines[2].enabled = false;
                healthVines[3].enabled = true;
                break;
            default:
                healthVines[0].enabled = true;
                healthVines[1].enabled = false;
                healthVines[2].enabled = false;
                healthVines[3].enabled = false;
                break;
        }
    }
}
