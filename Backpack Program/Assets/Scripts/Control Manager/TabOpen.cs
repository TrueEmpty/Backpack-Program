using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TabOpen : MonoBehaviour
{
    public Button tab;
    public GameObject dropdown;
    public float offTimer = .25f;

    // Start is called before the first frame update
    void Start()
    {
        dropdown.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(dropdown.activeInHierarchy && !Selected())
        {
            Invoke("Deactivate", offTimer);
        }
    }

    public void OpenTab()
    {
        dropdown.SetActive(true);
    }

    void Deactivate()
    {
        dropdown.SetActive(false);
    }

    bool Selected()
    {
        bool result = false;

        EventSystem eS = EventSystem.current;

        if (eS != null)
        {
            if (eS.currentSelectedGameObject == gameObject)
            {
                result = true;
            }
        }

        return result;
    }
}
