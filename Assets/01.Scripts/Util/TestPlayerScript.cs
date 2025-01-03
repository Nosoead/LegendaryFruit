using UnityEngine;

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
            interactable1.Interact(true,false);
        }   
    }

    // 현재 플레이어에게 해당하는 UI를 UIManager에서 관리
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
        if(collision.CompareTag("Potal"))
        {
            Debug.Log("닿음");
            interactable1 = collision.gameObject.GetComponentInChildren<IInteractable>();
            Debug.Log("눌림");
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(GameManager.Instance.isClear && GameManager.Instance.isCreatReward)
        {
            if (collision.CompareTag("RewardTree") || collision.CompareTag("Potal"))
            {
                UIManager.Instance.ToggleUI<UItest3>(false);
                interactable1 = null;
            }
        }
        else { return;}
    }
}
