using UnityEngine;
using UnityEngine.UI;

public class TaskItem : MonoBehaviour
{
    public string taskId;

    public Text txtTask;
    public Slider progressSlider;
    public Image image;
    
    public void Populate(LevelTask task, Sprite sprite)
    {
        txtTask.text = task.conditionMessage;
        image.sprite = sprite;
    }
    
    
}
