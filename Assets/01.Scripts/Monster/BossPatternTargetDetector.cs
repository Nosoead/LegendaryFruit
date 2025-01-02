using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BossPatternTargetDetector : MonoBehaviour
{
    public event UnityAction<GameObject> OnPlayerEnterDamageZone;
    private bool isInvoking = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (!isInvoking)
            {
                isInvoking = true;
                OnPlayerEnterDamageZone?.Invoke(collision.gameObject);
                isInvoking = false;

            }
            else
            {
                return;
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player") && !isInvoking)
        { 
            StartCoroutine(DelayedInvoke(collision.gameObject));
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        
    }

    private IEnumerator DelayedInvoke(GameObject player)
    {
        isInvoking = true;
        float delay = 0.5f; // 1초 지연
        yield return new WaitForSeconds(delay);

        OnPlayerEnterDamageZone?.Invoke(player);
        isInvoking = false;
    }
}
