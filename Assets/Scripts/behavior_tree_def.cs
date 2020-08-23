// Behavior Tree Node Definitions
// This file defines the types of Nodes found within a behavior tree.


/* The basic implementation of a tree Node/Task
 *    TaskStatus status: The current status of a task. Propagated up behavior
 *      tree from leaf/action tasks to determine behaviors of Sequence,
 *      Selector, and Decorator Tasks.
 *    TaskStatus execute(): The task carried out by a node. ActionTasks will
 *      perform a task whilst CompositeTasks carry out child tasks until
 *      failure. */
public abstract class Task 
{
  protected enum TaskStatus
  {
    Success,
    Failure,
    Runninng
  }

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
 * If one child_task succeeds, propagates success upward. If all fail, propagates failure
 * upward. */
public class SelectorTask : CompositeTask 
{
  public override TaskStatus execute() 
  {
    foreach (Task task in child_tasks) 
    {
      TaskStatus success = task.execute;
      if (success == TaskStatus.Success)
      {
        return TaskStatus.Success;
      }
    }
    return TaskStatus.Failure;
  }
}


/* Carries out each child_task in order until one fails
 * If one child_task fails, propagates failure upwards. If all child_tasks
 * succeed, propagate success upward. */
public class SequenceTask : CompositeTask 
{
  public override TaskStatus execute() 
  {
    foreach (Task task in child_tasks)
    {
      TaskStatus continue_execution = task.execute;
      if (continue_execution != TaskStatus.Success)
      {
        return continue_execution;
      }
    }
    return TaskStatus.Success;
  }
}