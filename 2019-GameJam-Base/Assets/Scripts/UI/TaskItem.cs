using UnityEngine;
using UnityEngine.UI;

public class TaskItem : MonoBehaviour
{
    public string taskId;

    public Text txtTask;
    public Slider progressSlider;

    public void Populate(LevelTask task)
    {
        txtTask.text = task.conditionMessage;
    }
}
