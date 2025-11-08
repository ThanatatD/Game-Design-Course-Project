using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverPanel : MonoBehaviour
{
    public void PlayAgainClick()
    {
        GameManager.Instance.RestartGame();
    }
    public void ExitClick()
    {
        GameManager.Instance.ExitGame();
    }
    public void PreviousLevelClick()
    {
        GameManager.Instance.LoadSceneFromIndex(-1);
    }
}
