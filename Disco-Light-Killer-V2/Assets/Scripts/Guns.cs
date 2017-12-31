using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum GunMode
{
    CONTINUOUS,
    BURST,
    SHOT_BY_SHOT
}

public class Guns : MonoBehaviour
{
    [SerializeField] private string m_GunName;
    [SerializeField] private uint m_BulletInMagazine;
    [SerializeField] private uint m_BulletMax;
    [SerializeField] private uint m_BulletTotal;
    [SerializeField] private float m_BulletSpeed;
    [SerializeField] private float m_ShotPerMinute;
    [SerializeField] private GunMode m_Mode;
    [SerializeField] private float m_Knockback;
    [SerializeField] private float m_Range;
    [SerializeField] private Transform m_BulletPopPosition;
    [SerializeField] private GameObject m_Flash;
    [SerializeField] private uint m_NumberOfBulletInBurst;
    [SerializeField] private AudioClip m_gunSound;
    private AudioSource m_source;

    private bool m_PreviouslyShoot;
    private float m_TimeElapsedSinceLastShoot;
    private float m_TimeBetweenShot;
    private bool m_HasShoot;
    private bool m_IsShooting;
    private uint m_BulletToShoot;

    // Use this for initialization
    void Start()
    {
        m_PreviouslyShoot = false;
        if(m_Flash != null)
            m_Flash.SetActive(false);
        m_TimeElapsedSinceLastShoot = 0.0f; 
        m_TimeBetweenShot = 1.0f / (m_ShotPerMinute / 60.0f);
        m_HasShoot = false;
        m_IsShooting = false;
        m_BulletToShoot = 0;
        m_source = GetComponent<AudioSource>(); 
    }

    // Update is called once per frame
    void Update()
    {
        float rand = Random.Range(0.5f, 1f);
        if(m_Flash != null)
        {
            m_Flash.transform.localScale = new Vector3(rand,rand,rand);
            m_Flash.transform.Rotate(new Vector3(Random.Range(0.0f, 90.0f), 0.0f, 0.0f));
            if (m_PreviouslyShoot)
            {
                m_Flash.SetActive(true);
                m_PreviouslyShoot = false;
            }
            else
                m_Flash.SetActive(false);
        }

    }

    private void FixedUpdate()
    {
        if (m_BulletInMagazine == 0)
            Reload();

        m_TimeElapsedSinceLastShoot += Time.fixedDeltaTime;
        if(m_Mode == GunMode.BURST && CanShoot() && m_IsShooting)
        {
            DoShoot();
            m_BulletToShoot--;

            m_IsShooting = m_BulletToShoot != 0 && m_BulletInMagazine != 0;

            if (!m_IsShooting)
                m_HasShoot = true;
        }

        if (!m_IsShooting && !Input.GetButton("Fire"))
            m_HasShoot = false;
    }

    public bool CanShoot()
    {
        return m_BulletInMagazine != 0 && m_TimeBetweenShot <= m_TimeElapsedSinceLastShoot && !m_HasShoot;
    }

    public bool Shoot()
    {

        if (!CanShoot() || m_Mode == GunMode.BURST && m_IsShooting)
            return false;

        if (m_Mode == GunMode.BURST)
        {
            m_IsShooting = true;
            m_BulletToShoot = m_NumberOfBulletInBurst - 1;
        }

        if (m_Mode == GunMode.SHOT_BY_SHOT)
            m_HasShoot = true;

        DoShoot();

        return true;
    }

    private void DoShoot()
    {
        m_source.PlayOneShot(m_gunSound);
        m_PreviouslyShoot = true;
        m_BulletInMagazine--;
        m_TimeElapsedSinceLastShoot = 0.0f;
        Vector3 rayOrigin = Camera.main.ViewportToWorldPoint(new Vector3(.5f, .5f, 0));
        RaycastHit hit;
        if (Physics.Raycast(rayOrigin, Camera.main.transform.forward, out hit, m_Range))
        {
            if(hit.collider.gameObject.CompareTag("enemy"))
            {
                Enemy enemy = hit.collider.gameObject.GetComponent<Enemy>();
                enemy.Hit(hit.collider is SphereCollider);
            }
        }

        Camera.main.transform.localRotation = new Quaternion(Camera.main.transform.localRotation.x + m_Knockback, Camera.main.transform.localRotation.y, Camera.main.transform.localRotation.z, Camera.main.transform.localRotation.w);
    }

    public bool Reload()
    {
        if (m_BulletTotal == 0 || m_BulletInMagazine == m_BulletMax)
            return false;

        uint bulletToReload = m_BulletMax - m_BulletInMagazine;

        if (m_BulletTotal < bulletToReload)
            bulletToReload = m_BulletTotal;

        m_BulletInMagazine += bulletToReload;
        m_BulletTotal -= bulletToReload;

        return true;
    }

    public void AddAmmo(uint ammo)
    {
        m_BulletTotal += ammo;
    }

    public uint EmptyAmmo()
    {
        uint total = m_BulletTotal + m_BulletInMagazine;
        m_BulletInMagazine = 0;
        m_BulletTotal = 0;
        return total;
    }

    public string GetName()
    {
        return m_GunName;
    }

    public string DisplayAmmo()
    {
        return m_BulletInMagazine + " - " + m_BulletTotal;
    }
}