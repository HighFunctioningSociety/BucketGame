using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestController : MonoBehaviour
{
    public GameObject[] ItemContents;
    public int currencyAmount;
    public ScriptableBool alreadyOpened;
    public Collider2D colliderOpen;
    public Collider2D colliderClosed;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("AlreadyOpened", alreadyOpened.state);

        colliderOpen.enabled = alreadyOpened.state;
        colliderClosed.enabled = !alreadyOpened.state;
    }

    public void OpenChest()
    {
        Debug.Log("Im Gay");
        if (!alreadyOpened.state)
        {
            alreadyOpened.state = true;
            _GameManager.SpawnChestContents(this);
            animator.SetTrigger("OpenChest");

            colliderClosed.enabled = false;
            colliderOpen.enabled = true;
        }
    }
}
