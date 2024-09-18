using Dima.Domain.Models;
using Dima.TaskPlanner.DataAccess;
using System;
using System.Collections.Generic;

namespace Dima.Domain.Logic
{
    internal class Logic
    {
        static void Main(string[] args)
        {
            List<WorkItem> workItems = new List<WorkItem>();
            IWorkItemsRepository repository = new FileWorkItemsRepository();  // Створення репозиторію
            SimpleTaskPlanner taskPlanner = new SimpleTaskPlanner(repository);  // Передача репозиторію у TaskPlanner

            while (true)
            {
                WorkItem workItem = new WorkItem();

                Console.WriteLine("Напишіть назву:");
                workItem.title = Console.ReadLine();

                Console.WriteLine("Напишіть Дату в форматі dd.MM.yyyy:");
                workItem.dueDate = DateTime.ParseExact(Console.ReadLine(), "dd.MM.yyyy", null);

                Console.WriteLine("Виберіть пріоритет (none, low, medium, high, urgent):");
                workItem.priority = Enum.Parse<Priority>(Console.ReadLine(), true);

                workItems.Add(workItem);

                string s;
                Console.WriteLine("Напишіть + якщо хочете додати ще 1 item і - якщо хочете перейти до сортування:");
                s = Console.ReadLine();

                if (s == "-")
                {
                    break;
                }
            }

            int s1;
            Console.WriteLine("Введіть цифру методу сортування. 1. По пріоритету, 2. По алфавіту, 3. По даті:");
            s1 = int.Parse(Console.ReadLine());

            WorkItem[] sortedItems;
            switch (s1)
            {
                case 1:
                    sortedItems = taskPlanner.CreatePlan(workItems.ToArray(), taskPlanner.SortPr);
                    break;
                case 2:
                    sortedItems = taskPlanner.CreatePlan(workItems.ToArray(), taskPlanner.SortStr);
                    break;
                case 3:
                    sortedItems = taskPlanner.CreatePlan(workItems.ToArray(), taskPlanner.SortByDate);
                    break;
                default:
                    Console.WriteLine("Невірний вибір, сортування не виконано.");
                    return;
            }


            Console.WriteLine("Результати сортування:");
            foreach (var item in sortedItems)
            {
                Console.WriteLine(item.ToString());
            }
        }
    }

    public class SimpleTaskPlanner
    {
        private readonly IWorkItemsRepository _repository;

        public SimpleTaskPlanner(IWorkItemsRepository repository)
        {
            _repository = repository;
            repository.SaveChanges();

        }

        public WorkItem[] CreatePlan(WorkItem[] items, Comparison<WorkItem> comparison)
        {
            List<WorkItem> workItems = items.ToList();
            workItems.Sort(comparison);
            foreach (var item in workItems) {
                _repository.Add(item);
                    }
            _repository.SaveChanges();
            return workItems.ToArray();
        }

        public int SortPr(WorkItem oneItem, WorkItem twoItem)
        {
            return twoItem.priority.CompareTo(oneItem.priority);
        }

        public int SortStr(WorkItem oneItem, WorkItem twoItem)
        {
            return string.Compare(oneItem.title, twoItem.title, StringComparison.OrdinalIgnoreCase);
        }

        public int SortByDate(WorkItem oneItem, WorkItem twoItem)
        {
            return oneItem.dueDate.CompareTo(twoItem.dueDate);
        }
    }

}