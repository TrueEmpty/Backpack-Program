using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowLoad : MonoBehaviour
{
    public GameObject main;
    public GameObject addParentPF;
    public GameObject addSchoolPF;
    public GameObject addPickupPF;

    GameObject addParent;
    GameObject addSchool;
    GameObject addPickup;

    OutputCSV ocsv;

    // Start is called before the first frame update
    void Start()
    {
        ocsv = GameObject.FindGameObjectWithTag("Manager").GetComponent<OutputCSV>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenWindow(string windowName)
    {
        switch (windowName.Trim().ToLower())
        {
            case "addschool":
                if (addSchool == null)
                {
                    addSchool = Instantiate(addSchoolPF, transform);
                }
                else
                {
                    addSchool.GetComponent<Window>().active = true;
                }
                break;
            case "addparent":
                if (addParent == null)
                {
                    addParent = Instantiate(addParentPF, transform);
                }
                else
                {
                    addParent.GetComponent<Window>().active = true;
                }
                break;
            case "addpickup":
                if (addPickup == null)
                {
                    addPickup = Instantiate(addPickupPF, transform);
                }
                else
                {
                    addPickup.GetComponent<Window>().active = true;
                }
                break;
            case "xls":
                if(ocsv != null)
                {
                    ocsv.OutputData();
                }
                break;
            default:
                break;
        }
    }
}
