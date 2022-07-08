using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Stats/PlayerStats")]
public class PlayerStats : ScriptableObject
{
    public int defaultHealth = 5;
    public int healthUpgrades = 0;
    public int maxSpirit = 3;
    public float maxSpiritProgression = 100;
    public float spiritDecayRate = 0.9f;
    public float maxSpeed = 140f;
    public float jumpForce = 98f;
    public float defaultGravity = 5;
    public float defaultDrag = 0;
    //public int fallBoundary = -999;
    public float fallMultiplier = 1.3f;
    public float shortHopSubtraction = 12;

    public float stunTime = 0.5f;
    public float invulTime = 1f;
    public float movementSmoothing = 0.05f;
    public float movementDivider = 5;
    private int _maxHealth;
    [SerializeField] private int _curHealth;
    [SerializeField] private int _curSpirit;
    private float _curSpiritProgession;
    private float _curSpeed;
    private float _curMovementDivider; //used during air attacks to make the character float for the first few hits

    public int maxHealth
    {
        get { _maxHealth = defaultHealth + healthUpgrades; return _maxHealth;}
        set { _maxHealth = defaultHealth + healthUpgrades; }
    }

    public int curHealth
    {
        get { return _curHealth; }
        set { _curHealth = Mathf.Clamp(value, 0, maxHealth); }
    }
    public int curSpirit
    {
        get { return _curSpirit; }
        set { _curSpirit = Mathf.Clamp(value, 0, maxSpirit); }
    }
    public float curSpiritProgression
    {
        get { return _curSpiritProgession; }
        set { _curSpiritProgession = Mathf.Clamp(value, 0, maxSpiritProgression); }
    }
    public float curSpeed
    {
        get { return _curSpeed; }
        set { _curSpeed = Mathf.Clamp(value, maxSpeed / 4, maxSpeed); }
    }
    public float curMovementDivider
    {
        get { return _curMovementDivider; }
        set { _curMovementDivider = Mathf.Clamp(value, 1, movementDivider); }
    }

    public void Init()
    {
        curHealth = maxHealth;
        curSpiritProgression = 0f;
        curSpeed = maxSpeed;
        curMovementDivider = movementDivider;
    }
}
