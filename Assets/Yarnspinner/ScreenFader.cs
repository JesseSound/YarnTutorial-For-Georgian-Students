using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Yarn;
using System.Collections;
using Yarn.Unity;
using static UnityEditor.FilePathAttribute;

public class ScreenFader : MonoBehaviour
{
    public Image fadeImage; // Assign your Image component in the Inspector
    
    public float fadeDuration = 1f; // Duration of the fade
    DialogueRunner dialogueRunner;
    
    private static ScreenFader instance;

    void Awake()
    {
        // Ensure there's only one instance
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Optional: Keep across scenes
        }
        else
        {
            Destroy(gameObject);
        }

        //command handlers
        // find the Dialogue Runner
        dialogueRunner = FindObjectOfType<Yarn.Unity.DialogueRunner>();
        
        //add our fade out function to it
        dialogueRunner.AddCommandHandler<int>("fade_out", FadeOut);

      
        fadeImage.enabled= false; // enables us to click continue!
    }

    void Start()
    {
        // Start with the screen fully transparent
        fadeImage.color = new Color(0, 0, 0, 0);
    }
    private void Update()
    {
        
    }


    public static IEnumerator FadeOut(int level) //level is what level you want to go to
    {
        if (instance == null) yield break; // Safety check
       instance.fadeImage.enabled = true; //make black screen visible
        float elapsedTime = 0f;
        Color color = instance.fadeImage.color;

        while (elapsedTime < instance.fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Clamp01(elapsedTime / instance.fadeDuration); // Fade to black
            instance.fadeImage.color = color;
            yield return null;
        }

        // Ensure the image is fully black at the end
        color.a = 1f;
        instance.fadeImage.color = color;
        yield return new WaitForSeconds(1.0f);
       
        SceneManager.LoadScene(level);
    }
}
