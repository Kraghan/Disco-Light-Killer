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
    //[SerializeField] private Vector3 m_BulletPopPosition;
    [SerializeField] private GameObject m_Flash;
    [SerializeField] private uint m_NumberOfBulletInBurst;

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
        m_Flash.SetActive(false);
        m_TimeElapsedSinceLastShoot = 0.0f;
        m_TimeBetweenShot = 1.0f / (m_ShotPerMinute / 60.0f);
        m_HasShoot = false;
        m_IsShooting = false;
        m_BulletToShoot = 0;
    }

    // Update is called once per frame
    void Update()
    {
        float rand = Random.Range(1f, 2f);
        m_Flash.transform.localScale = Vector3.one * rand;
        m_Flash.transform.Rotate(new Vector3(0.0f, 0.0f, Random.Range(0.0f, 90.0f)));

        if (m_PreviouslyShoot)
        {
            m_Flash.SetActive(true);
            m_PreviouslyShoot = false;
        }
        else
            m_Flash.SetActive(false);

    }

    private void FixedUpdate()
    {
        m_TimeElapsedSinceLastShoot += Time.fixedDeltaTime;
        if(m_Mode == GunMode.BURST && CanShoot() && m_IsShooting)
        {
            DoShoot();
            m_BulletToShoot--;

            m_IsShooting = m_BulletToShoot != 0 && m_BulletInMagazine != 0;

            if (!m_IsShooting)
                m_HasShoot = true;
        }

        if (!m_IsShooting && Input.GetButtonUp("Fire1"))
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
        m_PreviouslyShoot = true;
        m_BulletInMagazine--;
        m_TimeElapsedSinceLastShoot = 0.0f;
    }

    public bool Reload()
    {
        if (m_BulletTotal == 0 || m_BulletInMagazine == m_BulletMax)
            return false;

        uint bulletReloaded = m_BulletTotal;

        if(m_BulletTotal > m_BulletMax)
            bulletReloaded = m_BulletMax;

        bulletReloaded -= m_BulletInMagazine;

        m_BulletInMagazine += bulletReloaded;
        m_BulletTotal -= bulletReloaded;

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
}