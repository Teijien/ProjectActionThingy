using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Actor : MonoBehaviour
{
  private bool started_behavior;
  private Coroutine behavior;

  protected Task m_root;

  public Dictionary<string, object> blackboard { get; set; }

  // Start is called before the first frame update
  public virtual void Start()
  {
    blackboard = new Dictionary<string, object>();
    started_behavior = false;
  }

  // Update is called once per frame
  public virtual void Update()
  {
    if (!started_behavior)
    {
      behavior = StartCoroutine(RunBehavior());
      started_behavior = true;
    }
  }

  private IEnumerator RunBehavior()
  {
    Task.TaskStatus status = m_root.execute();
    while (status == Task.TaskStatus.Running)
    {
      Debug.Log("Root result: " + status);
      yield return null;
      status = m_root.execute();
    }

    Debug.Log("Behavior has finished with: " + status);
  }
}
