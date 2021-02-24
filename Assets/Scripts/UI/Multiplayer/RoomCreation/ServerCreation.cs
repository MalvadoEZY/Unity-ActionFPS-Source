using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using com.Core.Configuration;
using com.Core.Network;
public class ServerCreation : MonoBehaviour
{
    //Form inputs
    [SerializeField] private Toggle isPrivateServer;
    [SerializeField] private InputField nameServerInput;
    [SerializeField] private Slider slotsSlider;

    //Other info
    [SerializeField] private Text minSlots_label;
    [SerializeField] private Text maxSlots_label;
    // Start is called before the first frame update

    private float currentSlots = 1;
    void Start()
    {
        slotsSlider.minValue = 1;
        minSlots_label.text = "1";
        maxSlots_label.text = ConfigurationFile.s_slots.ToString();
        slotsSlider.maxValue = ConfigurationFile.s_slots;
    }

    // Update is called once per frame
    void Update()
    {
        currentSlots = slotsSlider.value;
        minSlots_label.text = slotsSlider.value.ToString();
    }
    
    //Will verify the name field before creating the server on NetworkManager 
    public void attemptToCreateRoom()
    {
        //Name length checker before creating server.  WIP: Notification system
        if (nameServerInput.text.Length < 5 || nameServerInput.text.Length > 30) return;

        Debug.Log("Server created. Private Status: " + isPrivateServer);
        NetworkManager.CreateServer(nameServerInput.text, (byte)currentSlots, true, isPrivateServer.enabled);
    }
}
