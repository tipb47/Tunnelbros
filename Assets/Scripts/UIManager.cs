using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public Image healthBar;
    public Image shieldBar;

    public static UIManager manager;

    void Awake()
    {
        if (manager == null)
        {
            healthBar.enabled = true;
            shieldBar.enabled = false;
            manager = this;  // singleton
        }
        else
        {
            Destroy(gameObject);
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
