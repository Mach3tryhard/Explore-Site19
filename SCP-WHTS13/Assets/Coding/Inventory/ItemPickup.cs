using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemPickup : Interacteble
{
    public Item item;
    public GameObject GameObjItem;

    [SerializeField]private AudioSource playerAudioSource = default;
    [SerializeField] private AudioClip[] pickupClips = default;
    public AudioClip[] useClips = default;
    public TextMeshProUGUI _text;

    void Start()
    {
        playerAudioSource = GameObject.FindWithTag("Player").GetComponent<AudioSource>();
        _text = GameObject.FindWithTag("TextInfo").GetComponent<TextMeshProUGUI>();
    }

    public override void OnFocus()
    {
        Debug.Log("Looking at " + item.name);
        /// Text Popup
        _text.enabled=true;
        _text.text = "MB1 - Pickup" + "\n" + item.name;
        ///Moving Crosshair position
        Camera _camera = GameObject.FindWithTag("Player").transform.GetChild(0).gameObject.GetComponent<Camera>();
        RectTransform _trans = GameObject.FindWithTag("Canvas").transform.GetChild(0).gameObject.GetComponent<RectTransform>();
        Vector2 _positionscreen = _camera.WorldToScreenPoint(GameObjItem.transform.position);
        Canvas _mycanvas = GameObject.FindWithTag("Canvas").GetComponent<Canvas>();
        float _scalefactor = _mycanvas.scaleFactor;
        Vector2 _finalpos = new Vector2(_positionscreen.x / _scalefactor, _positionscreen.y / _scalefactor );
        _trans.position = _finalpos;
    }
    public override void OnInteract()
    {
        Debug.Log("Picked up " + item.name);
        bool wasPickedUp = Inventory.instance.Add(item);
        if(wasPickedUp==true)
        {
            item.gameObjectOfItem = GameObjItem;
            Vector3 temp = new Vector3(0,-10.0f,0);
            item.gameObjectOfItem.transform.position=GameObject.FindWithTag("ObjectDrop").transform.position;
            item.gameObjectOfItem.SetActive(false);
            item.gameObjectOfItem.transform.parent = GameObject.FindWithTag("Player").transform;

            playerAudioSource.PlayOneShot(pickupClips[UnityEngine.Random.Range(0, pickupClips.Length - 1)]);

            _text.enabled=false;
            GameObject.FindWithTag("Canvas").transform.GetChild(0).gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(0,0,0);
            
        } 
    }
    public override void OnLoseFocus()
    {
        _text.enabled=false;
        GameObject.FindWithTag("Canvas").transform.GetChild(0).gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(0,0,0);
        Debug.Log("Stopped looking at " + item.name);
    }
}
