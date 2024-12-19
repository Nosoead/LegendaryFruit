using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.UI;
using static System.String;

public class UIDialogue : UIBase
{
    [SerializeField] private UIDialogueList uiDialogueList;
    [SerializeField] private BossRoomTrigger bossRoomTrigger;

    [SerializeField] private Button btnBack;
    
    [SerializeField] private RawImage imgBg;
    [SerializeField] private RawImage imgNpc1;
    [SerializeField] private RawImage imgNpc2;

    [SerializeField] private Image imgContinue;
    [SerializeField] private Image imgScreenDone;
    
    [SerializeField] private TMP_Text txtName;
    [SerializeField] private TMP_Text txtDialogue;

    [SerializeField] private Button[] btnAnswerList;
    [SerializeField] private TMP_Text[] txtAnswerList;

    [SerializeField] private Color disableColor;
    
    private bool done = false;

    private DialogueData _dialogueData;

    private void Awake()
    {
        for (var i = 0; i < btnAnswerList.Length; i++)
        {
            int idx = i;
            btnAnswerList[i].onClick.AddListener(() => { OnClickBtn(idx); });
        }
        
        btnBack.onClick.AddListener(DialogueDone);
        
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0))
        {
            if(uiDialogueList.gameObject.activeSelf)
                return;
            
            if (done == false)
            {
                DOTween.KillAll();
                txtDialogue.text = _dialogueData.Dialogue;
                LogDone();
            }
            else
            {
                if (_dialogueData.AnswerList == null || _dialogueData.AnswerList.Count == 0)
                {
                    if (_dialogueData.Next != 0)
                    {
                        var nextDialogue =  DialogueManager.Instance.GetDialogueData(_dialogueData.Next);
                        SetDialogue(nextDialogue);
                    }
                    else
                    {
                        DialogueDone();
                    }
                }
            }
        }
    }

    public void SetDialogue(DialogueData dialogue)
    {
        DOTween.KillAll(true);
        
        imgScreenDone.raycastTarget = false;
        
        foreach (var btn in btnAnswerList)
            btn.gameObject.SetActive(false);
        
        _dialogueData = dialogue;
        
        done = false;
        
        imgContinue.color = new Color(imgContinue.color.r, imgContinue.color.g, imgContinue.color.b , 0);
        txtDialogue.text = Empty;
        
        var npc =  DialogueManager.Instance.GetNpcData(_dialogueData.Speaker);
        txtName.text = npc.NpcName;

        SetBg(_dialogueData.Bg);
        SetNpc1(_dialogueData.Npc1);
        SetNpc2(_dialogueData.Npc2);

        float duration = _dialogueData.Dialogue.Length / 20;
        if (_dialogueData.TypingSpeed != 0)
            duration = _dialogueData.TypingSpeed;
        
        //txtDialogue.DOText(_dialogueData.Dialogue, duration).SetEase(Ease.Linear).OnComplete(LogDone); //두트윈 유료버전
        txtDialogue.text = _dialogueData.Dialogue; // 두트윈 없을시
        LogDone(); //두트윈 없을시
    }

    private void LogDone()
    {
        done = true;
        
        if (_dialogueData.AnswerList == null || _dialogueData.AnswerList.Count == 0)
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
                if(t) Destroy(t);
            }

            if (bossRoomTrigger != null)
            {
                Destroy(bossRoomTrigger.gameObject);
            }
            _textures.Clear();
        /*})*/;
    }
    
    public void SetBg(string bg)
    {
        bool bgNull = IsNullOrEmpty(bg);
        imgBg.gameObject.SetActive(!bgNull);
        
        if(bgNull)
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
        
        var npcData =  DialogueManager.Instance.GetNpcData(npc.Code);
        imgNpc1.gameObject.SetActive(true);
        imgNpc1.color = npc.Active ? Color.white : disableColor;

        string portrait = npcData.NpcImage;

        switch (npc.Portrait)
        {
            case 1:
                if(npcData.Portrait1 != Empty)
                    portrait = npcData.Portrait1;
                break;
            case 2:
                if(npcData.Portrait2 != Empty)
                    portrait = npcData.Portrait2;
                break;
            case 3:
                if(npcData.Portrait3 != Empty)
                    portrait = npcData.Portrait3;
                break;
            case 4:
                if(npcData.Portrait4 != Empty)
                    portrait = npcData.Portrait4;
                break;
            case 5:
                if(npcData.Portrait5 != Empty)
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
        
        var npcData =  DialogueManager.Instance.GetNpcData(npc.Code);
        imgNpc2.gameObject.SetActive(true);
        imgNpc2.color = npc.Active ? Color.white : disableColor;

        string portrait = npcData.NpcImage;

        switch (npc.Portrait)
        {
            case 1:
                if(npcData.Portrait1 != Empty)
                    portrait = npcData.Portrait1;
                break;
            case 2:
                if(npcData.Portrait2 != Empty)
                    portrait = npcData.Portrait2;
                break;
            case 3:
                if(npcData.Portrait3 != Empty)
                    portrait = npcData.Portrait3;
                break;
            case 4:
                if(npcData.Portrait4 != Empty)
                    portrait = npcData.Portrait4;
                break;
            case 5:
                if(npcData.Portrait5 != Empty)
                    portrait = npcData.Portrait5;
                break;
        }

        LoadTexture(imgNpc2, portrait);
    }

    private void SetBtn()
    {
        for (var i = 0; i < _dialogueData.AnswerList.Count; i++)
        {
            btnAnswerList[i].gameObject.SetActive(true);
            txtAnswerList[i].text = _dialogueData.AnswerList[i].Text;
        }
    }

    private void OnClickBtn(int idx)
    {
        var next = _dialogueData.AnswerList[idx].Action;
        var nextDialogue =  DialogueManager.Instance.GetDialogueData(next);
        SetDialogue(nextDialogue);
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

        if (filePath.Contains("://") || filePath.Contains(":///"))
        {
            using (WWW www = new WWW(filePath))
            {
                yield return www;
                Texture2D texture = new Texture2D(2, 2);
                www.LoadImageIntoTexture(texture);
                
                rImage.texture = texture;

                if (_textures.ContainsKey(fileName))
                    _textures[fileName] = texture;
                else
                    _textures.Add(fileName, texture);
            }
        }
        else
        {
            byte[] fileData = System.IO.File.ReadAllBytes(filePath);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(fileData);
            rImage.texture = texture;

            if (_textures.ContainsKey(fileName))
                _textures[fileName] = texture;
            else
                _textures.Add(fileName, texture);
        }
    }
}
