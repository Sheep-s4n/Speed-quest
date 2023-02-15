using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WeakSpot : MonoBehaviour
{
    public GameObject baby_snakes;
    public Canvas canvas;
    public bool is_taking_damage;
    public TMPro.TextMeshProUGUI textMeshPro;
    public SpriteRenderer hearth;
    public int boss_life = 5;

    public float touch = 0;
    private float prev_time;
    [SerializeField]
    private SpriteRenderer sprite;
    private Color blue = new Color(0, 255, 237);
    private Color red = new Color(255, 0, 0);
    private byte alpha = 0;


    void Start()
    {
        baby_snakes.SetActive(false);
 
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && prev_time + 1 < Time.time)
        {
            if (collision.transform.GetComponent<MovePlayer>().is_dead) return;
            touch++;
            prev_time = Time.time;
            StartCoroutine(damageFlash());
            is_taking_damage= true;
            textMeshPro.SetText((boss_life - touch).ToString());

            if (touch >= boss_life)
            {
                transform.parent.gameObject.GetComponent<MoveSnake>().damage = 0;
                transform.parent.gameObject.GetComponent<MoveSnake>().enabled = false;
                collision.transform.GetComponent<MovePlayer>().enemy_killed_count++;
                StartCoroutine(killAnimation());
            }
        }


    }

    void Update()
    {    
        if (prev_time + 1 < Time.time && is_taking_damage)
        {
            is_taking_damage= false;
        }
    }

    public IEnumerator damageFlash()
    {
        sprite.color = red;
        yield return new WaitForSeconds(0.3f);
        sprite.color = blue;
    }

    public IEnumerator killAnimation()
    {
        textMeshPro.SetText("X-X");
        hearth.color = new Color(255, 255, 255, 0);
        for (int i = 0; i < 3; i++)
        {
            sprite.color = red;
            yield return new WaitForSeconds(0.2f);
            sprite.color = blue;
            yield return new WaitForSeconds(0.2f);
        }
        textMeshPro.SetText("");
        transform.parent.gameObject.GetComponent<SpriteRenderer>().enabled= false;
        while (alpha < 255)
        {
            canvas.GetComponentInChildren<Image>().color = new Color32(0, 0, 0, alpha);
            yield return new WaitForSeconds(0.003f);
            alpha += 1;
        }
        baby_snakes.SetActive(true);
        while (alpha != 0)
        {
            canvas.GetComponentInChildren<Image>().color = new Color32(0, 0, 0, alpha);
            yield return new WaitForSeconds(0.003f);
            alpha -= 1;
        }
        Destroy(transform.parent.parent.gameObject);
    }
}
