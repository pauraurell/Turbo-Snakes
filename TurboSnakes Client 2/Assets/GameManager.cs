using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private bool hasEnded = false;
    public void EndGame() 
    {
        if (hasEnded) 
        { 
            return; 
        }

        hasEnded = true;

        StartCoroutine(PlayEndGameAnimation());
    }

    IEnumerator PlayEndGameAnimation() 
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
