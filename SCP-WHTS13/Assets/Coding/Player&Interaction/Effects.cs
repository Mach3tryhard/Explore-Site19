using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.PostProcessing;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Effects : MonoBehaviour
{
    [Header("General")]
    public AudioSource _playerAudioSource;
    public TextMeshProUGUI _text;
    public Volume _volume;

    [Header("SCP 1079")]
    public float _candy=0; 
    public float _candyTimer=0;
    [SerializeField] private GameObject _candyDisplay;
    public GameObject _GO_SCP1079;
    public GameObject _candyDecal;
    public float _candyDecalTimer=1;
    private UnityEngine.Rendering.Universal.Vignette _candyPP;

    [Header("SCP 500")]
    public bool _cured=false; 
    public float _curedTimer=10;
    [SerializeField] private GameObject _curedDisplay;
    public GameObject _GO_SCP500;

    [Header("SCP 484")]
    public bool _chilled=false; 
    public float _chilledTimer=10;
    [SerializeField] private GameObject _chilledDisplay;
    public GameObject _GO_SCP484;

    [Header("SCP 207")]
    public bool _energized=false; 
    public float _energizedTimer=1000;
    public GameObject _GO_SCP207;

    [Header("SCP 198")]
    public bool _hurt=false; 
    public float _hurtTimer=1000;
    [SerializeField] private GameObject _hurtDisplay;
    public GameObject _GO_SCP198;

    [Header("SCP 215")]
    public bool _paranoia=false; 
    public float _paranoiaTimer=19;
    [SerializeField] private GameObject _paranoiaDisplay;
    public GameObject _GO_SCP215;

    [Header("Epipen")]
    public bool _stimulated=false; 
    public float _stimulatedTimer=9.5f;
    [SerializeField] private GameObject _stimulatedDisplay;
    public GameObject _GO_Epipen;

    GameObject _player;

    void Awake()
    {
        _player= GameObject.FindWithTag("Player");
        _playerAudioSource = GameObject.FindWithTag("Player").GetComponent<AudioSource>();
        _text = GameObject.FindWithTag("TextInfo").GetComponent<TextMeshProUGUI>();
        _volume = GameObject.FindWithTag("ItemVolume").GetComponent<Volume>();
        _volume.profile.TryGet(out _candyPP);
    }

    void Update()
    {
        ///KILL
        if(_player.GetComponent<FirstPersonController>().currentHealth<=0)
        {
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            print("DEAD");
        }
        /// EPIPEN
        if(_stimulated==true && _stimulatedTimer==9.5f)
        {
            _text.enabled=true;
            _text.text = "You feel energized.";
            _playerAudioSource.PlayOneShot(_GO_Epipen.GetComponent<ItemPickup>().useClips[0]);
            _stimulatedDisplay.GetComponent<RawImage>().enabled=true;
            /// SPEED 
            _player.GetComponent<FirstPersonController>().walkSpeed=_player.GetComponent<FirstPersonController>().walkSpeed+1f;
            _player.GetComponent<FirstPersonController>().sprintSpeed=_player.GetComponent<FirstPersonController>().sprintSpeed+2f;
            _player.GetComponent<FirstPersonController>().crouchSpeed=_player.GetComponent<FirstPersonController>().crouchSpeed+0.5f;
            _player.GetComponent<FirstPersonController>().slopeSpeed=_player.GetComponent<FirstPersonController>().slopeSpeed+3.75f;
        }
        if(_stimulated==true)
        {
            _stimulatedTimer-=Time.deltaTime;
            /// HP AND SPRINT
            _player.GetComponent<FirstPersonController>().currentHealth-=Time.deltaTime*2;
            _player.GetComponent<FirstPersonController>().staminaUseMultiplier=0;
            GameObject.FindWithTag("Canvas").GetComponent<UI>().healthBar.value=_player.GetComponent<FirstPersonController>().currentHealth;
            GameObject.FindWithTag("Canvas").GetComponent<UI>().HealthPercent.text=(int)_player.GetComponent<FirstPersonController>().currentHealth+"%";
            GameObject.FindWithTag("Canvas").GetComponent<UI>().staminaBar.value=_player.GetComponent<FirstPersonController>().currentStamina;
            GameObject.FindWithTag("Canvas").GetComponent<UI>().StaminaPercent.text=(int)_player.GetComponent<FirstPersonController>().currentStamina+"%";
            /// BEEPING DISPLAY
            if(_stimulatedTimer<=2)
            {
                _stimulatedDisplay.GetComponent<RawImage>().enabled=!_stimulatedDisplay.GetComponent<RawImage>().enabled;
            }
        }
        if(_stimulatedTimer<0)
        {
            _text.enabled=false;
            _stimulated=false;
            _stimulatedTimer=9.5f;
            /// SPEED BACK
            _player.GetComponent<FirstPersonController>().staminaUseMultiplier=15;
            _player.GetComponent<FirstPersonController>().walkSpeed=_player.GetComponent<FirstPersonController>().walkSpeed-1f;
            _player.GetComponent<FirstPersonController>().sprintSpeed=_player.GetComponent<FirstPersonController>().sprintSpeed-2f;
            _player.GetComponent<FirstPersonController>().crouchSpeed=_player.GetComponent<FirstPersonController>().crouchSpeed-0.5f;
            _player.GetComponent<FirstPersonController>().slopeSpeed=_player.GetComponent<FirstPersonController>().slopeSpeed-3.75f;
            _stimulatedDisplay.GetComponent<RawImage>().enabled=false;
        }
        /// SCP-1079
        if(_candy>0 && _candyTimer%15==0 && _candyTimer!=0)
        {
            _text.enabled=true;
            _text.text = "You feel a tingling sensations.";
            _candyDisplay.GetComponent<RawImage>().enabled=true;
            _playerAudioSource.PlayOneShot(_GO_SCP1079.GetComponent<ItemPickup>().useClips[0]);
        }
        if(_candy>0 && _candyTimer>0)
        {
            _candyTimer-=Time.deltaTime;
            _candyDecalTimer-=Time.deltaTime;
            _candyPP.intensity.value+=Time.deltaTime/100;
            ///
            if(_candyDecalTimer<=0)
            {
                Instantiate(_candyDecal, _player.transform.position, Quaternion.Euler(90,0,Random.Range(0f, 180.0f)),_candyDecal.transform.parent.gameObject.transform);
                _playerAudioSource.PlayOneShot(_GO_SCP1079.GetComponent<ItemPickup>().useClips[UnityEngine.Random.Range(1, _GO_SCP1079.GetComponent<ItemPickup>().useClips.Length - 1)]);
                _candyDecalTimer=1;
            }
            if(_candyTimer<=2)
            {
                _candyDisplay.GetComponent<RawImage>().enabled=!_candyDisplay.GetComponent<RawImage>().enabled;
            }
            _player.GetComponent<FirstPersonController>().currentHealth-=Time.deltaTime;
            GameObject.FindWithTag("Canvas").GetComponent<UI>().healthBar.value=_player.GetComponent<FirstPersonController>().currentHealth;
            GameObject.FindWithTag("Canvas").GetComponent<UI>().HealthPercent.text=(int)_player.GetComponent<FirstPersonController>().currentHealth+"%";
        }
        if(_candyTimer<0)
        {
            _text.enabled=false;
            _candyTimer=0;
            _candyPP.intensity.value=0f;
            /// SPEED BACK
            _candyDisplay.GetComponent<RawImage>().enabled=false;
        }
        /// SCP-500
        if(_cured==true && _curedTimer==10)
        {
            
            if(_hurt!=true)
            {
                _text.enabled=true;
                _text.text = "You feel AMAZING.";
            }
            _curedDisplay.GetComponent<RawImage>().enabled=true;
            _playerAudioSource.PlayOneShot(_GO_SCP500.GetComponent<ItemPickup>().useClips[0]);
            if(_energized==true)
                _energizedTimer=2;
            _stimulatedTimer=2;
            if(_chilled==true)
                _chilledTimer=2;
        }
        if(_cured==true)
        {
            _curedTimer-=Time.deltaTime;
            /// HP AND SPRINT
            _player.GetComponent<FirstPersonController>().currentHealth=100;
            _player.GetComponent<FirstPersonController>().currentStamina=100;
            GameObject.FindWithTag("Canvas").GetComponent<UI>().healthBar.value=_player.GetComponent<FirstPersonController>().currentHealth;
            GameObject.FindWithTag("Canvas").GetComponent<UI>().HealthPercent.text=(int)_player.GetComponent<FirstPersonController>().currentHealth+"%";
            GameObject.FindWithTag("Canvas").GetComponent<UI>().staminaBar.value=_player.GetComponent<FirstPersonController>().currentStamina;
            GameObject.FindWithTag("Canvas").GetComponent<UI>().StaminaPercent.text=(int)_player.GetComponent<FirstPersonController>().currentStamina+"%";
            if(_curedTimer<=2)
            {
                _curedDisplay.GetComponent<RawImage>().enabled=!_curedDisplay.GetComponent<RawImage>().enabled;
            }
        }
        if(_curedTimer<0)
        {
            _text.enabled=false;
            _cured=false;
            _curedTimer=10;
            /// SPEED BACK
            _curedDisplay.GetComponent<RawImage>().enabled=false;
        }
        /// SCP-484
        if(_chilled==true && _chilledTimer==10)
        {
            _text.enabled=true;
            _text.text = "This is chill yo.";
            _chilledDisplay.GetComponent<RawImage>().enabled=true;
            _playerAudioSource.PlayOneShot(_GO_SCP484.GetComponent<ItemPickup>().useClips[0]);
            if(_stimulated==true)
            _stimulatedTimer=2;
        }
        if(_chilled==true)
        {
            _chilledTimer-=Time.deltaTime;
            /// HP AND SPRINT
            if(_player.GetComponent<FirstPersonController>().currentHealth<100)
                _player.GetComponent<FirstPersonController>().currentHealth+=Time.deltaTime;
            GameObject.FindWithTag("Canvas").GetComponent<UI>().healthBar.value=_player.GetComponent<FirstPersonController>().currentHealth;
            GameObject.FindWithTag("Canvas").GetComponent<UI>().HealthPercent.text=(int)_player.GetComponent<FirstPersonController>().currentHealth+"%";
            if(_chilledTimer<=2)
            {
                _chilledDisplay.GetComponent<RawImage>().enabled=!_chilledDisplay.GetComponent<RawImage>().enabled;
            }
        }
        if(_chilledTimer<0)
        {
            _text.enabled=false;
            _chilled=false;
            _chilledTimer=10;
            _chilledDisplay.GetComponent<RawImage>().enabled=false;
        }
        /// SCP-207
        if(_energized==true && _energizedTimer==1000)
        {
            _text.enabled=true;
            _text.text = "You feel energized.";
            _stimulatedDisplay.GetComponent<RawImage>().enabled=true;
            _playerAudioSource.PlayOneShot(_GO_SCP207.GetComponent<ItemPickup>().useClips[0]);
            /// SPEED 
            _player.GetComponent<FirstPersonController>().walkSpeed=_player.GetComponent<FirstPersonController>().walkSpeed+2f;
            _player.GetComponent<FirstPersonController>().sprintSpeed=_player.GetComponent<FirstPersonController>().sprintSpeed+4f;
            _player.GetComponent<FirstPersonController>().crouchSpeed=_player.GetComponent<FirstPersonController>().crouchSpeed+1f;
            _player.GetComponent<FirstPersonController>().slopeSpeed=_player.GetComponent<FirstPersonController>().slopeSpeed+7.5f;
        }
        if(_energized==true)
        {
            _energizedTimer-=Time.deltaTime;
            /// HP AND SPRINT
            _player.GetComponent<FirstPersonController>().currentHealth-=Time.deltaTime;
            _player.GetComponent<FirstPersonController>().currentStamina=100;
            _player.GetComponent<FirstPersonController>().staminaUseMultiplier=0;
            GameObject.FindWithTag("Canvas").GetComponent<UI>().healthBar.value=_player.GetComponent<FirstPersonController>().currentHealth;
            GameObject.FindWithTag("Canvas").GetComponent<UI>().HealthPercent.text=(int)_player.GetComponent<FirstPersonController>().currentHealth+"%";
            GameObject.FindWithTag("Canvas").GetComponent<UI>().staminaBar.value=_player.GetComponent<FirstPersonController>().currentStamina;
            GameObject.FindWithTag("Canvas").GetComponent<UI>().StaminaPercent.text=(int)_player.GetComponent<FirstPersonController>().currentStamina+"%";
            if(_energizedTimer<=2)
            {
                _stimulatedDisplay.GetComponent<RawImage>().enabled=!_stimulatedDisplay.GetComponent<RawImage>().enabled;
            }
        }
        if(_energizedTimer<0)
        {
            _text.enabled=false;
            _energized=false;
            _energizedTimer=10;
            /// SPEED BACK
            _player.GetComponent<FirstPersonController>().staminaUseMultiplier=15;
            _player.GetComponent<FirstPersonController>().walkSpeed=_player.GetComponent<FirstPersonController>().walkSpeed-2f;
            _player.GetComponent<FirstPersonController>().sprintSpeed=_player.GetComponent<FirstPersonController>().sprintSpeed-4f;
            _player.GetComponent<FirstPersonController>().crouchSpeed=_player.GetComponent<FirstPersonController>().crouchSpeed-1f;
            _player.GetComponent<FirstPersonController>().slopeSpeed=_player.GetComponent<FirstPersonController>().slopeSpeed-7.5f;
            _stimulatedDisplay.GetComponent<RawImage>().enabled=false;
        }
        /// SCP-198
        if(_hurt==true && _hurtTimer==1000)
        {
            _text.text = "You feel the cup melting with your hand.";
            _hurtDisplay.GetComponent<RawImage>().enabled=true;
            _playerAudioSource.PlayOneShot(_GO_SCP198.GetComponent<ItemPickup>().useClips[0]);
        }
        if(_hurt==true)
        {
            _text.enabled=true;
            _hurtTimer-=Time.deltaTime;
            /// HP
            _player.GetComponent<FirstPersonController>().timeBeforeStaminaRegenStarts=1000;
            _player.GetComponent<FirstPersonController>().currentHealth-=Time.deltaTime;
            if(_player.GetComponent<FirstPersonController>().currentStamina>0)
            {
                _player.GetComponent<FirstPersonController>().currentStamina-=Time.deltaTime;
            }
            GameObject.FindWithTag("Canvas").GetComponent<UI>().healthBar.value=_player.GetComponent<FirstPersonController>().currentHealth;
            GameObject.FindWithTag("Canvas").GetComponent<UI>().HealthPercent.text=(int)_player.GetComponent<FirstPersonController>().currentHealth+"%";
            GameObject.FindWithTag("Canvas").GetComponent<UI>().staminaBar.value=_player.GetComponent<FirstPersonController>().currentStamina;
            GameObject.FindWithTag("Canvas").GetComponent<UI>().StaminaPercent.text=(int)_player.GetComponent<FirstPersonController>().currentStamina+"%";
            if(_hurtTimer<=2)
            {
                _hurtDisplay.GetComponent<RawImage>().enabled=!_hurtDisplay.GetComponent<RawImage>().enabled;
            }
        }
        if(_hurtTimer<0)
        {
            _text.enabled=false;
            _player.GetComponent<FirstPersonController>().timeBeforeStaminaRegenStarts=4;
            _hurt=false;
            _hurtTimer=1000;
            _hurtDisplay.GetComponent<RawImage>().enabled=false;
        }
        /// SCP-215
        if(_paranoia==true)
        {
            _text.enabled=true;
            _text.text = "You feel like the objects are watching you.";
            _paranoiaDisplay.GetComponent<RawImage>().enabled=true;
        }
        if(_paranoia==false)
        {
            _paranoiaDisplay.GetComponent<RawImage>().enabled=false;
            _paranoiaTimer=19;
        }
    }
}
