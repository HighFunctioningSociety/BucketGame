using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpScript : MonoBehaviour
{
    private EnemyContainer enemy;
    //private Animator animator;
    public float verticalForce;
    public float horizontalForce;
    public Transform shockWaveLeftSpawn, shockWaveRightSpawn;
    public GameObject shockWavePrefab;
    private bool spawnShockwave = false;
    private Vector3 destination;
    private float noise;
    public Transform[] specificDestination;

    private void Start()
    {
        enemy = GetComponent<EnemyContainer>();
    }

    private void Jump()
    {
        float direction = Mathf.Sign(destination.x - transform.position.x);
        float xDifference = Mathf.Abs(destination.x - transform.position.x) / 10;
        float mass = enemy.RigidBody.mass;

        enemy.RigidBody.AddForce(new Vector2((horizontalForce * direction * xDifference) + noise, verticalForce) * mass, ForceMode2D.Impulse);
    }

    private void ShockWaveSpawn()
    {
        if (spawnShockwave == true)
        {
            spawnShockwave = false;
            float shockwaveDirLeft = -enemy.transform.localScale.x;
            float shockwaveDirRight = enemy.transform.localScale.x;

            GameObject projectileLeft = (GameObject)Instantiate(shockWavePrefab, shockWaveLeftSpawn.position, shockWaveLeftSpawn.rotation);
            GameObject projectileRight = (GameObject)Instantiate(shockWavePrefab, shockWaveRightSpawn.position, shockWaveLeftSpawn.rotation);
            projectileLeft.transform.localScale = new Vector2(-shockwaveDirLeft, projectileLeft.transform.localScale.y);
            projectileRight.transform.localScale = new Vector2(-shockwaveDirRight, projectileRight.transform.localScale.y);

            Rigidbody2D rbLeft = projectileLeft.GetComponent<Rigidbody2D>();
            Rigidbody2D rbRight = projectileRight.GetComponent<Rigidbody2D>();

            rbLeft.AddForce(new Vector2(shockwaveDirLeft * 1500, 0), ForceMode2D.Force);
            rbRight.AddForce(new Vector2(shockwaveDirRight * 1500, 0), ForceMode2D.Force);
        }
    }

    private void GetDestination()
    {
        if (enemy.AbilityManager.abilityToUse.scriptableAbility.aName == "ScampLord_Blast")
        {
            SpecificDestination();
        }
        else if (enemy.AbilityManager.abilityToUse.scriptableAbility.aName == "ScampLord_Jump")
        {
            PlayerDestination();
        }
    }

    private void PlayerDestination()
    {
        spawnShockwave = true;
        noise = Random.Range(-20f, 20f);
        destination = enemy.TargetObject.position;
    }

    private void SpecificDestination()
    {
        noise = 0;
        int index = 0;
        float maxDistance = 0;
        for (int i = 0; i < specificDestination.Length; i++)
        {
            float distance = Mathf.Abs(specificDestination[i].position.x - enemy.transform.position.x);
            if (maxDistance < distance)
            {
                maxDistance = distance;
                index = i;
            }
        }
        Debug.Log(index);
        destination = specificDestination[index].position;
    }

    private void ChangeDirection()
    {
        enemy.Direction = Mathf.Sign(enemy.transform.position.x - enemy.TargetObject.position.x);
    }

    private void ZeroOutDirection()
    {
        enemy.Direction = 0;
    }

    private void ZeroOutSpeed()
    {
        enemy.Speed = 0;
    }
}
