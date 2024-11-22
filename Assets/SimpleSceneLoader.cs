using UnityEngine;
using UnityEngine.SceneManagement;
public class SimpleSceneLoader : MonoBehaviour
{
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SceneManager.LoadScene(1);
        }
    }
}
