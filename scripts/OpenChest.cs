using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class OpenChest : MonoBehaviour
{
    public float power_time = 15f;
    public Camera cam;
    public int enemy_kill_count_needed = 0;
    public TMPro.TextMeshProUGUI text;
    public TMPro.TextMeshProUGUI power_timer;



    private bool opened_chest = false;
    private float second_between_color_change = 0.2f;
    private bool is_on_power = false;
    void Start()
    {
        
    }


    void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (transform.GetComponentInParent<PoweManager>().has_power)
            {
                text.SetText("You already have a super power !");
                return;
            }

            int enemy_killed_count = collision.transform.GetComponent<MovePlayer>().enemy_killed_count;
            if (enemy_killed_count >= enemy_kill_count_needed) {
                if (opened_chest) return;
                is_on_power = true;
                transform.GetComponentInParent<PoweManager>().has_power = true;

                StartCoroutine(OpenChestAnimation());
                StartCoroutine(RemoveAnimation());
                StartCoroutine(MorphPlayer(collision.transform));
                text.SetText("");
            }
            else
            {
                text.SetText("You need  to kill "+ (enemy_kill_count_needed - enemy_killed_count).ToString() + " more enemies in order to open this chest");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        text.SetText("");
    }

    private IEnumerator OpenChestAnimation()
    {
            transform.Find("chest1").gameObject.SetActive(false);
            transform.Find("chest2").gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            transform.Find("chest2").gameObject.SetActive(false);
            transform.Find("chest3").gameObject.SetActive(true);
            opened_chest = true;

    }


    private IEnumerator RemoveAnimation()
    {
        yield return new WaitForSeconds(0.5f + 5 * 0.01f * 2);
        for (float i = power_time; i > 0; i--)
        {
            power_timer.SetText(i.ToString());
            yield return new WaitForSeconds(1);
        }

        is_on_power = false;
    }

    private IEnumerator MorphPlayer(Transform player)
    {
       SpriteRenderer playerSprite = player.GetComponent<SpriteRenderer>();
       playerSprite.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
       yield return new WaitForSeconds(0.5f);

       float Xscale = player.localScale.x;
       float Yscale = player.localScale.y;
       for (int i = 0;i < 5; i++)
       {
            cam.orthographicSize += 1;
            player.localScale = new Vector3(Xscale + i /3, Yscale + i / 3, 0);
            yield return new WaitForSeconds(0.01f);
       }
       player.GetComponent<MovePlayer>().char_speed = player.GetComponent<MovePlayer>().char_speed + 1000;
       playerSprite.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        while (is_on_power)
       {
            playerSprite.color = new Color(0 , 93 , 255);
            yield return new WaitForSeconds(second_between_color_change);
            playerSprite.color = new Color(0 , 255 ,200);
            yield return new WaitForSeconds(second_between_color_change);
            playerSprite.color = new Color(0 ,255 ,60);
            yield return new WaitForSeconds(second_between_color_change);
            playerSprite.color = new Color(251 , 255 , 0); 
            yield return new WaitForSeconds(second_between_color_change);
            playerSprite.color = new Color(255 , 80 , 0);
            yield return new WaitForSeconds(second_between_color_change);
            playerSprite.color = new Color(255, 0, 220);
            yield return new WaitForSeconds(second_between_color_change);

        }
        power_timer.SetText("");
        for (int i = 0; i < 5; i++)
        {
            cam.orthographicSize -= 1;
            yield return new WaitForSeconds(0.01f);
        }
        playerSprite.color = new Color(255, 255, 255);
        player.GetComponent<MovePlayer>().char_speed = player.GetComponent<MovePlayer>().char_speed - 1000;
        player.localScale = new Vector3(Xscale , Yscale , 0);
        transform.GetComponentInParent<PoweManager>().has_power = false;
       
    }



}
