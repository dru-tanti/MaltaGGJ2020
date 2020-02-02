using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    public Image[] healthImgs; // 0-2, left-right
    PlayerController playerHealth;
    private int health;
    // Start is called before the first frame update
    void Start()
    {
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        health = playerHealth.health;

        switch(health)
        {
            case 3:
                foreach(Image img in healthImgs)
                {
                    img.gameObject.SetActive(true);
                }
            break;
            case 2:
                healthImgs[0].gameObject.SetActive(true);
                healthImgs[1].gameObject.SetActive(true);
                healthImgs[2].gameObject.SetActive(false);
            break;
            case 1:
                healthImgs[0].gameObject.SetActive(true);
                healthImgs[1].gameObject.SetActive(false);
                healthImgs[2].gameObject.SetActive(false);
            break;
            case 0:
                healthImgs[0].gameObject.SetActive(false);
                healthImgs[1].gameObject.SetActive(false);
                healthImgs[2].gameObject.SetActive(false);

                Debug.Log("Player dead");
            break;
        }
    }
}
