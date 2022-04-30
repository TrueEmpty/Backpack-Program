using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapY : MonoBehaviour
{
    RectTransform rt;
    public float y = -31;

    public float addHeight = 60;

    // Start is called before the first frame update
    void Start()
    {
        rt = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rt != null)
        {
            int cC = transform.childCount;
            float height = cC * addHeight;

            rt.sizeDelta = new Vector2(rt.sizeDelta.x, height);

            if (rt.anchoredPosition.y < y)
            {
                rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, y);
            }
            else if (rt.anchoredPosition.y > height-addHeight)
            {
                rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, height - addHeight);
            }

        }
    }
}
