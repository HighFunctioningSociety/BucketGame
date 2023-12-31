using UnityEngine;

public class EnemySwoopTriggerable : EnemyProximityTriggerable
{
    [HideInInspector] public EnemyContainer enemy;
    [HideInInspector] public Animator animator;
    public bool IsSwooping;

    private void Awake()
    {
        animator = GetComponentInParent<Animator>();
        enemy = GetComponentInParent<EnemyContainer>();
    }

    private void Start()
    {
        enemyAbilityClone = Instantiate(scriptableAbility);
        Initialize(enemyAbilityClone, gameObject);
    }

    public void Update()
    {
        if (IsSwooping)
            Swoop();
    }

    public override void Initialize(EnemyAbility selectedAbility, GameObject abilityObject)
    {
        selectedAbility.Initialize(abilityObject);
    }
    
    public override void Trigger()
    {
        enemy.AbilityManager.SetCooldown(enemyAbilityClone.globalCooldown);
        StartSwoop();
    }

    public void StartSwoop()
    {
        Vector3 enemyPosition = enemy.transform.position;
        Vector3 targetObjectPosition = enemy.TargetObject.position;
        
        enemy.StartingPosition = enemyPosition;
        enemy.TargetPosition = new Vector2(targetObjectPosition.x, targetObjectPosition.y);
            
        enemy.Direction = Mathf.Sign(enemyPosition.x - targetObjectPosition.x);
        enemy.Speed = Mathf.Abs(enemy.Direction);
        
        enemy.RigidBody.velocity = Vector2.zero;
        
        animator.Play(AnimationName);
    }

    public void Swoop()
    {
        
    }
}
