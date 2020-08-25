/* Roaming Behavior for Testing Behavior Tree
 *    Tested with 2D sprite in 3D space: Currently does not roam in z
 *    axis */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomWalk : Task
{
  private Vector3 NextDestination { get; set; }
  private TestActor actor;
  public float speed = 3.0f;

  public RandomWalk(TestActor actor)
  {
    this.actor = actor;
    NextDestination = Vector3.zero;
    FindNextDestination();
  }

  public bool FindNextDestination()
  {
    object o;
    bool found = false;
    found = actor.blackboard.TryGetValue("WorldBounds", out o);
    if (found)
    {
      Rect bounds = (Rect)o;
      float x = UnityEngine.Random.value * bounds.width;
      float z = UnityEngine.Random.value * bounds.height;
      NextDestination = new Vector3(x, NextDestination.y, z);
    }

    return found;
  }

  public override TaskStatus execute()
  {
    if (actor.transform.position == NextDestination)
    {
      if (!FindNextDestination())
        return TaskStatus.Failure;
      else
        return TaskStatus.Success;
    }
    else
    {
      actor.transform.position =
        Vector3.MoveTowards(actor.transform.position, NextDestination,
        Time.deltaTime * speed);

      return TaskStatus.Running;
    }
  }
}
