using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleController : MonoBehaviour {
    public string startLevel;

	private void Start () {
        StartCoroutine(LoadGame(startLevel));
    }

    private IEnumerator LoadGame(string strLevel)
    {
        Scene scene = SceneManager.GetSceneByName(strLevel);
        if (scene.name != strLevel) 
        {
            yield return SceneManager.LoadSceneAsync(strLevel, LoadSceneMode.Additive);
            Debug.Log("Level Load complete.");
        } else
        {
            Debug.Log("Scene already loaded.");
        }
        GameController controller = FindObjectOfType<GameController>();
        controller.InitGame(strLevel);
    }
}
