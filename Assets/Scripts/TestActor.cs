using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestActor : MonoBehaviour
{
  private Task m_root;
  private bool started_behavior;
  private Coroutine behavior;

  public Dictionary<string, object> blackboard { get; set; }
  public Task root { get { return m_root; } }

  // Start is called before the first frame update
  void Start()
  {
    blackboard = new Dictionary<string, object>();
    blackboard.Add("WorldBounds", new Rect(0, 0, 5, 5));

    started_behavior = false;

    m_root = new AlwaysFailTask();
  }

  // Update is called once per frame
  void Update()
  {
    if (!started_behavior)
    {
      behavior = StartCoroutine(RunBehavior());
      started_behavior = true;
    }
  }

  private IEnumerator RunBehavior()
  {
    Task.TaskStatus status = root.execute();
    while (status == Task.TaskStatus.Running)
    {
      Debug.Log("Root result: " + status);
      yield return null;
      status = root.execute();
    }

    Debug.Log("Behavior has finished with: " + status);
  }

  private class AlwaysFailTask : Task
  {
    public override TaskStatus execute()
    {
      return TaskStatus.Failure;
    }
  }
}
