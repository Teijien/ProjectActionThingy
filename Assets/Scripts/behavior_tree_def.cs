// Behavior Tree Node Definitions
// This file defines the types of Nodes found within a behavior tree.


using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* The basic implementation of a tree Node/Task
 *    TaskStatus status: The current status of a task. Propagated up behavior
 *      tree from leaf/action tasks to determine behaviors of Sequence,
 *      Selector, and Decorator Tasks.
 *    TaskStatus execute(): The task carried out by a node. ActionTasks will
 *      perform a task whilst CompositeTasks carry out child tasks until
 *      failure. */
public abstract class Task 
{
  public enum TaskStatus { Running, Success, Failure }

  protected TaskStatus status;

  public abstract TaskStatus execute();
}


/* A task that carries more than one child tasks */
public abstract class CompositeTask : Task 
{
  protected Task[] child_tasks;

  public CompositeTask(Task[] child_tasks) 
  {
    this.child_tasks = child_tasks;
  }

  public override abstract TaskStatus execute();
}


/* Carries out each child_task in order until one succeeds or all fail
 *    If one child_task succeeds, propagates success upward. If all fail, propagates failure
 *    upward. */
public class Selector : CompositeTask 
{
  public Selector(Task[] child_tasks) : base(child_tasks) { }

  public override TaskStatus execute() 
  {
    foreach (Task task in child_tasks) 
    {
      TaskStatus success = task.execute();
      if (success == TaskStatus.Success)
      {
        return TaskStatus.Success;
      }
    }
    return TaskStatus.Failure;
  }
}


/* Carries out each child_task in order until one fails
 *    If one child_task fails, propagates failure upwards. If all child_tasks
 *    succeed, propagate success upward. */
public class Sequencer : CompositeTask 
{
  public Sequencer(Task[] child_tasks) : base(child_tasks) { }

  public override TaskStatus execute() 
  {
    foreach (Task task in child_tasks)
    {
      TaskStatus continue_execution = task.execute();
      if (continue_execution != TaskStatus.Success)
      {
        return continue_execution;
      }
    }
    return TaskStatus.Success;
  }
}


/* Basic structure of a modifying node 
 *    This node will only have one child task that it will modify.
 *    Examples of modifications include: always failing/succeeding, inverting the
 *    result (i.e.: fail = success and vice versa), or repeating the execution of
 *    the child node. */
public abstract class Decorator : Task
{
  protected Task child;

  public Decorator(Task child)
  {
    this.child = child;
  }

  public abstract override TaskStatus execute();
}


/* Repeats the child task forever */
public class Repeater : Decorator
{
  public Repeater(Task child) : base(child) { }

  public override TaskStatus execute()
  {
    Debug.Log("Child returned: " + child.execute());
    return TaskStatus.Running;
  }
}