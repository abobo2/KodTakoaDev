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
        TaskItem item =
            GameObject.Instantiate(taskItemPrefab.gameObject, Vector3.zero, Quaternion.identity, taskItemsContainer).GetComponent<TaskItem>();
        item.Populate(startedTask);

        startedTasks.Add(startedTask.interactionToBeDone, item);
    }

    public void OnTaskCompleted(LevelTask completedTask)
    {
        if (startedTasks.ContainsKey(completedTask.interactionToBeDone))
        {
            TaskItem task = startedTasks[completedTask.interactionToBeDone];
            startedTasks.Remove(completedTask.interactionToBeDone);

            Destroy(task.gameObject);
        }
    }
}
