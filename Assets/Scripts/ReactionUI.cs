using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ReactionUI : MonoBehaviour
{
    public string reaction;
    public TextMeshProUGUI text;
    
    public void Click()
    {
        ReactionMakeManager.me.SetTextInput(reaction);
    }
    // Update is called once per frame
    void Update()
    {
        text.text = reaction;
    }
}
