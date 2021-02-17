﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using com.Core.Configuration;
public class ServerSlotUpdater : MonoBehaviour
{

    [SerializeField] private Slider slotsSlider;
    [SerializeField] private Text minSlots_label;
    [SerializeField] private Text maxSlots_label;
    // Start is called before the first frame update
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
        
    }
}
