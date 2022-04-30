using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddNewPickup : MonoBehaviour
{
    Database db;
    [SerializeField]
    PickUpTimes newPickupTime = null;

    public InputField pickupTime;

    [Space()]
    public Text message;

    // Start is called before the first frame update
    void Start()
    {
        db = GameObject.FindGameObjectWithTag("Manager").GetComponent<Database>();

        newPickupTime = new PickUpTimes();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateInfo();
        CheckValidity();
    }

    void CheckValidity()
    {
        string mesText = "";

        if (db.pickuptimes.Exists(x => x.Times.Trim() == newPickupTime.Times))
        {
            mesText = "This Pickup Time already Exist";
        }

        if(newPickupTime.Times == "")
        {
            mesText += "Please add a time";
        }

        message.text = mesText;
    }

    public void AddPickupTime()
    {
        //Check if already exist
        if (!db.pickuptimes.Exists(x => x.Times.Trim() == newPickupTime.Times) && newPickupTime.Times != "")
        {
            //Add New
            db.AddNew(newPickupTime);
        }
    }
    
    void UpdateInfo()
    {
        if (pickupTime != null)
        {
            newPickupTime.Times = pickupTime.text.Trim();
        }

        newPickupTime.Computer = db.computer.text;
    }
}
