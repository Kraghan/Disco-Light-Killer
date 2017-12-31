using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;
using UnityEngine.UI;

public class Room1Event : MonoBehaviour {
    private bool m_Started = false;
    private bool m_StartedSecond = false;
    private bool m_soundStarted = false;

    private AudioSource m_Light1Audio;
    private AudioSource m_Light2Audio;

    [SerializeField] private Light m_lightSpot;
    [SerializeField] private Light m_lightSpot2;
    [SerializeField] private GameObject m_zombie;
    [SerializeField] private float m_TimeBetweenFlash = 0.2f;
    [SerializeField] private float m_TimeElapsedSinceLastFlash = 0.0f;
    [SerializeField] private uint m_NumberOfFlash = 10;
    [SerializeField] private float m_TimeElapsedInTheDark = 0.0f;
    [SerializeField] private float m_TimeInTheDark = 0.1f;
    [SerializeField] private bool m_InTheDark = false;
    [SerializeField] private float m_LightIntensity = 5;
    [SerializeField] private AudioClip m_AlterLightSound;
    [SerializeField] private AudioClip m_ExplodeSound;
    [SerializeField] private Text m_messageUI;
    [SerializeField] private Transform m_nextRoomStart;
    [SerializeField] private Player m_player;

    private float m_TimeElapsedWithZombie = -1;
    private float m_TimeWithZombie = 1.5f;

    public void Launch()
    {
        m_Started = true;
        m_Light1Audio.clip = m_AlterLightSound;
        m_Light1Audio.Play();
    }

    public void LaunchSecond()
    {
        m_StartedSecond = true;
    }

    void Start ()
    {
        m_Light1Audio = m_lightSpot.GetComponent<AudioSource>();
        m_Light2Audio = m_lightSpot2.GetComponent<AudioSource>();
        m_lightSpot2.gameObject.SetActive(false);
        m_zombie.SetActive(false);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (m_Started)
            ProcessFirstEvents();

        if (m_StartedSecond)
            ProcessSecondEvents(Input.GetButton("FlashLight"));
        
	}

    private void ProcessSecondEvents(bool lightOn)
    {
        if (!m_soundStarted)
        {
            m_Light2Audio.Stop();
            m_Light2Audio.PlayOneShot(m_ExplodeSound);
            m_lightSpot2.intensity = 0;
            m_soundStarted = true;
            m_messageUI.text = "Press \"F\" to switch on the torchlight";
        }

        if(lightOn)
        {
            m_zombie.SetActive(true);
            m_player.setParalized(true);
            m_TimeElapsedWithZombie = 0.0f;
        }

        if (m_TimeElapsedWithZombie != -1)
            m_TimeElapsedWithZombie += Time.deltaTime;

        if (m_TimeElapsedWithZombie >= m_TimeWithZombie)
        {
            m_player.setParalized(false);
            m_player.ToggleLight();
            m_player.gameObject.transform.position = m_nextRoomStart.transform.position;
            m_messageUI.text = "";
            m_player.Hit();

            Destroy(m_zombie);
            Destroy(this);
        }
    }

    private void ProcessFirstEvents()
    {
        if (m_InTheDark)
        {
            m_lightSpot.intensity = 0;
            m_TimeElapsedInTheDark += Time.deltaTime;
            if (m_TimeElapsedInTheDark >= m_TimeInTheDark)
            {
                m_TimeElapsedInTheDark = 0.0f;
                m_InTheDark = false;
                --m_NumberOfFlash;
            }
            else
                return;
        }
        else
            m_lightSpot.intensity = m_LightIntensity;

        if (m_NumberOfFlash != 0)
        {
            m_TimeElapsedSinceLastFlash += Time.deltaTime;
            if (m_TimeElapsedSinceLastFlash >= m_TimeBetweenFlash)
            {
                m_TimeElapsedSinceLastFlash = 0.0f;
                m_TimeBetweenFlash *= 9.0f / 10.0f;
                m_InTheDark = true;
                //m_LightIntensity = m_lightSpot.intensity;

            }
        }
        else
        {
            m_Started = false;
            m_Light1Audio.Stop();
            m_Light1Audio.PlayOneShot(m_ExplodeSound);
            m_lightSpot2.gameObject.SetActive(true);
        }
    }
}
