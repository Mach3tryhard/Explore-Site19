using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ElevatorButton : Interacteble
{
    [SerializeField]private AudioSource playerAudioSource = default;
    [SerializeField] private AudioClip[] doorClipsclose = default;
    [SerializeField] private AudioClip[] doorClipsopen = default;
    [SerializeField] private AudioClip[] elevatorNoises = default;
    [SerializeField] private AudioClip[] ScanSounds = default;
    [SerializeField] private Animator myDoor1 = null;
    [SerializeField] private Animator myDoor2 = null;
    public GameObject GameObjButton;
    public TextMeshProUGUI _text;
    public GameObject _player;
    
    bool elevatorupdate1=false;
    bool[] step1 = new bool[] {false,false,false};
    bool elevatorupdate2=false;
    bool[] step2 = new bool[] {false,false,false};

    void Start()
    {
        playerAudioSource = GameObject.FindWithTag("Player").GetComponent<AudioSource>();
        _text = GameObject.FindWithTag("TextInfo").GetComponent<TextMeshProUGUI>();
        _player = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        if(elevatorupdate1==true)
        {
            if(step1[0]==true)
            {
                GameObject.FindWithTag("Elevator").GetComponent<ElevatorTimer>().ElevTimer=11;
                playerAudioSource.PlayOneShot(ScanSounds[0]);
                if(GameObject.FindWithTag("DoorCheck").GetComponent<DoorCheck>().opened==true)
                {
                    myDoor1.Play("dor_close", 0, 0.0f);
                    playerAudioSource.PlayOneShot(doorClipsclose[UnityEngine.Random.Range(0, doorClipsclose.Length - 1)]);
                    GameObject.FindWithTag("DoorCheck").GetComponent<DoorCheck>().opened=false;
                }
                step1[0]=false;
            }
            if(GameObject.FindWithTag("Elevator").GetComponent<ElevatorTimer>().ElevTimer<=9 && step1[1]==true)
            {
                playerAudioSource.PlayOneShot(elevatorNoises[1]);
                _player.GetComponent<FirstPersonController>().CanMove=false;
                CharacterController cc = _player.GetComponent<CharacterController>();
                cc.enabled=false;
                _player.transform.position += new Vector3(0,-50,0);
                cc.enabled=true;
                _player.GetComponent<FirstPersonController>().CanMove=true;
                step1[1]=false;
            }
            if(GameObject.FindWithTag("Elevator").GetComponent<ElevatorTimer>().ElevTimer<=2 && step1[2]==true)
            {
                playerAudioSource.PlayOneShot(elevatorNoises[0]);
                playerAudioSource.PlayOneShot(doorClipsopen[UnityEngine.Random.Range(0, doorClipsopen.Length - 1)]);
                myDoor2.Play("dor_open", 0, 0.0f);
                GameObject.FindWithTag("DoorCheck2").GetComponent<DoorCheck>().opened=true;
                step1[2]=false;
            }
            if(GameObject.FindWithTag("Elevator").GetComponent<ElevatorTimer>().ElevTimer<=0)
            {
                elevatorupdate1=false;
            }
        }
        if(elevatorupdate2==true)
        {
            if(step2[0]==true)
            {
                GameObject.FindWithTag("Elevator").GetComponent<ElevatorTimer>().ElevTimer=11;
                playerAudioSource.PlayOneShot(ScanSounds[0]);
                if(GameObject.FindWithTag("DoorCheck2").GetComponent<DoorCheck>().opened==true)
                {
                    myDoor1.Play("dor_close", 0, 0.0f);
                    playerAudioSource.PlayOneShot(doorClipsclose[UnityEngine.Random.Range(0, doorClipsclose.Length - 1)]);
                    GameObject.FindWithTag("DoorCheck").GetComponent<DoorCheck>().opened=false;
                }
                step2[0]=false;
            }
            if(GameObject.FindWithTag("Elevator").GetComponent<ElevatorTimer>().ElevTimer<=9 && step2[1]==true)
            {
                playerAudioSource.PlayOneShot(elevatorNoises[1]);
                _player.GetComponent<FirstPersonController>().CanMove=false;
                CharacterController cc = _player.GetComponent<CharacterController>();
                cc.enabled=false;
                _player.transform.position += new Vector3(0,50,0);
                cc.enabled=true;
                _player.GetComponent<FirstPersonController>().CanMove=true;
                step2[1]=false;
            }
            if(GameObject.FindWithTag("Elevator").GetComponent<ElevatorTimer>().ElevTimer<=2 && step2[2]==true)
            {
                playerAudioSource.PlayOneShot(elevatorNoises[0]);
                playerAudioSource.PlayOneShot(doorClipsopen[UnityEngine.Random.Range(0, doorClipsopen.Length - 1)]);
                myDoor2.Play("dor_open", 0, 0.0f);
                GameObject.FindWithTag("DoorCheck").GetComponent<DoorCheck>().opened=true;
                step2[2]=false;
            }
            if(GameObject.FindWithTag("Elevator").GetComponent<ElevatorTimer>().ElevTimer<=0)
            {
                elevatorupdate2=false;
            }
        }
    }

    public override void OnFocus()
    {
        print("Looking at " + gameObject.name);
        if(gameObject.name == "ButtonElevator" || gameObject.name == "ButtonElevator2")
        {
            /// Text Popup
            _text.enabled=true;
            if(GameObject.FindWithTag("Elevator").GetComponent<ElevatorTimer>().ElevTimer>0)_text.text = "You are using the elevator";
            else _text.text = "Use elevator";
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
        if(gameObject.name == "ButtonElevator" && GameObject.FindWithTag("Elevator").GetComponent<ElevatorTimer>().ElevTimer<=0)
        {
            elevatorupdate1=true;
            step1[0]=true;step1[1]=true;step1[2]=true;
        }
        if(gameObject.name == "ButtonElevator2" && GameObject.FindWithTag("Elevator").GetComponent<ElevatorTimer>().ElevTimer<=0)
        {
            elevatorupdate2=true;
            step2[0]=true;step2[1]=true;step2[2]=true;
        }
    }
    public override void OnLoseFocus()
    {
        print("Stopped looking at " + gameObject.name);
        _text.enabled=false;
        if((gameObject.name == "ButtonElevator" || gameObject.name == "ButtonElevator2"))
            GameObject.FindWithTag("Canvas").transform.GetChild(0).gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(0,0,0);
    }
}
