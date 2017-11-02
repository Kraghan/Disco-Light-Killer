using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PickUpWeapon : MonoBehaviour {
    [SerializeField] private Guns m_gun;
    [SerializeField] private Room1Event m_RoomEvent;
    
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            Player player = other.gameObject.GetComponent<Player>();
            player.AddGun(m_gun);
            m_RoomEvent.Launch();
            Destroy(this.gameObject);
        }
    }
}
