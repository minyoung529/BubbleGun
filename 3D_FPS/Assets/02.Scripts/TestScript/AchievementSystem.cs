using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementSystem : MonoBehaviour
{
    public Text achievementText;
    
    public void UnLockAchievement(string title)
    {
        achievementText.text = $"도전 과제 해제 : {title}";
    }
}
