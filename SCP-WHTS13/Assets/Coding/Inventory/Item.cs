using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;
using TMPro;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    new public string name = "New Item";
    public Sprite icon = null;
    public bool isUsing = false;
    public GameObject gameObjectOfItem;
    public bool oneTimeUse = true;
    public Sprite document = null;
    public bool isDocument = false;

    public virtual void Use()
    {
        AudioSource playerAudioSource = GameObject.FindWithTag("Player").GetComponent<AudioSource>();
        GameObject holdPlace = GameObject.FindWithTag("ObjectHoldPlace");
        Transform PlaceItemHereOnDrop = GameObject.FindWithTag("ObjectDrop").transform;
        TextMeshProUGUI _TEXT = GameObject.FindWithTag("TextInfo").GetComponent<TextMeshProUGUI>();

        if(name=="Medkit")
        {
            playerAudioSource.PlayOneShot(gameObjectOfItem.GetComponent<ItemPickup>().useClips[0]);
            GameObject.FindWithTag("Player").GetComponent<FirstPersonController>().currentHealth+=50;
            if(GameObject.FindWithTag("Player").GetComponent<FirstPersonController>().currentHealth>100)GameObject.FindWithTag("Player").GetComponent<FirstPersonController>().currentHealth=100;
            GameObject.FindWithTag("Canvas").GetComponent<UI>().healthBar.value=GameObject.FindWithTag("Player").GetComponent<FirstPersonController>().currentHealth;
            GameObject.FindWithTag("Canvas").GetComponent<UI>().HealthPercent.text=GameObject.FindWithTag("Player").GetComponent<FirstPersonController>().currentHealth+"%";
            Debug.Log("Using " + name);
        }
        if(name=="NightVisionGoggle")
        {
            ColorGrading  _colorGrading=null;
            GameObject.FindWithTag("PP/Item").GetComponent<PostProcessVolume>().profile.TryGetSettings(out _colorGrading);
            if(GameObject.FindWithTag("Player").GetComponent<FirstPersonController>().canZoom==false)
            {
                playerAudioSource.PlayOneShot(gameObjectOfItem.GetComponent<ItemPickup>().useClips[0]);
                GameObject.FindWithTag("Player").GetComponent<FirstPersonController>().canZoom = true;
                _colorGrading.enabled.value=true;
                Debug.Log("Activating" + name);
                isUsing = true;
            }
            else
            if(GameObject.FindWithTag("Player").GetComponent<FirstPersonController>().canZoom==true)
            {
                playerAudioSource.PlayOneShot(gameObjectOfItem.GetComponent<ItemPickup>().useClips[1]);
                GameObject.FindWithTag("Player").GetComponent<FirstPersonController>().canZoom = false;
                _colorGrading.enabled.value=false;
                Debug.Log("DEActivating" + name);
                isUsing = false;
            }
        }
        if(name=="GasMask")
        {
            Vignette _vignette=null;
            GameObject.FindWithTag("PP/Item").GetComponent<PostProcessVolume>().profile.TryGetSettings(out _vignette);
            if(GameObject.FindWithTag("Player").GetComponent<FirstPersonController>().staminaUseMultiplier==15)
            {
                playerAudioSource.PlayOneShot(gameObjectOfItem.GetComponent<ItemPickup>().useClips[0]);
                GameObject.FindWithTag("Player").GetComponent<FirstPersonController>().staminaUseMultiplier=7;
                _vignette.enabled.value=true;
                Debug.Log("Activating" + name);
                isUsing = true;
            }
            else
            if(GameObject.FindWithTag("Player").GetComponent<FirstPersonController>().staminaUseMultiplier==7)
            {
                playerAudioSource.PlayOneShot(gameObjectOfItem.GetComponent<ItemPickup>().useClips[1]);
                GameObject.FindWithTag("Player").GetComponent<FirstPersonController>().staminaUseMultiplier=15;
                _vignette.enabled.value=false;
                Debug.Log("DEActivating" + name);
                isUsing = false;
            }
        }
        if(name=="Vest")
        {
            if(GameObject.FindWithTag("Player").GetComponent<FirstPersonController>().walkSpeed==4f)
            {
                playerAudioSource.PlayOneShot(gameObjectOfItem.GetComponent<ItemPickup>().useClips[0]);
                ///Hp grown
                GameObject.FindWithTag("SCP/173").GetComponent<SCPXXXX>().damageDealt/=2;
                GameObject.FindWithTag("SCP/008").GetComponent<SCPXXXX>().damageDealt/=2;
                ///movement slowed
                GameObject.FindWithTag("Player").GetComponent<FirstPersonController>().walkSpeed=3f;
                GameObject.FindWithTag("Player").GetComponent<FirstPersonController>().sprintSpeed=6f;
                GameObject.FindWithTag("Player").GetComponent<FirstPersonController>().crouchSpeed=1.5f;
                GameObject.FindWithTag("Player").GetComponent<FirstPersonController>().slopeSpeed=11.25f;
                Debug.Log("Activating" + name);
                isUsing = true;
            }
            else
            if(GameObject.FindWithTag("Player").GetComponent<FirstPersonController>().walkSpeed==3f)
            {
                playerAudioSource.PlayOneShot(gameObjectOfItem.GetComponent<ItemPickup>().useClips[1]);
                ///Hp grown
                GameObject.FindWithTag("SCP/173").GetComponent<SCPXXXX>().damageDealt*=2;
                GameObject.FindWithTag("SCP/008").GetComponent<SCPXXXX>().damageDealt*=2;
                ///movement slowed
                GameObject.FindWithTag("Player").GetComponent<FirstPersonController>().walkSpeed=4f;
                GameObject.FindWithTag("Player").GetComponent<FirstPersonController>().sprintSpeed=8f;
                GameObject.FindWithTag("Player").GetComponent<FirstPersonController>().crouchSpeed=2f;
                GameObject.FindWithTag("Player").GetComponent<FirstPersonController>().slopeSpeed=15f;
                Debug.Log("DEActivating" + name);
                isUsing = false;
            }
        }
        if(name=="HazmatSuit")
        {
            if(isUsing==false)
            {
                playerAudioSource.PlayOneShot(gameObjectOfItem.GetComponent<ItemPickup>().useClips[0]);
                GameObject.FindWithTag("SCP/008").GetComponent<SCPXXXX>().damageDealt=0;
                GameObject.FindWithTag("Player").GetComponent<FirstPersonController>().walkSpeed=3f;
                GameObject.FindWithTag("Player").GetComponent<FirstPersonController>().sprintSpeed=6f;
                GameObject.FindWithTag("Player").GetComponent<FirstPersonController>().crouchSpeed=1.5f;
                GameObject.FindWithTag("Player").GetComponent<FirstPersonController>().slopeSpeed=11.25f;
                Debug.Log("Activating" + name);
                isUsing = true;
            }
            else
            if(isUsing==true)
            {
                playerAudioSource.PlayOneShot(gameObjectOfItem.GetComponent<ItemPickup>().useClips[1]);
                GameObject.FindWithTag("SCP/008").GetComponent<SCPXXXX>().damageDealt=20;
                GameObject.FindWithTag("Player").GetComponent<FirstPersonController>().walkSpeed=4f;
                GameObject.FindWithTag("Player").GetComponent<FirstPersonController>().sprintSpeed=8f;
                GameObject.FindWithTag("Player").GetComponent<FirstPersonController>().crouchSpeed=2f;
                GameObject.FindWithTag("Player").GetComponent<FirstPersonController>().slopeSpeed=15f;
                Debug.Log("Activating" + name);
                isUsing = false;
            }
        }
        if(name=="FlashLight")
        {
            if(isUsing==false)
            {
                ///Punerea parintelui Corect
                gameObjectOfItem.transform.parent = GameObject.FindWithTag("MainCamera").transform;
                /// Audio Si Vizual
                playerAudioSource.PlayOneShot(gameObjectOfItem.GetComponent<ItemPickup>().useClips[0]);
                GameObject.FindWithTag("Item/Flashlight").GetComponent<Light>().enabled=true;
                ///RigidBody si activarea
                gameObjectOfItem.SetActive(true);
                gameObjectOfItem.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
                gameObjectOfItem.GetComponent<Rigidbody>().freezeRotation = true;
                ///Punerea pe pozitii
                gameObjectOfItem.transform.position=holdPlace.transform.position;
                gameObjectOfItem.transform.eulerAngles = new Vector3(GameObject.FindWithTag("MainCamera").transform.eulerAngles.x+270,GameObject.FindWithTag("MainCamera").transform.eulerAngles.y,GameObject.FindWithTag("MainCamera").transform.eulerAngles.z);
                isUsing = true;
                Debug.Log("Activating" + name);
            }
            else
            if(isUsing==true)
            {
                gameObjectOfItem.transform.parent = GameObject.FindWithTag("ItemParent").transform;
                playerAudioSource.PlayOneShot(gameObjectOfItem.GetComponent<ItemPickup>().useClips[1]);
                GameObject.FindWithTag("Item/Flashlight").GetComponent<Light>().enabled=false;
                gameObjectOfItem.SetActive(false);
                gameObjectOfItem.transform.position=PlaceItemHereOnDrop.position;
                gameObjectOfItem.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                gameObjectOfItem.GetComponent<Rigidbody>().freezeRotation = false;
                isUsing = false;
            }
        }
        if(name=="StimPack")
        {
            GameObject.FindWithTag("Player").GetComponent<Effects>()._stimulated=true;
            GameObject.FindWithTag("Player").GetComponent<Effects>()._stimulatedTimer=9.5f;
            GameObject.FindWithTag("Player").GetComponent<Effects>()._GO_Epipen=gameObjectOfItem;
        }
        #region Documents
        if(isDocument)
        {
            playerAudioSource.PlayOneShot(gameObjectOfItem.GetComponent<ItemPickup>().useClips[0]);
            GameObject.FindWithTag("Document").GetComponent<Image>().sprite=document;
            GameObject.FindWithTag("Document").GetComponent<Image>().enabled=true;
            ///Scoaterea Inventarului
            GameObject.FindWithTag("Canvas").GetComponent<InventoryUI>().inventoryUI.SetActive(false);
            GameObject.FindWithTag("Player").GetComponent<FirstPersonController>().canLook = !GameObject.FindWithTag("Player").GetComponent<FirstPersonController>().canLook; 
            GameObject.FindWithTag("Player").GetComponent<FirstPersonController>().canInteract = !GameObject.FindWithTag("Player").GetComponent<FirstPersonController>().canInteract; 
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        #endregion 
        #region SCPS
        if(name=="SCP-1079")
        {
            GameObject.FindWithTag("Player").GetComponent<Effects>()._candy++;
            GameObject.FindWithTag("Player").GetComponent<Effects>()._candyTimer=GameObject.FindWithTag("Player").GetComponent<Effects>()._candy*15f;
            GameObject.FindWithTag("Player").GetComponent<Effects>()._GO_SCP1079=gameObjectOfItem;
        }
        if(name=="SCP-500")
        {
            GameObject.FindWithTag("Player").GetComponent<Effects>()._cured=true;
            GameObject.FindWithTag("Player").GetComponent<Effects>()._curedTimer=10;
            GameObject.FindWithTag("Player").GetComponent<Effects>()._GO_SCP500=gameObjectOfItem;
        }
        if(name=="SCP-484")
        {
            GameObject.FindWithTag("Player").GetComponent<Effects>()._chilled=true;
            GameObject.FindWithTag("Player").GetComponent<Effects>()._chilledTimer=10;
            GameObject.FindWithTag("Player").GetComponent<Effects>()._GO_SCP484=gameObjectOfItem;
        }
        if(name=="SCP-207")
        {
            GameObject.FindWithTag("Player").GetComponent<Effects>()._energized=true;
            GameObject.FindWithTag("Player").GetComponent<Effects>()._energizedTimer=1000;
            GameObject.FindWithTag("Player").GetComponent<Effects>()._GO_SCP207=gameObjectOfItem;
        }
        if(name=="SCP-198")
        {
            GameObject.FindWithTag("Player").GetComponent<Effects>()._hurt=true;
            GameObject.FindWithTag("Player").GetComponent<Effects>()._hurtTimer=1000;
            GameObject.FindWithTag("Player").GetComponent<Effects>()._GO_SCP198=gameObjectOfItem;
        }
        if(name=="SCP-198")
        {
            GameObject.FindWithTag("Player").GetComponent<Effects>()._hurt=true;
            GameObject.FindWithTag("Player").GetComponent<Effects>()._hurtTimer=1000;
            GameObject.FindWithTag("Player").GetComponent<Effects>()._GO_SCP198=gameObjectOfItem;
        }
        if(name=="SCP-215" && isUsing==false)
        {
            GameObject.FindWithTag("Player").GetComponent<Effects>()._paranoia=true;
            GameObject.FindWithTag("Player").GetComponent<Effects>()._GO_SCP215=gameObjectOfItem;
            playerAudioSource.PlayOneShot(gameObjectOfItem.GetComponent<ItemPickup>().useClips[0]);
            isUsing=true;
        }
        else
        if(name=="SCP-215" && isUsing==true)
        {
            _TEXT.enabled=false;
            GameObject.FindWithTag("Player").GetComponent<Effects>()._paranoia=false;
            playerAudioSource.PlayOneShot(gameObjectOfItem.GetComponent<ItemPickup>().useClips[0]);
            isUsing=false;
        }
        #endregion
    }
}
