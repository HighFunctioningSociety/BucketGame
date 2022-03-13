using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent (typeof(SpriteRenderer))]
public class Tiling : MonoBehaviour
{
    public int offsetX = 2; // Offset so that we don't get errors

    // Check if we need to instantiate buddies
    public bool hasARightBuddy = false;
    public bool hasAleftBuddy = false;

    public bool reverseScale = false;

    private float spriteWidth = 0f; // Width of out elemenet
    private Camera cam;
    private Transform myTransform; // Storing transform in a variable is faster and good practice

    //where you want to do the referencing between scripts
    private void Awake()
    {
        cam = Camera.main;
        myTransform = transform;
    }
    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer sRenderer = GetComponent<SpriteRenderer>();
        spriteWidth = sRenderer.sprite.bounds.size.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (hasAleftBuddy == false || hasARightBuddy == false)
        {
            // Calculating the cameras extend (half of the width) of what the camera can see in the world coordinates
            float camHorizontalExtend = cam.orthographicSize * Screen.width / Screen.height;

            // calculate the x position where the camera can see the edge of the sprite (element)
            float edgeVisiblePositionRight = (myTransform.position.x + spriteWidth / 2) - camHorizontalExtend;
            float edgeVisiblePositionLeft = (myTransform.position.x - spriteWidth / 2) + camHorizontalExtend;

            //checking if we can see the edge of the element and then calling MakeNewBuddy to create a loop of that edge
            if (cam.transform.position.x >= edgeVisiblePositionRight - offsetX && hasARightBuddy == false)
            {
                MakeNewBuddy(1);
                hasARightBuddy = true;
            }
            else if (cam.transform.position.x <= edgeVisiblePositionLeft + offsetX && hasAleftBuddy == false)
            {
                MakeNewBuddy(-1);
                hasAleftBuddy = true;
            }
        }
    }
    
    // Creates a buddy to loop background or foreground
    void MakeNewBuddy(int rightOrLeft)
    {
        // calculating the new position for our new buddy
        Vector3 newPosition = new Vector3 (myTransform.position.x + spriteWidth * rightOrLeft, myTransform.position.y, myTransform.position.z);
        //instantiating our new buddy and storing him in a variable
        Transform newBuddy = (Transform) Instantiate(myTransform, newPosition, myTransform.rotation);

        //if not tilable let's reverse the x size of the object to get rid of seams
        if (reverseScale == true)
        {
            newBuddy.localScale = new Vector3(newBuddy.localScale.x * -1, newBuddy.localScale.y, newBuddy.localScale.z);
        }

        newBuddy.parent = myTransform.parent;
        if (rightOrLeft > 0)
        {
            newBuddy.GetComponent<Tiling>().hasAleftBuddy = true;
        } else
        {
            newBuddy.GetComponent<Tiling>().hasARightBuddy = true;
        }
    }
}
