using System;

namespace EventDrivenWorkflow
{
    class Program
    {
        static void Main(string[] args)
        {
            var workflowManager = new WorkflowManager();

            var userTask = new UserTask("Task 1");
            var dataValidationTask = new DataValidationTask("Task 2");
            var databaseTask = new DatabaseTask("Task 3");

            userTask.Completed += dataValidationTask.OnUserTaskCompleted;
            dataValidationTask.Validated += databaseTask.OnDataValidationTaskCompleted;

            workflowManager.Execute(userTask);

            Console.ReadKey();
        }
    }

    public class WorkflowManager
    {
        public void Execute(ITask task)
        {
            Console.WriteLine($"Starting {task.Name}");

            task.Execute();
        }
    }

    public interface ITask
    {
        string Name { get; }

        void Execute();
    }

    public class UserTask : ITask
    {
        public string Name { get; private set; }

        public event EventHandler Completed;

        public UserTask(string name)
        {
            Name = name;
        }

        public void Execute()
        {
            Console.WriteLine($"{Name} is executing");

            OnCompleted();
        }

        protected virtual void OnCompleted()
        {
            Completed?.Invoke(this, EventArgs.Empty);
        }
    }

    public class DataValidationTask : ITask
    {
        public string Name { get; private set; }

        public event EventHandler Validated;

        public DataValidationTask(string name)
        {
            Name = name;
        }

        public void OnUserTaskCompleted(object sender, EventArgs args)
        {
            Execute();
        }

        public void Execute()
        {
            Console.WriteLine($"{Name} is executing");

            OnValidated();
        }

        protected virtual void OnValidated()
        {
            Validated?.Invoke(this, EventArgs.Empty);
        }
    }

    public class DatabaseTask : ITask
    {
        public string Name { get; private set; }

        public DatabaseTask(string name)
        {
            Name = name;
        }

        public void OnDataValidationTaskCompleted(object sender, EventArgs args)
        {
            Execute();
        }

        public void Execute()
        {
            Console.WriteLine($"{Name} is executing");
        }
    }
}
