using System;
using System.Collections.Generic;
using System.Linq;
using Dima.Domain.Models;
using Dima.Domain.Logic;
using Moq;
using Xunit;

namespace Dima.TaskPlanner.Domain.Logic.Tests
{
    public class SimpleTaskPlannerTests
    {
        [Fact]
        public void CreatePlan_SortsByPriority_Correctly()
        {
            // Arrange
            var mockRepository = new Mock<IWorkItemsRepository>();

            var workItems = new List<WorkItem>
            {
                new WorkItem { title = "Task 1", priority = Priority.low, isCompleted = false },
                new WorkItem { title = "Task 2", priority = Priority.high, isCompleted = false },
                new WorkItem { title = "Task 3", priority = Priority.medium, isCompleted = false }
            };

            mockRepository.Setup(r => r.GetAll()).Returns(workItems.ToArray());

            var taskPlanner = new SimpleTaskPlanner(mockRepository.Object);

            // Act
            var result = taskPlanner.CreatePlan(workItems.ToArray(), taskPlanner.SortPr);

            // Assert
            Assert.Equal(Priority.high, result[0].priority);
            Assert.Equal(Priority.medium, result[1].priority);
            Assert.Equal(Priority.low, result[2].priority);
        }

        [Fact]
        public void CreatePlan_ExcludesCompletedTasks()
        {
            // Arrange
            var mockRepository = new Mock<IWorkItemsRepository>();

            var workItems = new List<WorkItem>
            {
                new WorkItem { title = "Task 1", priority = Priority.low, isCompleted = true },
                new WorkItem { title = "Task 2", priority = Priority.high, isCompleted = false },
                new WorkItem { title = "Task 3", priority = Priority.medium, isCompleted = false }
            };

            mockRepository.Setup(r => r.GetAll()).Returns(workItems.ToArray());

            var taskPlanner = new SimpleTaskPlanner(mockRepository.Object);

            // Act
            var result = taskPlanner.CreatePlan(workItems.ToArray(), taskPlanner.SortPr);

            // Assert
            Assert.Equal(2, result.Length);
            Assert.DoesNotContain(result, item => item.isCompleted);
        }

        [Fact]
        public void CreatePlan_SortsByDate_Correctly()
        {
            // Arrange
            var mockRepository = new Mock<IWorkItemsRepository>();

            var workItems = new List<WorkItem>
            {
                new WorkItem { title = "Task 1", dueDate = DateTime.Now.AddDays(3), isCompleted = false },
                new WorkItem { title = "Task 2", dueDate = DateTime.Now.AddDays(1), isCompleted = false },
                new WorkItem { title = "Task 3", dueDate = DateTime.Now.AddDays(2), isCompleted = false }
            };

            mockRepository.Setup(r => r.GetAll()).Returns(workItems.ToArray());

            var taskPlanner = new SimpleTaskPlanner(mockRepository.Object);

            // Act
            var result = taskPlanner.CreatePlan(workItems.ToArray(), taskPlanner.SortByDate);

            // Assert
            Assert.Equal("Task 2", result[0].title);
            Assert.Equal("Task 3", result[1].title);
            Assert.Equal("Task 1", result[2].title);
        }
    }
}
