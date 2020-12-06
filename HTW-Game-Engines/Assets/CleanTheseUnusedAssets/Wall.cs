using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{   
    //this is the sprite we'll display if hit the wall
    public Sprite dmgSprite;

    //health of the wall
    public int hp = 4;
    public AudioClip chopSound1;
    public AudioClip chopSound2;

    private SpriteRenderer spriteRenderer;

    //gets a component reference to our Sprite Renderer
    void Awake ()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    //takes an integer parameter, named loss
    //this gives feedback that the player has hit the wall
    public void DamageWall (int loss)
    {
        SoundManager.instance.RandomizeSfx(chopSound1, chopSound2);
        spriteRenderer.sprite = dmgSprite;
        hp -= loss;
        //disabled the game object if it's "dead"
        if (hp <= 0)
            gameObject.SetActive(false);
    }
}
