using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static System.String;
using UnityEngine.Networking;


public class UIDialogue : UIBase
{
    [SerializeField] private List<GameObject> uiList;
    private UIDialogueList uiDialogueList;
    [SerializeField] private BossRoomTrigger bossRoomTrigger;

    [SerializeField] private Button btnBack;

    [SerializeField] private RawImage imgBg;
    [SerializeField] private RawImage imgNpc1;
    [SerializeField] private RawImage imgNpc2;

    [SerializeField] private Image imgContinue;
    [SerializeField] private Image imgScreenDone;

    [SerializeField] private TMP_Text txtName;
    [SerializeField] private TextMeshProUGUI txtDialogue;

    [SerializeField] private Button[] btnAnswerList;
    [SerializeField] private TMP_Text[] txtAnswerList;

    [SerializeField] private Color disableColor;

    private bool done;
    private bool keyPressed;
    private bool isFirst;

    private DialogueData dialogueData;
    private UIManager uiManager;
    private PlayerInput input;

    private void Awake()
    {
        for (var i = 0; i < btnAnswerList.Length; i++)
        {
            int idx = i;
            btnAnswerList[i].onClick.AddListener(() => { OnClickBtn(idx); });
        }

        btnBack.onClick.AddListener(DialogueDone);
        input = GatherInputManager.Instance.input;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && !keyPressed)
        {
            /*
            if(uiDialogueList.gameObject.activeSelf)
                return;
                */

            keyPressed = true;
            StartCoroutine(HandleDialogue());
        }

        if (Input.GetKeyUp(KeyCode.F))
        {
            keyPressed = false;
        }
    }

    private IEnumerator HandleDialogue()
    {
        
        if (done == false)
        {
            DOTween.KillAll();
            yield return null;
            txtDialogue.text = dialogueData.Dialogue;
            LogDone();
        }
        else
        {
            if (dialogueData.AnswerList == null || dialogueData.AnswerList.Count == 0)
            {
                if (dialogueData.Next != 0)
                {
                    var nextDialogue = DialogueManager.Instance.GetDialogueData(dialogueData.Next);
                    SetDialogue(nextDialogue);
                }
                else
                {
                    input.Player.Enable();
                    DialogueDone();
                }
            }
        }

        yield return null;
    }
    public void SetDialogue(DialogueData dialogue)
    {
        DOTween.KillAll(true);

        imgScreenDone.raycastTarget = false;

        foreach (var btn in btnAnswerList)
            btn.gameObject.SetActive(false);

        dialogueData = dialogue;

        done = false;

        imgContinue.color = new Color(imgContinue.color.r, imgContinue.color.g, imgContinue.color.b, 0);
        txtDialogue.text = Empty;

        var npc = DialogueManager.Instance.GetNpcData(dialogueData.Speaker);
        txtName.text = npc.NpcName;

        SetBg(dialogueData.Bg);
        SetNpc1(dialogueData.Npc1);
        SetNpc2(dialogueData.Npc2);

        float duration = dialogueData.Dialogue.Length / 20;
        if (dialogueData.TypingSpeed != 0)
            duration = dialogueData.TypingSpeed;

        txtDialogue.DOText(dialogueData.Dialogue, duration).SetEase(Ease.Linear).OnComplete(LogDone); //두트윈 유료버전
        //txtDialogue.text = dialogueData.Dialogue; // 두트윈 없을시
        //LogDone(); //두트윈 없을시
        input.Player.Disable();
    }

    private void LogDone()
    {
        done = true;

        if (dialogueData.AnswerList == null || dialogueData.AnswerList.Count == 0)
            imgContinue.DOFade(1, 0.7f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InQuad);
        else
            SetBtn();
       
    }

    private void DialogueDone()
    {
        imgScreenDone.raycastTarget = true;
        /*imgScreenDone.DOFade(1, 1).OnComplete(() =>
        {*/
        this.gameObject.SetActive(false);
        //imgScreenDone.color = new Color(imgScreenDone.color.r, imgScreenDone.color.g, imgScreenDone.color.b , 0);

        foreach (var t in _textures.Values)
        {
            if (t) Destroy(t);
        }

        if (bossRoomTrigger != null)
        {
            Destroy(bossRoomTrigger.gameObject);
        }

        _textures.Clear();
        /*})*/
        ;
    }

    public void SetBg(string bg)
    {
        bool bgNull = IsNullOrEmpty(bg);
        imgBg.gameObject.SetActive(!bgNull);

        if (bgNull)
            return;

        LoadTexture(imgBg, bg);
    }

    private void SetNpc1(DialogueNpc npc)
    {
        if (npc == null || npc.Code == 0)
        {
            imgNpc1.gameObject.SetActive(false);
            return;
        }

        var npcData = DialogueManager.Instance.GetNpcData(npc.Code);
        imgNpc1.gameObject.SetActive(true);
        imgNpc1.color = npc.Active ? Color.white : disableColor;

        string portrait = npcData.NpcImage;

        switch (npc.Portrait)
        {
            case 1:
                if (npcData.Portrait1 != Empty)
                    portrait = npcData.Portrait1;
                break;
            case 2:
                if (npcData.Portrait2 != Empty)
                    portrait = npcData.Portrait2;
                break;
            case 3:
                if (npcData.Portrait3 != Empty)
                    portrait = npcData.Portrait3;
                break;
            case 4:
                if (npcData.Portrait4 != Empty)
                    portrait = npcData.Portrait4;
                break;
            case 5:
                if (npcData.Portrait5 != Empty)
                    portrait = npcData.Portrait5;
                break;
        }

        LoadTexture(imgNpc1, portrait);
    }

    private void SetNpc2(DialogueNpc npc)
    {
        if (npc == null || npc.Code == 0)
        {
            imgNpc2.gameObject.SetActive(false);
            return;
        }

        var npcData = DialogueManager.Instance.GetNpcData(npc.Code);
        imgNpc2.gameObject.SetActive(true);
        imgNpc2.color = npc.Active ? Color.white : disableColor;

        string portrait = npcData.NpcImage;

        switch (npc.Portrait)
        {
            case 1:
                if (npcData.Portrait1 != Empty)
                    portrait = npcData.Portrait1;
                break;
            case 2:
                if (npcData.Portrait2 != Empty)
                    portrait = npcData.Portrait2;
                break;
            case 3:
                if (npcData.Portrait3 != Empty)
                    portrait = npcData.Portrait3;
                break;
            case 4:
                if (npcData.Portrait4 != Empty)
                    portrait = npcData.Portrait4;
                break;
            case 5:
                if (npcData.Portrait5 != Empty)
                    portrait = npcData.Portrait5;
                break;
        }

        LoadTexture(imgNpc2, portrait);
    }

    private void SetBtn()
    {
        for (var i = 0; i < dialogueData.AnswerList.Count; i++)
        {
            //uiList[i].gameObject.SetActive(true);
            btnAnswerList[i].gameObject.SetActive(true);
            txtAnswerList[i].text = dialogueData.AnswerList[i].Text;
        }
    }

    private void OnClickBtn(int idx)
    {
        var answer = dialogueData.AnswerList[idx];
        if (!string.IsNullOrEmpty(answer.UIResource))
        {
            SoundManagers.Instance.PlaySFX(SfxType.UIButton);
            if (answer.UIResource == "WeaponUpgradeUI" && answer.Text == "강화하기")
            {
                UIManager.Instance.ToggleUI<WeaponUpgradeUI>(true);
                //if (dialogueData.AnswerList == null || dialogueData.AnswerList.Count == 0);
            }

            if (answer.UIResource == "NpcUpgradeUI" && answer.Text == "강화하기")
            {
                UIManager.Instance.ToggleUI<NpcUpgradeUI>(true);
                //if (dialogueData.AnswerList == null || dialogueData.AnswerList.Count == 0);
            }
        }

        if (answer.Action != 0)
        {
            var nextDialogue = DialogueManager.Instance.GetDialogueData(answer.Action);
            SetDialogue(nextDialogue);
        }
    }

    private Dictionary<string, Texture> _textures = new Dictionary<string, Texture>();

    private void LoadTexture(RawImage rImage, string fileName)
    {
        if (_textures.ContainsKey(fileName))
        {
            rImage.texture = _textures[fileName];
        }
        else
        {
            StartCoroutine(LoadImage(rImage, fileName));
        }
    }

    private IEnumerator LoadImage(RawImage rImage, string fileName)
    {
        string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, fileName);

        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(filePath))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError ||
                request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"Failed to load texture: {request.error}");
            }
            else
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(request);
                rImage.texture = texture;

                if (_textures.ContainsKey(fileName))
                    _textures[fileName] = texture;
                else
                    _textures.Add(fileName, texture);
            }
        }
    }
}