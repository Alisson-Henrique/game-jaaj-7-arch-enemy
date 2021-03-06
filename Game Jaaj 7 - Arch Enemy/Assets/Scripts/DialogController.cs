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
    private Image illustration;
    private int index;

    public string nextScene;

    void Start()
    {
        gameController = FindObjectOfType(typeof(GameController)) as GameController;
        cutsceneController = FindObjectOfType(typeof(CutsceneController)) as CutsceneController;

        DialogTxt.text = "";

        dialog = new Dialog();

        if (!gameController.isDead){
            dialog = gameController.cutcenesDialogs[gameController.curretLevel];
        }
        
        sentences = dialog.sentences;

        if (sentences?.Length > 0)
        {
            index = 0;
            illustration.sprite = dialog.illustration;
            StartCoroutine("ShowDialog");
        }
        else
        {
            illustration.sprite = gameController.GetIlustrationDead();
        }
        
    }

    IEnumerator ShowDialog()
    {
        foreach (char c in sentences[index].ToCharArray()){
            DialogTxt.text += c;
            yield return new WaitForSeconds(0.05f);
        }
        
        yield return new WaitForSeconds(3f);

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
