using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PickUpLight : MonoBehaviour
{
    [SerializeField] private Room1Event m_RoomEvent;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Player player = other.gameObject.GetComponent<Player>();
            player.EnableLight();
            m_RoomEvent.LaunchSecond();
            Destroy(this.gameObject);
        }
    }
}
