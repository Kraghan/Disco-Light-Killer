using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private Light m_Light;
    [SerializeField] private GameObject m_GunInventory;
    [SerializeField] private Text m_bulletUI;
    [SerializeField] private Text m_LifeUI;
    public uint m_NumberOfGuns = 5;
    public uint m_ActiveGunIndex;

    private bool m_lightEnabled;
    private Guns[] m_Guns;
    private uint m_NumberOfGunInInventory;
    private AudioSource m_audioSource;
    private bool m_paralized = false;
    private RigidbodyFirstPersonController m_controller;
    private Quaternion m_playerRotation;
    private Quaternion m_cameraRotation;
    private int m_hitPoint = 10;
    private float m_TimeElapsedSinceLastHit;
    private float m_TimeBetweenHit = 1.5f;
    [SerializeField]
    private Text m_messageBox;
    private float m_timeElapsed;
    private float m_timeBeforeLeave = 3;

    // Use this for initialization
    void Start () {
        m_NumberOfGunInInventory = 0;
        m_Guns = new Guns[m_NumberOfGuns];
        m_lightEnabled = false;
        m_Light.gameObject.SetActive(false);
        m_audioSource = GetComponent<AudioSource>();
        m_controller = GetComponent<RigidbodyFirstPersonController>();
        m_TimeElapsedSinceLastHit = m_TimeBetweenHit;
        m_LifeUI.text = m_hitPoint + " / 10";
        m_timeElapsed = 0;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(m_hitPoint == 0)
        {
            setParalized(true);
            m_messageBox.text = "You are dead...";
            m_timeElapsed += Time.deltaTime;
            if (m_timeBeforeLeave <= m_timeElapsed)
                Application.Quit();

        }
        
        m_TimeElapsedSinceLastHit += Time.deltaTime;

        if (m_NumberOfGunInInventory == 0)
        {
            m_bulletUI.text = "";
            return;
        }

        m_bulletUI.text = m_Guns[m_ActiveGunIndex].DisplayAmmo();

        if (m_paralized)
        {
            transform.rotation = m_cameraRotation;
            Camera.main.transform.rotation = m_cameraRotation;
            return;
        }

        m_playerRotation = transform.rotation;
        m_cameraRotation = Camera.main.transform.rotation;

        if (Input.GetButtonDown("FlashLight"))
            ToggleLight();

        if (Input.GetButtonDown("Reload"))
            m_Guns[m_ActiveGunIndex].Reload();

        if (Input.GetButton("Fire"))
        {
            m_Guns[m_ActiveGunIndex].Shoot();
        }
    }

    public void AddGun(Guns gun)
    {
        // Check if gun is in inventory
        for(uint i = 0; i < m_NumberOfGunInInventory; ++i)
        {
            if (m_Guns[i].GetName() == gun.GetName())
            {
                m_Guns[i].AddAmmo(gun.EmptyAmmo());
                return;
            }
        }

        GameObject newGun = Instantiate<GameObject>(gun.gameObject, m_GunInventory.transform);
        m_Guns[m_NumberOfGunInInventory] = newGun.GetComponent<Guns>();
        ++m_NumberOfGunInInventory;
    }

    public void EnableLight()
    {
        m_lightEnabled = true;
    }

    public void ToggleLight()
    {
        if (m_lightEnabled)
        {
            m_Light.gameObject.SetActive(!m_Light.gameObject.active);

            m_audioSource.pitch = m_Light.gameObject.active ? 1.0f : 1.2f;
            m_audioSource.Play();
        }
    }

    public void setParalized(bool para)
    {
        m_paralized = para;
    }

    public void Hit()
    {
        if(m_TimeElapsedSinceLastHit >= m_TimeBetweenHit)
        {
            m_hitPoint--;
            m_TimeElapsedSinceLastHit = 0;
            m_LifeUI.text = m_hitPoint + " / 10";

            if (m_hitPoint < 0)
                m_hitPoint = 0;
        }
    }

    public int GetHitPoint()
    {
        return m_hitPoint;
    }
}
