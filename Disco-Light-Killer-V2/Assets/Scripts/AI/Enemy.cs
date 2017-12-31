using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    [SerializeField]
    private Transform m_playerTransform;
    Animator m_monsterAnimator;
    [SerializeField]
    private float m_distanceBeforeTriggerWalk = 10;
    [SerializeField]
    private float m_distanceBeforeTriggerAttack = 1.5f;
    [SerializeField]
    private float m_speed = 0.01f;
    [SerializeField]
    private int m_hitPoint = 5;
    [SerializeField]
    private AudioClip[] m_sons;
    private AudioSource m_source;

    private float m_timeElapsedSinceLastSound;
    private float m_timeBeforeNextSound;

    // Use this for initialization
    void Start () {
        m_monsterAnimator = GetComponent<Animator>();
        m_source = GetComponent<AudioSource>();
        m_timeElapsedSinceLastSound = 0;
        SetTimeBeforeNextSound();
    }
	
	// Update is called once per frame
	void Update () {
        if (m_hitPoint <= 0)
        {
            m_monsterAnimator.SetBool("Alive", false);
            Collider[] colliders = GetComponents<Collider>();
            for (int i = 0; i < colliders.Length; ++i)
                Destroy(colliders[i]);
            return;
        }
        else
            m_monsterAnimator.SetBool("Alive", true);

        m_timeElapsedSinceLastSound += Time.deltaTime;

        if (Vector3.Distance(m_playerTransform.position, transform.position) < m_distanceBeforeTriggerWalk)
        {
            if (m_timeElapsedSinceLastSound >= m_timeBeforeNextSound)
            {
                m_source.PlayOneShot(m_sons[Random.Range(0, m_sons.Length - 1)]);
                m_timeElapsedSinceLastSound = 0;
                SetTimeBeforeNextSound();
            }

            Vector3 direction = m_playerTransform.position - transform.position;
            direction.y = 0;

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 0.1f);

            m_monsterAnimator.SetBool("Attack", false);
            if (direction.magnitude < m_distanceBeforeTriggerAttack)
            {
                m_monsterAnimator.SetBool("Attack", true);
            }
            else
                transform.Translate(0, 0, m_speed);
        }
	}

    private void SetTimeBeforeNextSound()
    {
        m_timeBeforeNextSound = Random.Range(7,12);
    }

    public void Hit(bool critical)
    {
        m_hitPoint--;
        if (critical)
        {
            m_hitPoint -= 2;
        }

        if (m_hitPoint < 0)
            m_hitPoint = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Player>().Hit();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Player>().Hit();
        }
    }

    public bool IsDead()
    {
        return m_hitPoint == 0;
    }
        
}
