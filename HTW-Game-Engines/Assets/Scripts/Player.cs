using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//inherits from our MovingObject class
public class Player : MovingObject
{   
    //damage inflicted when the wall is hit by an arrow
    public int wallDamage = 1;
    //health powerups - added to player health, when pickedup throughout game
    public int pointsPerFood = 10;
    public int pointsPerSoda = 20;
    
    public float restartLevelDelay = 1f;
    //used as part of our in game UI information
    public Text foodText;

    //game sounds related to player
    public AudioClip moveSound1;
    public AudioClip moveSound2;
    public AudioClip eatSound1;
    public AudioClip eatSound2;
    public AudioClip drinkSound1;
    public AudioClip drinkSound2;
    public AudioClip gameOverSound;

    //this is used to store a refernce to our animator controller
    private Animator animator;
    //stores the players health
    private int food;

    // Start is called before the first frame update
    //this is protected override, because we want it to have different
    //behavior than we use have in the MovingObject class
    protected override void Start()
    {
        //retrieves the reference to our animator component
        animator = GetComponent<Animator>();
        //this is done, so that Player class can store the food score during the level
        //then pass it to game manager as we change levels
        food = GameManager.instance.playerFoodPoints;

        foodText.text = "Food: " + food;
        //starts our MovingObject class
        base.Start();
    }

    private void OnDisable() 
    {
        //stores our food value as we change levels
        GameManager.instance.playerFoodPoints = food;
    }

    // Update is called once per frame
    void Update()
    {
        //checks it's currently players turn from boolean we create in game manager
        if (!GameManager.instance.playersTurn) return;

        //these are used to store the directions, along x-y axis
        int horizontal = 0;
        int vertical = 0;

        //casts our floats to into ints, and stores to our variables just declared
        horizontal = (int) Input.GetAxisRaw("Horizontal");
        vertical = (int) Input.GetAxisRaw("Vertical");

        //this prevents player from moving diagonally
        if (horizontal != 0)
            vertical = 0;
        
        //if we have a none zero value -- we are attempting to move
        if (horizontal != 0 || vertical != 0)
            //we are expecting that the player MAY encounter a wall
            AttemptMove<Wall> (horizontal, vertical);
    }

    //generic parameter for our expected movement
    //every time the player moves, they lose -1 health
    protected override void AttemptMove <T> (int xDir, int yDir)
    {
        food --;
        foodText.text = "Food: " + food;

        base.AttemptMove <T> (xDir, yDir);

        //allows us to reference the result of the line cast in move
        RaycastHit2D hit;
        if (Move (xDir, yDir, out hit))
        {
            SoundManager.instance.RandomizeSfx(moveSound1, moveSound2);
        }
        //since player has lost health by moving, we are checking if 
        //game should end
        CheckIfGameOver();
        //sets the "turn" variable to false, to show that players turn has ended
        GameManager.instance.playersTurn = false;
    }

    //exit, soda, food objects - interaction allows with these objects
    //from the Unity API function, OnTriggerEnter2D
    private void OnTriggerEnter2D (Collider2D other)
    {
        //we essentially are checking are Tags defined in our Unity editor
        if (other.tag == "Exit")
        {
            Invoke ("Restart", restartLevelDelay);
            enabled = false;
        }
        //add food/soda points, if we touch food, and deactivate the item
        else if (other.tag == "Food")
        {
            food += pointsPerFood;
            foodText.text = "+" + pointsPerFood + " Food: " + food;
            SoundManager.instance.RandomizeSfx(eatSound1, eatSound2);
            other.gameObject.SetActive(false);
        }
        else if (other.tag == "Soda")
        {
            food += pointsPerSoda;
            foodText.text = "+" + pointsPerSoda + " Food: " + food;
            SoundManager.instance.RandomizeSfx(drinkSound1, drinkSound2);
            other.gameObject.SetActive(false);
        }
    }

    //here, our intent is for the player to "take an action" if they run into a wall
    //namely, to attack it
    protected override void OnCantMove <T> (T component)
    {
        Wall hitWall = component as Wall;
        //wallDamage is how much damage the wall will take
        hitWall.DamageWall(wallDamage);
        //calls and animates our action "playerchop"
        animator.SetTrigger("PlayerChop");
    }

    private void Restart () 
    {
        //we are gonna reload the last scene, which is main since it is the
        //only scene in the game -- that is regenerated/procedurally
        Application.LoadLevel(Application.loadedLevel);
    }

    //when an enemy attacks a player
    public void LoseFood (int loss)
    {
        //triggers animator effects
        animator.SetTrigger("PlayerHit");
        food -= loss;
        foodText.text = "-" + loss + " Food: " + food;
        //since player has taken damage, we gotta check if they are still alive
        CheckIfGameOver();
    }
    
    //this function checks our food score/health to see if the game is over
    private void CheckIfGameOver()
    {
        if (food <= 0)
        {
            SoundManager.instance.PlaySingle(gameOverSound);
            SoundManager.instance.musicSource.Stop();
            GameManager.instance.GameOver();
        }
    }
}
