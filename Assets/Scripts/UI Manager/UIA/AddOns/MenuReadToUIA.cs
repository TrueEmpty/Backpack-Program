using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UIAnimate))]
public class MenuReadToUIA : MonoBehaviour
{
    public MenuControl mC;
    UIAnimate uia;
    [SerializeField]
    public List<string> delayTriggers = new List<string>();
    public float delay = 0;
    public bool nonOverlay = false;
    string oldtile = "xxx!@xx";

    // Start is called before the first frame update
    void Start()
    {
        uia = gameObject.GetComponent<UIAnimate>();

        if (mC == null)
        {
            mC = GameObject.FindGameObjectWithTag("Menu").GetComponent<MenuControl>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (delayTriggers.Count > 0)
        {
            if (delayTriggers.Contains(mC.GetOpenMenu(nonOverlay).title))
            {
                Invoke("SetItem", delay);
                oldtile = mC.GetOpenMenu(nonOverlay).title;
            }
            else if (oldtile != mC.GetOpenMenu(nonOverlay).title)
            {
                Invoke("SetItem", 0);
                oldtile = mC.GetOpenMenu(nonOverlay).title;
            }
        }
        else
        {
            Invoke("SetItem", 0);
            oldtile = mC.GetOpenMenu(nonOverlay).title;
        }
    }

    void SetItem()
    {
        uia.inputKey = mC.GetOpenMenu(nonOverlay).title;
    }
}
