using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShieldBarController : MonoBehaviour
{
    public Image[] shieldImgs;
    public int shieldHealth;

    // Update is called once per frame
    void Update()
    {
        switch(shieldHealth)
        {
            case 2:
                foreach(Image img in shieldImgs)
                {
                    img.gameObject.SetActive(true);
                }
            break;
            case 1:
                    shieldImgs[0].gameObject.SetActive(true);
                    shieldImgs[1].gameObject.SetActive(false);
            break;
            case 0:
                shieldImgs[0].gameObject.SetActive(false);
                shieldImgs[1].gameObject.SetActive(false);

                Debug.Log("Shield down");
            break;
        }
    }
}
