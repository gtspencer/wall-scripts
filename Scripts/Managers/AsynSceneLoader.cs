using System;
using System.Collections;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class AsynSceneLoader : MonoBehaviour
{
    public static AsynSceneLoader Instance;

    public enum Scenes
    {
        MainScene,
        EndCorridor,
        
        // Room Path Rooms
        RoomPath, // ground/tunnels
        CrowdedRoom,
        GiveUpControlRoom,
        StageRoom,
        TreeRoom,
        CreationRoom,
        CorpoRoom,
        VacuumVerse,
    }
    
    private void Awake()
    {
        if (Instance != null) Destroy(this.gameObject);

        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
    
    public string sceneName = "SampleScene"; // Name of the scene to load
    public Action OnSceneLoaded = () => { };
    
    public IEnumerator TempLoad()
    {
        Debug.LogError("10");
        yield return new WaitForSeconds(1f);
        
        Debug.LogError("9");
        yield return new WaitForSeconds(1f);
        
        Debug.LogError("8");
        yield return new WaitForSeconds(1f);
        
        Debug.LogError("7");
        yield return new WaitForSeconds(1f);
        
        Debug.LogError("6");
        yield return new WaitForSeconds(1f);
        
        Debug.LogError("5");
        yield return new WaitForSeconds(1f);
        
        Debug.LogError("4");
        yield return new WaitForSeconds(1f);
        
        Debug.LogError("3");
        yield return new WaitForSeconds(1f);
        
        Debug.LogError("2");
        yield return new WaitForSeconds(1f);
        
        Debug.LogError("1");
        yield return new WaitForSeconds(1f);
        
        StartCoroutine(LoadSceneCoroutine(sceneName));
    }
    
    public void LoadScene(AsynSceneLoader.Scenes sceneName)
    {
        StartCoroutine(LoadSceneCoroutine(sceneName.ToString()));
    }

    // Coroutine to load the scene asynchronously
    private IEnumerator LoadSceneCoroutine(string sceneToLoad)
    {
        // Start loading the scene asynchronously
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);

        // While the scene is still loading, you can optionally do something (e.g., show a loading bar)
        while (!asyncOperation.isDone)
        {
            // Here you can use asyncOperation.progress to show progress if needed
            float progress = Mathf.Clamp01(asyncOperation.progress / 0.9f); // Progress goes from 0 to 1

            // You can update a loading bar or perform other tasks here (if desired)
            Debug.Log("Loading Progress: " + progress * 100 + "%");

            yield return null; // Wait until the next frame
        }

        // Scene is fully loaded
        Debug.Log("Scene loaded successfully!");
        OnSceneLoaded.Invoke();
    }

    public void UnloadIntroScene()
    {
        Mainmenu.Instance.EnableMenu();
        // Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.UnloadSceneAsync("IntroScene");
    }

    public void UnloadScene(AsynSceneLoader.Scenes sceneName)
    {
        SceneManager.UnloadSceneAsync(sceneName.ToString());
    }
}
