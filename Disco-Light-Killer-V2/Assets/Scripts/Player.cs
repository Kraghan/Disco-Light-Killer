using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Light m_Light;
    [SerializeField] private GameObject m_GunInventory;
    [SerializeField] private AudioClip m_LightSound;
    public uint m_NumberOfGuns = 5;
    public uint m_ActiveGunIndex;

    private bool m_lightEnabled;
    private Guns[] m_Guns;
    private uint m_NumberOfGunInInventory;
    private AudioSource m_audioSource;

	// Use this for initialization
	void Start () {
        m_NumberOfGunInInventory = 0;
        m_Guns = new Guns[m_NumberOfGuns];
        m_lightEnabled = false;
        m_Light.gameObject.SetActive(false);
        m_audioSource = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (m_NumberOfGunInInventory == 0)
            return;

        if (Input.GetButtonDown("FlashLight"))
            ToggleLight();


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
}
