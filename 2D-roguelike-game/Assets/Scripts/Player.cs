using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    
    public float moveSpeed;
    public Rigidbody2D rb;
    private Vector2 moveDirection;

    public int maxHealth = 100;
    public int currentHealth = 100;
    public Healthbar healthbar;
    private int damage = 10;
    private int heal = 10;
    float elapsed = 0f;

    //this is used to store a refernce to our animator controller
    private Animator animator;

    void Start()
    {
        currentHealth = maxHealth;
        healthbar.SetMaxHealth(maxHealth);

        //retrieves the reference to our animator component
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInputs();
        Die();
    }
    
    void FixedUpdate(){
        Move();
    }
    
    void ProcessInputs(){
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        
        moveDirection = new Vector2(moveX, moveY).normalized;

        if(Input.GetKeyDown(KeyCode.Space)){
            GiveHealth(heal);
        }
        // else if (Input.GetKeyDown(KeyCode.H))
        // {
        //     Die();
        // }
    }
    
    void Move(){
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);

        //Sets our animator to show "running"
        animator.SetBool("playerRunning", true);
    }

    void TakeDamage(int damage){
        if(currentHealth > 0){
            currentHealth -= damage;
            healthbar.SetHealth(currentHealth);

            //Sets our animator to show "damage taken"
            animator.SetTrigger("playerHurt");
        }
    }

    void GiveHealth(int heal){
        if(currentHealth < 100){
            currentHealth += heal;
            healthbar.SetHealth(currentHealth);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag.Equals("Enemy")){
            TakeDamage(damage);
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        elapsed += Time.deltaTime;
        if (elapsed >= 0.5f) {
            elapsed = elapsed % 0.5f;
            if(other.gameObject.tag.Equals("Enemy")){
                TakeDamage(damage);
            }
        }  
    }

    void Die()
    {
        if(currentHealth <= 0){
            //Destroy(gameObject);
            Destroy(gameObject);
        
            //Sets our animator to show "our death"
            animator.SetTrigger("playerDie");
        }
    }
}
