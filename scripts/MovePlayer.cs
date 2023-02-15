using System;
using UnityEngine;
using System.Collections;
using UnityEditor.Build;

public class MovePlayer : MonoBehaviour
{
    public Rigidbody2D rb;
    public Transform collision_right;
    public Transform collision_left;
    public float jump_force;
    public Animator anim;
    public SpriteRenderer sprite_renderer;
    public int enemy_killed_count;
    public float char_speed = 20;
    public bool is_dead;

    private Vector3 vel = Vector3.zero;
    private Vector3 origin_pos = Vector3.zero;
    private bool is_jumping;
    private bool is_grounded;

    //   private float prev_time;
    // Start is called before the first frame update
    void Start()
    {
     //   prev_time = Time.time;
        origin_pos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Respawn(false);
        }

        if (collision_left.transform.position.y < origin_pos.y - 5)
        {
          Respawn(false);
        }
           
          is_grounded = Physics2D.OverlapArea(collision_left.position , collision_right.position);

          if (Input.GetButtonDown("Jump") && is_grounded) 
            is_jumping = true;

          if (is_jumping)
          {
          //  if (prev_time + 1 < Time.time)
            //{
            rb.AddForce(new Vector2(0f, jump_force) , ForceMode2D.Impulse);
            is_jumping = false;
               // prev_time = Time.time;
           // }
          }
        
        
    }


    public void Respawn(bool get_back_health=true)
    {   
        transform.position = origin_pos;
        if (!get_back_health) return;
        transform.GetComponent<PlayerHealth>().setPlayerHealth(transform.GetComponent<PlayerHealth>().max_health);
    }

    void PlayerMove(float _horizontal_mov)
    {
        Vector3 new_vel = new Vector2(_horizontal_mov, rb.velocity.y);
        rb.velocity = Vector3.SmoothDamp(rb.velocity, new_vel,ref vel, 0.01f);
        Flip(rb.velocity.x);
        

    }

    void Flip(float _velocity)
    {
        if (_velocity > 0.1f)
        {
            sprite_renderer.flipX = false;
        }
        else if (_velocity < -0.1f)
        {
            sprite_renderer.flipX = true;
        }

    }

    void FixedUpdate()
    {

        float horizontal_mov = Input.GetAxis("Horizontal") * char_speed * Time.deltaTime;
       //Debug.Log(horizontal_mov);
        anim.SetFloat("speed", Math.Abs(rb.velocity.x));
        PlayerMove(horizontal_mov);
    }


    public IEnumerator killAnimation()
    {
        is_dead = true;
        transform.GetComponent<MovePlayer>().enabled = false;
        transform.GetComponent<Animator>().enabled = false;
        SpriteRenderer sprite_renderer = transform.GetComponent<SpriteRenderer>();
        sprite_renderer.color = new Color(255, 0, 0);
        for (int i = 0; i > -90; i--)
        {
            transform.eulerAngles = new Vector3(0, 0, i);
            yield return new WaitForSeconds(0.001f);
        }
        yield return new WaitForSeconds(2);
        transform.eulerAngles = new Vector3(0, 0, 0);
        transform.GetComponent<MovePlayer>().enabled = true;
        transform.GetComponent<Animator>().enabled = true;
        sprite_renderer.color = new Color(255, 255, 255);
        Respawn();
        is_dead = false;
    }


}
