﻿using Moq;
using System;
using System.Collections.Generic;
using ToDoWebApp.Data.Intefaces;
using ToDoWebApp.Models;
using ToDoWebApp.Repository;
using ToDoWebApp.Services;
using Xunit;

namespace ToDoWebAppTests
{
    public class ToDoItemServiceTests
    {

        /*********************************/
        /* Positive(PassValidData) TEST */
        /*******************************/

        public static IEnumerable<object[]> GoodPriority = new List<object[]>
        {
            new object[]{0},
            new object[]{1},
            new object[]{2},
            new object[]{3},
            new object[]{4},
            new object[]{5}
        };

        [Theory]
        [MemberData(nameof(GoodPriority))]
        public void Add_PassValidData_Ok(int testScore){
                        // Arange ()
                        var contextMock = new Mock<TodoAPIRepository>();
                         IToDoItemService service = new ToDoItemService(contextMock.Object);

                        // Act
                      string eq = service.Add(new TodoItem /* id - 3 */
                        {
                            Name = "ItemForTest",
                            Description = "DescriptionForTest",
                            priority = testScore
                      });

                    // Assert
                    Assert.Equal("True", eq);
        }

        /* Can't create 2 TodoItems with same name (throw exception) */
        [Fact]
        public void AddAlreadyExistDataName_UnPassingAlreadyExistingData_Ok()
        {
            // Arange ()
            var contextMock = new Mock<TodoAPIRepository>();
            IToDoItemService service = new ToDoItemService(contextMock.Object);

            // Assert
            Assert.Throws<ArgumentException>(() => service.Add(new TodoItem /* id - 3 */
            {
                Name = "ItemForTestAlreadeCreated",
                Description = "DescriptionForTest",
                priority = 4
            }));
        }


        [Fact]
        public void Remove_PassValidData_Ok()
        {
            // Arange ()
            var contextMock = new Mock<TodoAPIRepository>();
            IToDoItemService service = new ToDoItemService(contextMock.Object);

            // Act
            service.Remove("2");
        

            // Assert
            var mark = contextMock.Object.Find("2"); /* Id = 4 */
            Assert.Null(mark);
        }


        [Fact]
        public void Find_PassValidData_Ok()
        {
            // Arange ()
            var contextMock = new Mock<TodoAPIRepository>();
            IToDoItemService service = new ToDoItemService(contextMock.Object);

            // Act
            var mark = service.Find("1");


            // Assert
            Assert.NotNull(mark);
        }


        [Fact]
        public void Update_PassValidData_Ok()
        {
            // Arange ()
            var contextMock = new Mock<TodoAPIRepository>();
            IToDoItemService service = new ToDoItemService(contextMock.Object);

            // Act
            service.Update(new TodoItem { 
              TodoItemId = 2,
              Name = "ItemForUpdated",
              Description = "DescriptionUpdated",
              priority = 4
            });

            // Assert
            var mark = contextMock.Object.Find("2"); /* Id = 4 */
            Assert.Equal( "ItemForUpdated", mark.Name);
        }

        /* Can't edit 2 TodoItems with same name (throw exception) */
        [Fact]
        public void UpdateToAlreadyExistDataName_UnPassingAlreadyExistingData_Ok()
        {
            // Arange ()
            var contextMock = new Mock<TodoAPIRepository>();
            IToDoItemService service = new ToDoItemService(contextMock.Object);


            // Assert
            var mark = contextMock.Object.Find("2"); /* Id = 4 */
        Assert.NotEqual("ItemForTestAlreadeCreated", mark.Name);
            Assert.Throws<ArgumentException>(() => service.Update(new TodoItem
            {
                TodoItemId = 2,
                Name = "ItemForTestAlreadeCreated",
                Description = "DescriptionUpdated",
                priority = 4
            }));
        }

        [Fact]
        public void GetAll_PassValidData_Ok()
        {
            // Arange ()
            var contextMock = new Mock<TodoAPIRepository>();
            IToDoItemService service = new ToDoItemService(contextMock.Object);

            // Act
            service.GetAll();

            // Assert
            var mark = contextMock.Object.GetAll(); /* Id = 4 */
            Assert.NotNull(mark);
        }

        /* TodoItem DeadlineDate must be higher then CreationDate. (throw exception)*/
        [Fact]
        public void AddCorrectDate_PassGoodDate_Ok()
        {
            // Arange ()
            var contextMock = new Mock<TodoAPIRepository>();
            IToDoItemService service = new ToDoItemService(contextMock.Object);


            // Assert
            var mark = contextMock.Object.Find("2"); /* Id = 4 */
            Assert.NotEqual("ItemForTestAlreadeCreated", mark.Name);
            Assert.Throws<ArgumentException>(() => service.Add(new TodoItem
            {
                Name = "PamPamPam",
                Description = "DescriptionPAM",
                priority = 4,
                DeadLineDate = new DateTime(2088, 3, 1, 7, 0, 0) // 3/1/2088 7:00:00 AM
            }));
        }

        /* then creating/editing TodoItems, we can have only 1 Wip status item with priority 1. */
        [Fact]
        public void AddCorrectWipStatusNumber_PassGoodStatus_Ok()
        {
            // Arange ()
            var contextMock = new Mock<TodoAPIRepository>();
            IToDoItemService service = new ToDoItemService(contextMock.Object);

            // Act
            var eq = service.Add(new TodoItem
            {
                Name = "Item132323",
                Description = "Description1",
                priority = 1,
                status = Status.Wip
            });

            // Assert
            Assert.NotEqual("Wip Status With Priority1 Can Be Only One", eq);
  
        }

        [Fact]
        public void UpdateCorrectWipStatusNumber_PassGoodStatus_Ok()
        {
            // Arange ()
            var contextMock = new Mock<TodoAPIRepository>();
            IToDoItemService service = new ToDoItemService(contextMock.Object);

            // Act
            var eq = service.Update(new TodoItem
            {
                CategoryId = 2,
                Name = "Item2dasdasd",
                Description = "Description13232",
                priority = 1,
                status = Status.Wip
            });

            // Assert
            Assert.Equal("Updated Successfully", eq);

        }


        /********************************/
        /*Against(PassUnValidData) TEST*/
        /******************************/

        public static IEnumerable<object[]> BadPriority = new List<object[]>
        {
            new object[]{1637},
            new object[]{3233232},
            new object[]{99999},
            new object[]{6},
            new object[]{-54667},
            new object[]{-1}
        };

        [Theory]
        [MemberData(nameof(BadPriority))]
        public void Add_PassUnValidData_Ok(int testScore)
        {
            // Arange ()
            var contextMock = new Mock<TodoAPIRepository>();
            IToDoItemService service = new ToDoItemService(contextMock.Object);

            // Act
            string eq = service.Add(new TodoItem /* id - 3 */
            {
                Name = "ItemForUnValidPriorityTest",
                Description = "DescriptionForTest",
                priority = testScore,
            });

            // Assert
            Assert.Equal("False", eq);

        }


        [Fact]
        public void Remove_PassUnValidData_Ok()
        {
            // Arange ()
            var contextMock = new Mock<TodoAPIRepository>();
            IToDoItemService service = new ToDoItemService(contextMock.Object);

            // Act
            var mark = service.Remove("999");


            // Assert
            Assert.Null(mark);
        }


        [Fact]
        public void Find_PassUnValidData_Ok()
        {
            // Arange ()
            var contextMock = new Mock<TodoAPIRepository>();
            IToDoItemService service = new ToDoItemService(contextMock.Object);

            // Act
            var mark = service.Find("9999");

            // Assert
            Assert.Null(mark);
        }


        [Fact]
        public void Update_PassUnValidData_Ok()
        {
            // Arange ()
            var contextMock = new Mock<TodoAPIRepository>();
            IToDoItemService service = new ToDoItemService(contextMock.Object);

            string eq = service.Update(new TodoItem
            {
                TodoItemId = 2,
                Name = "ItemForUpdated",
                Description = "DescriptionUpdated",
                priority = 55
            });

            // Assert
            Assert.Equal("False", eq);
        }

    }
}
