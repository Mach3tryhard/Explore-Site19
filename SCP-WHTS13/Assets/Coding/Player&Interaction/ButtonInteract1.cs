using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonInteract1 : Interacteble
{
    [SerializeField]private AudioSource playerAudioSource = default;
    [SerializeField] private AudioClip[] doorClipsopen = default;
    [SerializeField] private AudioClip[] doorClipsclose = default;
    [SerializeField] private AudioClip[] doorClipsnoopen = default;
    [SerializeField] private AudioClip[] ScanSounds = default;
    [SerializeField] private Animator myDoor1 = null;
    [SerializeField] private  int SecurityLevel = 1;
    public GameObject GameObjButton;
    public TextMeshProUGUI _text;

    void Start()
    {
        playerAudioSource = GameObject.FindWithTag("Player").GetComponent<AudioSource>();
        _text = GameObject.FindWithTag("TextInfo").GetComponent<TextMeshProUGUI>();
    }

    public override void OnFocus()
    {
        print("Looking at " + gameObject.name);
        if(gameObject.name == "Button"  || gameObject.name == "Button1")
        {
            /// Text Popup
            _text.enabled=true;
            if(SecurityLevel<0)
            {
                _text.text = "Use elevator Door.";
            }
            else
            {
                _text.text = "Level " + SecurityLevel +  " Keycard Required";
            }
            /// Crosshair move
            Camera _camera = GameObject.FindWithTag("Player").transform.GetChild(0).gameObject.GetComponent<Camera>();
            RectTransform _trans = GameObject.FindWithTag("Canvas").transform.GetChild(0).gameObject.GetComponent<RectTransform>();
            Vector2 _positionscreen = _camera.WorldToScreenPoint(GameObjButton.transform.position);
            Canvas _mycanvas = GameObject.FindWithTag("Canvas").GetComponent<Canvas>();
            float _scalefactor = _mycanvas.scaleFactor;
            Vector2 _finalpos = new Vector2(_positionscreen.x / _scalefactor, _positionscreen.y / _scalefactor );
            _trans.position = _finalpos;
        }
    }
    public override void OnInteract()
    {
        print("Interacted with " + gameObject.name);
        if(gameObject.name == "Button" || gameObject.name == "Button1")
        {
            if(transform.parent.gameObject.GetComponent<DoorCheck>().opened == false && transform.parent.gameObject.GetComponent<DoorCheck>().damTimer<=0 && GameObject.FindWithTag("GameManager").GetComponent<Inventory>().CheckCardLevel()>=SecurityLevel)
            {
                playerAudioSource.PlayOneShot(ScanSounds[UnityEngine.Random.Range(0, ScanSounds.Length - 1)]);
                myDoor1.Play("dor_open", 0, 0.0f);
                transform.parent.gameObject.GetComponent<DoorCheck>().opened=true;
                transform.parent.gameObject.GetComponent<DoorCheck>().damTimer=2;
                playerAudioSource.PlayOneShot(doorClipsopen[UnityEngine.Random.Range(0, doorClipsopen.Length - 1)]);
            }
            else
            if(transform.parent.gameObject.GetComponent<DoorCheck>().opened == true && transform.parent.gameObject.GetComponent<DoorCheck>().damTimer<=0 && GameObject.FindWithTag("GameManager").GetComponent<Inventory>().CheckCardLevel()>=SecurityLevel)
            {
                playerAudioSource.PlayOneShot(ScanSounds[UnityEngine.Random.Range(0, ScanSounds.Length - 1)]);
                myDoor1.Play("dor_close", 0, 0.0f);
                transform.parent.gameObject.GetComponent<DoorCheck>().opened=false;
                transform.parent.gameObject.GetComponent<DoorCheck>().damTimer=2;
                playerAudioSource.PlayOneShot(doorClipsclose[UnityEngine.Random.Range(0, doorClipsclose.Length - 1)]);
            }
            else
            if(GameObject.FindWithTag("GameManager").GetComponent<Inventory>().CheckCardLevel()<SecurityLevel && transform.parent.gameObject.GetComponent<DoorCheck>().damTimer<=0)
            {
                playerAudioSource.PlayOneShot(doorClipsnoopen[UnityEngine.Random.Range(0, doorClipsnoopen.Length - 1)]);
                transform.parent.gameObject.GetComponent<DoorCheck>().damTimer=2;
            }
        }
    }
    public override void OnLoseFocus()
    {
        print("Stopped looking at " + gameObject.name);
        _text.enabled=false;
        if(gameObject.name == "Button"  || gameObject.name == "Button1")
            GameObject.FindWithTag("Canvas").transform.GetChild(0).gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(0,0,0);
    }
}
