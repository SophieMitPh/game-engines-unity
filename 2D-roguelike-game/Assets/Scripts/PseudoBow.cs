using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//This class is named pseudo bow, because we will not be creating an 
//actual bow on our player, but arrows will still flow from him
//The class will be used for "aiming" with the mouse
public class PseudoBow : MonoBehaviour
{
    //our arrow - we first need to create a GameObject from a sprite!
    //this stores our arrow prefab
    public GameObject arrow;
    //how much force are we gonna apply to our arrow
    public float launchForce;
    //which allows us to control launch point of our arrow
    public Transform shotPoint;

    // Update is called once per frame
    void Update()
    {
        //mouse tracking for our non-existent bow
        //this is our current standing position
        Vector2 bowPosition = transform.position;
        //our mouse position -> since mouse positions are x,y positions, we have to
        //convert it to the world position
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //this is the direction (we are aiming)
        Vector2 direction = mousePosition - bowPosition;
        //
        transform.right = direction;

        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }

        void Shoot() 
        {
            GameObject newArrow = Instantiate(arrow, shotPoint.position, shotPoint.rotation);
            newArrow.GetComponent<Rigidbody2D>().velocity = transform.right * launchForce;

        }
    }
}
