using UnityEngine;
using System.Collections;
public class PlayerHealth : MonoBehaviour
{
    public int max_health = 100;
    public HealthBar health_bar;
    public bool is_invincible;
    public float invincible_time = 1;

    public int curent_health;
    // Start is called before the first frame update
    void Start()
    {
        curent_health= max_health;
        health_bar.setHealth(max_health);
    }

    public void setPlayerHealth(int health)
    {
        curent_health = health;
        health_bar.setHealth(health);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator removeInvicibilty()
    {
        yield return new WaitForSeconds(invincible_time);
        is_invincible = false;
    }

    private IEnumerator invincibleAnimation()
    {
        SpriteRenderer sprite_renderer = transform.GetComponent<SpriteRenderer>();
        while (is_invincible) { 
            sprite_renderer.color = new Color(255 , 255 , 255 , 0.25f);
            yield return new WaitForSeconds(0.2f);
            sprite_renderer.color = new Color(255, 255, 255 , 1);
            yield return new WaitForSeconds(0.2f);
        }
    }

    public bool TakeDamage(int damage)
    {
        if (transform.GetComponent<MovePlayer>().is_dead) return true;
        if (!is_invincible)
        {
            curent_health -= damage;
            health_bar.setHealth(curent_health);
            if (curent_health <= 0) {
                StartCoroutine(transform.GetComponent<MovePlayer>().killAnimation());
                return true;
            }
            is_invincible = true;
            StartCoroutine(invincibleAnimation());
            StartCoroutine(removeInvicibilty());
            return false;
        }
        return false;
    }
}
