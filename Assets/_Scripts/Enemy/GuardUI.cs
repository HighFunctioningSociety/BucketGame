using UnityEngine.UI;
using UnityEngine;

public class GuardUI : MonoBehaviour
{
    [SerializeField] private RectTransform guardBar;
    [SerializeField] private Text guardText;

    void Start()
    {
        if (guardBar == null)
        {
            Debug.LogError("GUARD UI: No guard bar object referenced!");
        }
        if (guardText == null)
        {
            Debug.LogError("GUARD UI: No guard text object referenced!");
        }
    }

    public void SetGuard(float _cur, float _max)
    {
        float _value = _cur / _max;

        guardBar.localScale = new Vector3(_value, guardBar.localScale.y, guardBar.localScale.z);
        guardText.text = _cur + "/" + _max;
    }
}
