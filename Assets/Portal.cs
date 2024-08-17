using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public Portal linkedPortal;
    public bool isActive = true;
    private bool isCoolingDown = false;
    public float cooldownTime = 0.5f;

    private void OnTriggerEnter2D(Collider2D other) {
        if((other.CompareTag("Player") || other.CompareTag("Moveable")) && isActive && !isCoolingDown && linkedPortal != null) {
            StartCoroutine(TeleportObject(other.gameObject));
        }
    }

    private IEnumerator TeleportObject(GameObject obj) {
        isActive = false;
        linkedPortal.isActive = false;
        
        obj.transform.position = linkedPortal.transform.position;

        isCoolingDown = true;
        yield return new WaitForSeconds(cooldownTime);
        isCoolingDown = false;

        isActive = true;
        linkedPortal.isActive = true;
    }
}
