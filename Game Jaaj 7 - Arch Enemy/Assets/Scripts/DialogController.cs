using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogController : MonoBehaviour
{
    private GameController gameController;
    private CutsceneController cutsceneController;

    [SerializeField]
    private Text DialogTxt;

    [SerializeField]
    private string[] sentences;

    [SerializeField]
    private Dialog dialog;

    [SerializeField]
    private Sprite illustration;
    private int index;

    public string nextScene;

    void Start()
    {
        gameController = FindObjectOfType(typeof(GameController)) as GameController;
        cutsceneController = FindObjectOfType(typeof(CutsceneController)) as CutsceneController;
        DialogTxt.text = "";
        dialog = gameController.cutcenesDialogs[gameController.curretLevel];
        sentences = dialog.sentences;
        index = 0;
        StartCoroutine("ShowDialog");
    }

    IEnumerator ShowDialog()
    {
        illustration = dialog.illustration;

        if (Input.anyKey)
        {
            Debug.Log("A key or mouse click has been detected");
        }

        foreach (char c in sentences[index].ToCharArray()){
            DialogTxt.text += c;
            yield return new WaitForSeconds(0.075f);
        }
        
        yield return new WaitForSeconds(2f);

        nextDialog();
    }
    
    public bool nextDialog()
    {
        if (sentences[index] == DialogTxt.text)
        {
            if(index < sentences.Length - 1)
            {
                index++;
                DialogTxt.text = "";
                StartCoroutine("ShowDialog");
                return true;
            }
            else
            {
                cutsceneController.PlayFadeOut();
                Invoke("Finish", 5f);
            }
        }

        return false;
    }

    public void Finish()
    {
        gameController.NextScene("Gameplay");
    }
}
