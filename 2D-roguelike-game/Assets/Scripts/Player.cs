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

    void Start()
    {
        currentHealth = maxHealth;
        healthbar.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInputs();
        Die(); // TODO: OnTrigger methode? wenn: currentHealth = 0
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
    }

    void TakeDamage(int damage){
        if(currentHealth > 0){
            currentHealth -= damage;
            healthbar.SetHealth(currentHealth);
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
            // TODO: restart button.isClicked => SceneManager.LoadScene(0);
        }
    }
}
