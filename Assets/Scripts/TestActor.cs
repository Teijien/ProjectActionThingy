using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestActor : Actor
{
  // Start is called before the first frame update
  public override void Start()
  {
    base.Start();

    blackboard.Add("WorldBounds", new Rect(0, 0, 5, 5));

    m_root = new Repeater(new RandomWalk(this));
  }

  // Update is called once per frame
  public override void Update()
  {
    base.Update();
  }

  private class AlwaysFailTask : Task
  {
    public override TaskStatus execute()
    {
      return TaskStatus.Failure;
    }
  }
}
