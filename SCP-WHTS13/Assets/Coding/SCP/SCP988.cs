using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SCP988 : Interacteble
{
    [SerializeField]private AudioSource playerAudioSource = default;
    [SerializeField] private AudioClip[] chestNoises = default;
    public GameObject GameObj;
    public TextMeshProUGUI _text;
    bool heartbeat=false;
    float beatTimer=0;
    float beatLimit=5;

    void Start()
    {
        playerAudioSource = GameObject.FindWithTag("Player").GetComponent<AudioSource>();
        _text = GameObject.FindWithTag("TextInfo").GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if(heartbeat==true)
        {
            if(beatTimer>0)
            {
                beatTimer-=Time.deltaTime;
            }
            if(beatTimer<=0)
            {
                playerAudioSource.PlayOneShot(chestNoises[1]);
                beatLimit/=1.1f;
                beatTimer=beatLimit;
            }
            if(beatLimit<0.2)
            {
                GameObject.FindWithTag("Player").GetComponent<FirstPersonController>().KillPlayer();
                heartbeat=false;
            }
        }
    }

    public override void OnFocus()
    {
        print("Looking at " + gameObject.name);
        if(gameObject.name == "SCP988")
        {
            /// Text Popup
            _text.enabled=true;
            if(heartbeat==false)
            {
                _text.text = "Open the chest";
            }
            /// Crosshair move
            Camera _camera = GameObject.FindWithTag("Player").transform.GetChild(0).gameObject.GetComponent<Camera>();
            RectTransform _trans = GameObject.FindWithTag("Canvas").transform.GetChild(0).gameObject.GetComponent<RectTransform>();
            Vector2 _positionscreen = _camera.WorldToScreenPoint(GameObj.transform.position);
            Canvas _mycanvas = GameObject.FindWithTag("Canvas").GetComponent<Canvas>();
            float _scalefactor = _mycanvas.scaleFactor;
            Vector2 _finalpos = new Vector2(_positionscreen.x / _scalefactor, _positionscreen.y / _scalefactor );
            _trans.position = _finalpos;
        }
    }
    public override void OnInteract()
    {
        print("Interacted with " + gameObject.name);
        if(gameObject.name == "SCP988")
        {
            playerAudioSource.PlayOneShot(chestNoises[0]);
            _text.text = "The chest won't open";
            heartbeat=true;
        }
    }
    public override void OnLoseFocus()
    {
        print("Stopped looking at " + gameObject.name);
        _text.enabled=false;
        if(gameObject.name == "SCP988")
            GameObject.FindWithTag("Canvas").transform.GetChild(0).gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(0,0,0);
    }
}
