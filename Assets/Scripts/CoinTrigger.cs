using System;
using UnityEngine;
// remember to attach to the coin specifically 
public class CoinTrigger : MonoBehaviour {
    private void OnTriggerEnter(Collider other)
    {
        GameEvents.OnCoinCollected.Invoke(transform.position);
        transform.parent.gameObject.SetActive(false);
    }
    void Start()
    {
        transform.parent.gameObject.SetActive(true);
    }
}
