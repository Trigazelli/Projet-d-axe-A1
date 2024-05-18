using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] private Text _nameText;
    [SerializeField] private Text _dialogueText;

    private static DialogueManager _instance;
    public static DialogueManager Instance {  get { return _instance; } }

    private Queue<string> _sentences;


    private void Awake()
    {
        if (_instance == null) _instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        _canvas.enabled = false;
        _sentences = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        ControllerDisabler.Instance.InDialogue = true;
        _nameText.text = dialogue.name;
        _sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            _sentences.Enqueue(sentence);
        }
        _canvas.enabled = true;
        DisplayNextSentence();
    }

    public bool DisplayNextSentence()
    {
        if (_sentences.Count == 0)
        {
            EndDialogue();
            return false;
        }

        string sentence = _sentences.Dequeue();
        _dialogueText.text = sentence;
        return true;
    }

    private void EndDialogue()
    {
        Debug.Log("End of dialogue");
        _canvas.enabled = false;
        ControllerDisabler.Instance.InDialogue = false;
    }
}
