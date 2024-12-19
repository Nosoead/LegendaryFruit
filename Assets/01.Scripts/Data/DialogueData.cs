using System.Collections.Generic;

public class DialogueData
{
    public int Code;
    public DialogueNpc Npc1;
    public DialogueNpc Npc2;
    public int Speaker;
    public float TypingSpeed;
    public string Bg;
    public string Dialogue;
    public List<string> Dialogues = new List<string>();
    public List<Answer> AnswerList = new List<Answer>();
    public int Next;
}

public class DialogueNpc
{
    public int Code;
    public int Portrait;
    public bool Active;
}

public class Answer
{
    public string Text;
    public int Action;
}