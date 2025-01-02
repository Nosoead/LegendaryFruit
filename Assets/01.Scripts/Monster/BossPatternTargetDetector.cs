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
            StartCoroutine(DelayedInvoke(collision.gameObject));            
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            StartCoroutine(DelayedInvoke(collision.gameObject));
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            isInvoking = false;
        }
    }

    private IEnumerator DelayedInvoke(GameObject player)
    {
        if (isInvoking) yield break;
        isInvoking = true;
        float delay = 0.3f;
        yield return new WaitForSeconds(delay);

        OnPlayerEnterDamageZone?.Invoke(player);
        isInvoking = false;
    }

    public void ResetDetector()
    {
        isInvoking = false;
        StopAllCoroutines();
    }
}
