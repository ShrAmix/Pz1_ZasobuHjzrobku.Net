using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using Dima.DataAccess.Abstractions;
using Dima.Domain.Models;

namespace Dima.DataAccess
{
    public class FileWorkItemsRepository : IWorkItemsRepository
    {
        private const string FilePath = "work-items.json";
        private readonly Dictionary<Guid, WorkItem> _workItems = new Dictionary<Guid, WorkItem>();

        public FileWorkItemsRepository()
        {
            if (File.Exists(FilePath) && new FileInfo(FilePath).Length > 0)
            {
                var json = File.ReadAllText(FilePath);
                var workItems = JsonConvert.DeserializeObject<List<WorkItem>>(json);
                if (workItems != null)
                {
                    foreach (var item in workItems)
                    {
                        _workItems[item.Id] = item;
                    }
                }
            }
        }

        public Guid Add(WorkItem workItem)
        {
            var clone = workItem.Clone();
            var id = Guid.NewGuid();
            clone.Id = id;
            _workItems[id] = clone;
            return id;
        }

        public WorkItem Get(Guid id)
        {
            _workItems.TryGetValue(id, out var workItem);
            return workItem;
        }

        public WorkItem[] GetAll()
        {
            return new List<WorkItem>(_workItems.Values).ToArray();
        }

        public bool Update(WorkItem workItem)
        {
            if (_workItems.ContainsKey(workItem.Id))
            {
                _workItems[workItem.Id] = workItem;
                return true;
            }
            return false;
        }

        public bool Remove(Guid id)
        {
            return _workItems.Remove(id);
        }

        public void SaveChanges()
        {
            var json = JsonConvert.SerializeObject(_workItems.Values, Formatting.Indented);
            File.WriteAllText(FilePath, json);
        }
    }
}
