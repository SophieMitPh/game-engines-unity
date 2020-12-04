using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//the abstract class allows us to create incomplete classes
//which will be fulfilled in the derived class
public abstract class MovingObject : MonoBehaviour
{
    //time it takes object to move - in seconds
    public float moveTime = 0.1f;
    //this layer upon which we'll check if we can move into a space
    public LayerMask blockingLayer;

    
    private BoxCollider2D boxCollider;
    private Rigidbody2D rb2D;
    //used for efficient movement calculations
    private float inverseMoveTime;


    // Start is called before the first frame update
    // these can be overridden from their parent classes
    protected virtual void Start()
    {   
        //get our game component references
        boxCollider = GetComponent<BoxCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();
        //storing the reciprocal is more efficient 
        //since we can no multiple it.
        inverseMoveTime = 1f / moveTime;

    }
    //returns booleans, takes two integers - out causes arguments to be passed by reference
    //here we will return to values: boolean and hitray2D
    protected bool Move (int xDir, int yDir, out RaycastHit2D hit)
    {   
        //converting Vector3 -> vector2, since we don't need z-axis
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2 (xDir, yDir);

        //ensures we don't hit our own collider, when casting ray
        boxCollider.enabled = false;
        //casts a line between start -> points, to ensure no collisions
        hit = Physics2D.Linecast (start, end, blockingLayer);
        boxCollider.enabled = true;

        //checking if anything was hit
        if (hit.transform == null)
        {   
            //movement is allows
            StartCoroutine(SmoothMovement (end));
            return true;
        }

        //move was unsuccessful
        return false;
    }
    //this is used for moving units from one space to the next
    //"end" is the next move
    protected IEnumerator SmoothMovement (Vector3 end)
    {
        //sqrMagnitude is computationally more efficient than Magnitude
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

        while (sqrRemainingDistance > float.Epsilon)
        {
            Vector3 newPosition = Vector3.MoveTowards (rb2D.position, end, inverseMoveTime * Time.deltaTime);
            rb2D.MovePosition(newPosition);
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;
            yield return null;
        }
    }

    //Generic parameter T used to specify the type of component if blocked
    //in case of player, this is gonna be walls (not players?)
    //ikn the case of an enemy, this is gonna be a player
    protected virtual void AttemptMove <T> (int xDir, int yDir)
        where T: Component
        {
            //this means the player is hit
            RaycastHit2D hit;
            //set to true, if move is successful
            bool canMove = Move (xDir, yDir, out hit);

            if (hit.transform == null)
                return;
            
            T hitComponent = hit.transform.GetComponent<T>();

            //means our object is blocked, so we are calling our OnCantMove function
            if (!canMove && hitComponent != null)
                OnCantMove(hitComponent);
        }

    protected abstract void OnCantMove <T> (T component)
        where T : Component;
}
