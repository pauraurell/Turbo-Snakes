using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject snake1;
    public GameObject snake2;
    public GameObject tail1;
    public GameObject tail2;
    public GameObject player1;
    public GameObject player2;
    Vector3 respawnPosSnake1;
    Quaternion respawnRotSnake1;
    Vector3 respawnPosSnake2;
    Quaternion respawnRotSnake2;  
    public bool hasEnded = false;

    private void Start()
    {
        respawnPosSnake1 = new Vector3(-2,2,0);
        respawnRotSnake1 = new Quaternion(0, 0, 180, 0);
        respawnPosSnake2 = new Vector3(2, -2, 0);
        respawnRotSnake2 = new Quaternion(0, 0, 0, 0);
    }

    public void EndGame() 
    {
        StartCoroutine(PlayEndGameAnimation());
    }

    
    IEnumerator PlayEndGameAnimation() 
    {
        yield return new WaitForSeconds(1f);

        snake1.GetComponent<Transform>().position = respawnPosSnake1;
        snake1.GetComponent<Transform>().rotation = respawnRotSnake1;
        snake2.GetComponent<Transform>().position = respawnPosSnake2;
        snake2.GetComponent<Transform>().rotation = respawnRotSnake2;
        tail1.GetComponent<LineRenderer>().SetVertexCount(0);
        tail2.GetComponent<LineRenderer>().SetVertexCount(0);
        //UnityEditor.PrefabUtility.RevertObjectOverride(player1, UnityEditor.InteractionMode.UserAction);
        //UnityEditor.PrefabUtility.RevertObjectOverride(player2, UnityEditor.InteractionMode.UserAction);
        snake1.GetComponent<snake>().speed = 2f;
        snake1.GetComponent<snake>().rotation_speed = 260;
        snake2.GetComponent<snake>().speed = 2f;
        snake2.GetComponent<snake>().rotation_speed = 260;

      /*  Vector2[] vecArray;
        vecArray = new Vector2[]
        {
            new Vector2( 999, 999 ),
            new Vector2( 999, 999 ),
        };

        tail1.GetComponent<EdgeCollider2D>().points = vecArray; 
      */
    }

    public void RespawnSnakes()
    {
        snake1.GetComponent<Transform>().position = respawnPosSnake1;
        snake1.GetComponent<Transform>().rotation = respawnRotSnake1;
        snake2.GetComponent<Transform>().position = respawnPosSnake2;
        snake2.GetComponent<Transform>().rotation = respawnRotSnake2;
        Destroy(tail1.GetComponent<LineRenderer>());
        Destroy(tail2.GetComponent<LineRenderer>());
    }
}
