using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class TestPlayerScript : MonoBehaviour
{
    Vector2 moveX;
    private float moveSpeed = 3.0f;

    IInteractable interactable1 = null;

    // Update is called once per frame
    private void Update()
    {
        if(Input.GetKey(KeyCode.LeftArrow))
        {
            this.transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
        }
        else if(Input.GetKey(KeyCode.RightArrow))
        {
            this.transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.F))
        {
            Debug.Log("눌림");
            interactable1.Interact(true,false);
        }   
        else if (Input.GetKeyDown(KeyCode.F))
        {
            interactable1?.Interact(false,true);

        }
    }

    // 일단 플레이어에서 닿으면 UIManager에서 UI꺼내줌
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(GameManager.Instance.isClear && GameManager.Instance.isCreatReward)
        {
            if (collision.CompareTag("RewardTree"))
            {
                UIManager.Instance.ToggleUI<UItest3>(false);
                interactable1 = collision.gameObject.GetComponentInChildren<IInteractable>();
            }
        }
        else { return; }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(GameManager.Instance.isClear && GameManager.Instance.isCreatReward)
        {
            if (collision.CompareTag("RewardTree"))
            {
                UIManager.Instance.ToggleUI<UItest3>(false);
                interactable1 = null;
            }
        }
        else { return;}
    }
}
