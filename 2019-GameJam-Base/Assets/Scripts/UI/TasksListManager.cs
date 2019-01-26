using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TasksListManager : MonoBehaviour
{
    public RectTransform taskItemsContainer;
    public TaskItem taskItemPrefab;

    private Dictionary<Interactable, TaskItem> startedTasks = new Dictionary<Interactable, TaskItem>();

    public void OnNewTaskStarted(LevelTask startedTask)
    {
        Debug.Log(startedTask.interactionToBeDone + " " + startedTask.conditionMessage);

        TaskItem item =
            GameObject.Instantiate(taskItemPrefab.gameObject, Vector3.zero, Quaternion.identity, taskItemsContainer).GetComponent<TaskItem>();
        item.Populate(startedTask);

        startedTasks.Add(startedTask.interactionToBeDone, item);
    }

    public void OnTaskCompleted(LevelTask completedTask)
    {

    }
}
