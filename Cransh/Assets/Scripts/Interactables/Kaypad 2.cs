using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KayPad2 : Interactable
{
    // Start is called before the first frame update
    [SerializeField]
    public GameObject Pixel_box_1;
    private bool BoxOpen;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    protected override void Interact()
    {
        BoxOpen = !BoxOpen;
    }
}
