using System;

namespace Dima.Domain.Models
{
    public enum Priority
    {
        none = 0,
        low = 1,
        medium = 2,
        high = 3,
        urgent = 4,
    }
    public enum Complexily
    {
        none = 0,
        day = 1,
        mounth = 2,
        years = 3,
    }

    public class WorkItem
    {
        public Guid Id { get; set; }
        public DateTime creationDate;
        public DateTime dueDate;
        public Priority priority;
        public Complexily complexily;
        public string title;
        public string description;
        public bool isCompleted;

        public WorkItem Clone()
        {
            return (WorkItem)this.MemberwiseClone();
        }

        public override string ToString()
        {
            return $"{title}: due to {dueDate.ToString("dd.MM.yyyy")}, {priority.ToString().ToLower()} priority";
        }
    }
    public interface IWorkItemsRepository
    {
        Guid Add(WorkItem workItem);
        WorkItem[] GetAll();
        void SaveChanges();
    }


    internal class Model
    {
        static void Main(string[] args)
        {

        }
    }
}
