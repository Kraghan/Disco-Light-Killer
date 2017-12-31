using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpAmmo : MonoBehaviour {
    private Guns gun;

    private void Start()
    {
        gun = GetComponent<Guns>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Player player = other.gameObject.GetComponent<Player>();
            player.AddGun(gun);
            Destroy(this.gameObject);
        }
    }
}
