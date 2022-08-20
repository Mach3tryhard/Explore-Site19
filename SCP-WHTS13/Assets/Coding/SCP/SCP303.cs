using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SCP303 : MonoBehaviour
{
    [SerializeField] private GameObject _theparent;
    //[SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private GameObject _player;
    [SerializeField] private AudioSource _sounds;
    [SerializeField] private AudioSource playerAudioSource = default;
    [SerializeField] private AudioClip[] jumpscare = default;

    public List<Vector3> _scpPositions = new List<Vector3>();
    public List<Vector3> _scpRotation = new List<Vector3>();

    [Header("Seeing SCP303 parameters")]
    public GameObject target;
    public Camera cam;

    private bool IsVisible(Camera c, GameObject target)
    {
        var planes = GeometryUtility.CalculateFrustumPlanes(c);
        var point = target.transform.position;

        foreach (var plane in planes)
        {
            if (plane.GetDistanceToPoint(point)< 0)
            {
                return false;
            }
        }
        return true;
    }

    float _timer=0;

    private void Update ()
    {
        _timer+=Time.deltaTime;
        var targetRender = target.GetComponent<Renderer>();
        if (IsVisible(cam,target))
        {
            RaycastHit hitInfo = new RaycastHit();
            Vector3 dir = target.transform.position - cam.transform.position;
            if(Physics.Raycast(cam.transform.position, dir, out hitInfo, 1000))
            {
                Debug.Log(hitInfo.collider.name+", "+hitInfo.collider.tag);
                if(hitInfo.collider.tag=="SCP/303")
                {
                    playerAudioSource.PlayOneShot(jumpscare[0]);
                    Teleport();
                }
            }
        }
        if(_timer>=300)
        {
            Teleport();
        }
    }

    private void Teleport()
    {
        int i=UnityEngine.Random.Range(0, _scpPositions.Count);
        _theparent.transform.position = _scpPositions[i];
        _theparent.transform.eulerAngles = _scpRotation[i];
        _timer=0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            _sounds.enabled=true;
            //_text.enabled=true;
            //_text.text = "What the hell is behind this door!";
            _player.GetComponent<FirstPersonController>().walkSpeed=2f;
            _player.GetComponent<FirstPersonController>().canSprint=false;
            _player.GetComponent<FirstPersonController>().crouchSpeed=0.5f;
            _player.GetComponent<FirstPersonController>().slopeSpeed=3.75f;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            _sounds.enabled=false;
            //_text.enabled=false;
            _player.GetComponent<FirstPersonController>().walkSpeed=4f;
            _player.GetComponent<FirstPersonController>().canSprint=false;
            _player.GetComponent<FirstPersonController>().crouchSpeed=2f;
            _player.GetComponent<FirstPersonController>().slopeSpeed=15f;
        }
    }
}
