using System.Threading.Tasks;

public static class TaskExtensions {
  public static bool isReadyToStart(this Task task) {
    return task.Status == TaskStatus.Created;
  }

  public static bool isFailed(this Task task) {
    return task.Status == TaskStatus.Faulted;
  }

  public static bool isFinished(this Task task) {
    switch(task.Status) {
      case TaskStatus.Canceled:
      case TaskStatus.RanToCompletion:
      case TaskStatus.Faulted:
        return true;
    }
    return false;
  }

  public static bool isActive(this Task task) {
    switch (task.Status) {
      case TaskStatus.Running:
      case TaskStatus.WaitingForChildrenToComplete:
      case TaskStatus.WaitingToRun:
        return true;
    }
    return false;
  }
}
