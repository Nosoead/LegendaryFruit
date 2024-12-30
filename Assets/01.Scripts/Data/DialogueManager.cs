using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;


public class DialogueManager : Singleton<DialogueManager>
{
  
    private UIDialogue uidialogue;
    private int loadData = 0;

    public bool IsLoad => loadData >= 2;
    
    private void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        StartCoroutine(LoadCSV("Npc.csv"));
        StartCoroutine(LoadCSV("Dialogue.csv"));
        StartCoroutine(LoadCSV("DialogueList.csv"));
    }

    private Dictionary<int, NpcData> _npcDb = new Dictionary<int, NpcData>();
    private Dictionary<int, DialogueData> _dialogueDb = new Dictionary<int, DialogueData>();
    private Dictionary<int, DialogueList> _dialogueListDb = new Dictionary<int, DialogueList>();

    public void SetUIDialogue(UIDialogue dialogue)
    {
        uidialogue = dialogue;
    }
    IEnumerator LoadCSV(string fileName)
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);
    
        string result;
        if (filePath.Contains("://") || filePath.Contains(":///"))
        {
            // Web 요청을 통해 파일 로드 (WebGL 및 Android의 경우)
            UnityWebRequest www = new UnityWebRequest(filePath);
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();
            result = www.downloadHandler.text;
        }
        else
        {
            // 로컬 파일 로드
            result = File.ReadAllText(filePath);
        }
    
        StringReader reader = new StringReader(result);
        List<Dictionary<string, string>> data = new List<Dictionary<string, string>>();
    
        // 첫 번째 줄은 헤더
        string[] headers = reader.ReadLine().Split(',');
    
        while (reader.Peek() != -1)
        {
            string line = reader.ReadLine();
            if (line == null) break;
            string[] fields = line.Split(',');
    
            Dictionary<string, string> entry = new Dictionary<string, string>();
            for (int i = 0; i < headers.Length; i++)
            {
                entry[headers[i]] = fields[i];
            }
    
            data.Add(entry);
        }
    
        switch (fileName)
        {
            case "Npc.csv":
                SetNpcTable(data);
                break;
                
            case "Dialogue.csv":
                SetDialogueTable(data);
                break;
            
            case "DialogueList.csv":
                SetDialogueListTable(data);
                break;
        }
    
        loadData++;
    }

    private void SetNpcTable(List<Dictionary<string, string>> data)
    {
        for (int j = 0; j < data.Count; j++)
        {
            var entry = data[j];
            
            var npc = new NpcData();
            
            int.TryParse(entry["code"], out npc.Code);
            npc.NpcName = entry["npcName"];
            npc.NpcImage = entry["npcImage"];
            npc.Portrait1 = entry["portrait1"];
            npc.Portrait2 = entry["portrait2"];
            npc.Portrait3 = entry["portrait3"];
            npc.Portrait4 = entry["portrait4"];
            npc.Portrait5 = entry["portrait5"];
            
            _npcDb.Add(npc.Code, npc);
        }
    }
    
    private void SetDialogueTable(List<Dictionary<string, string>> data)
    {
        for (int j = 0; j < data.Count; j++)
        {
            var entry = data[j];
            
            var dialogue = new DialogueData();
            dialogue.Dialogue = entry["dialogue"];
            dialogue.Bg = entry["bg"];
            int.TryParse(entry["code"], out dialogue.Code);
            int.TryParse(entry["next"], out dialogue.Next);
            int.TryParse(entry["speaker"], out dialogue.Speaker);
            float.TryParse(entry["typingSpeed"], out dialogue.TypingSpeed);

            dialogue.UIResource = entry["UIResource"];
         
            var npc1 = new DialogueNpc();
            int.TryParse(entry["npc1"], out npc1.Code);
            int.TryParse(entry["npc1Portrait"], out npc1.Portrait);
            bool.TryParse(entry["npc1Active"], out npc1.Active);
            
            var npc2 = new DialogueNpc();
            int.TryParse(entry["npc2"], out npc2.Code);
            int.TryParse(entry["npc2Portrait"], out npc2.Portrait);
            bool.TryParse(entry["npc2Active"], out npc2.Active);

            dialogue.Npc1 = npc1;
            dialogue.Npc2 = npc2;
            
            string answer1 = entry["answer1"];
            if (string.IsNullOrEmpty(answer1) == false)
            {
                var answer = new Answer()
                {
                    Text = answer1,
                    UIResource = entry["UIResource"]
                };
                int.TryParse(entry["answer1Action"], out answer.Action);
                dialogue.AnswerList.Add(answer);
            }
            
            string answer2 = entry["answer2"];
            if (string.IsNullOrEmpty(answer2) == false)
            {
                var answer = new Answer()
                {
                    Text = answer2,
                    UIResource = entry["UIResource"]
                };
                int.TryParse(entry["answer2Action"], out answer.Action);
                dialogue.AnswerList.Add(answer);
            }
            
            string answer3 = entry["answer3"];
            if (string.IsNullOrEmpty(answer3) == false)
            {
                var answer = new Answer()
                {
                    Text = answer3,
                    UIResource = entry["UIResource"]
                };
                int.TryParse(entry["answer3Action"], out answer.Action);
                dialogue.AnswerList.Add(answer);
            }
            
            string answer4 = entry["answer4"];
            if (string.IsNullOrEmpty(answer4) == false)
            {
                var answer = new Answer()
                {
                    Text = answer4,
                    UIResource = entry["UIResource"]
                };
                int.TryParse(entry["answer4Action"], out answer.Action);
                dialogue.AnswerList.Add(answer);
            }
            CreateDialogueUI(dialogue);
            _dialogueDb.Add(dialogue.Code, dialogue);
        }
    }

    private void CreateDialogueUI(DialogueData dialogue)
    {
        string uiResourceName = dialogue.UIResource;
        if (!string.IsNullOrEmpty(uiResourceName))
        {
            GameObject uiObject = Resources.Load<GameObject>(uiResourceName);
            if (uiObject != null)
            {
                Instantiate(uiObject);
            }
        }
    }

    private void SetDialogueListTable(List<Dictionary<string, string>> data)
    {
        for (int j = 0; j < data.Count; j++)
        {
            var entry = data[j];
            
            var dList = new DialogueList();
            
            int.TryParse(entry["code"], out dList.Code);
            int.TryParse(entry["startDialogue"], out dList.StartDialogue);
            dList.Title = entry["title"];

            _dialogueListDb.Add(dList.Code, dList);
        }
    }
    
    public NpcData GetNpcData(int code)
    {
        return _npcDb.GetValueOrDefault(code);
    }
    
    public DialogueData GetDialogueData(int code)
    {
        return _dialogueDb.GetValueOrDefault(code);
    }

    public Dictionary<int, DialogueList>.Enumerator GetDialogueList()
    {
        return _dialogueListDb.GetEnumerator();
    }

    public DialogueList GetDialogueListData(int code)
    {
        return _dialogueListDb.GetValueOrDefault(code);
    }
}