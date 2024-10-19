using Core.ListActions.ActionCommands;
using Core.ListActions.Actions;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Tests.xUnit
{
    public class ActionTests
    {
        private static readonly AddListCommand Command = new()
        {
            ChatId = new Random().NextInt64(1, long.MaxValue),
            Name = "TestList"
        };

        private const int TIMEOUT_TESTS = 15000;
        private readonly ReadListAction _readListAction;
        private readonly AddListAction _addListAction;
        private readonly AddElementToListAction _addElementToListAction;
        private readonly DeleteElementFromListAction _deleteElementFromListAction;
        private readonly UpdateElementFromListAction _updateElementFromListAction;
        private readonly ILogger<ActionTests> _logger;
        private readonly CancellationTokenSource _cancellationTokenSource = new(TIMEOUT_TESTS);
        
        public ActionTests(
            ILogger<ActionTests> logger, 
            ReadListAction readListAction,
            AddListAction addListAction,
            AddElementToListAction addElementToListAction,
            DeleteElementFromListAction deleteElementFromListAction,
            UpdateElementFromListAction updateElementFromListAction)
        {
            _readListAction = readListAction;
            _addListAction = addListAction;
            _addElementToListAction = addElementToListAction;
            _deleteElementFromListAction = deleteElementFromListAction;
            _updateElementFromListAction = updateElementFromListAction;
            _logger = logger;
        }

        [Fact]
        public async Task AddListTest()
        {
            await _addListAction.AddList(Command, _cancellationTokenSource.Token);
            var list = await _readListAction.GetList(Command, _cancellationTokenSource.Token);
            
            Assert.NotNull(list);
            Assert.Empty(list);
        }

        [Fact]
        public async Task CrudOperationsWithListTest()
        {
            #region Create
            
            _logger.LogInformation("Create start");

            foreach (ushort number in Enumerable.Range(1, 10))
            {
                await _addElementToListAction.AddElement(new AddOrUpdateElementCommand
                {
                    ChatId = Command.ChatId,
                    Name = Command.Name,
                    Number = number,
                    Data = "1"
                }, _cancellationTokenSource.Token);
            }
            _addElementToListAction.OnAfterActionEvent(Command);

            var addElementResult = await _readListAction.GetList(Command, _cancellationTokenSource.Token);
            
            Assert.NotNull(addElementResult);
            Assert.Equal(10, addElementResult.Length);
            Assert.Equal(10, addElementResult.Select(r=> Convert.ToInt32(r.Data)).Sum());
            
            _logger.LogInformation("Create done");
            #endregion

            #region Update
            
            _logger.LogInformation("Update start");

            foreach (ushort number in Enumerable.Range(1, 10))
            {
                await _updateElementFromListAction.UpdateFromList(new AddOrUpdateElementCommand
                {
                    ChatId = Command.ChatId,
                    Name = Command.Name,
                    Number = number,
                    Data = "2"
                }, _cancellationTokenSource.Token);
            }
            _updateElementFromListAction.OnAfterActionEvent(Command);
            
            var updateElementResult = await _readListAction.GetList(Command, _cancellationTokenSource.Token);
            
            Assert.NotNull(updateElementResult);
            Assert.Equal(10, updateElementResult.Length);
            Assert.Equal(20, updateElementResult.Select(r=> Convert.ToInt32(r.Data)).Sum());
            
            _logger.LogInformation("Update done");
            
            #endregion
            
            #region Delete
            
            _logger.LogInformation("Delete start");

            foreach (ushort number in Enumerable.Range(1, 10).Reverse())
            {
                await _deleteElementFromListAction.DeleteFromList(new DeleteElementCommand
                {
                    ChatId = Command.ChatId,
                    Name = Command.Name,
                    Numbers = new [] {number}
                }, _cancellationTokenSource.Token);
            }
            _deleteElementFromListAction.OnAfterActionEvent(Command);
            
            var deleteActionResult = await _readListAction.GetList(Command, _cancellationTokenSource.Token);
           
            Assert.NotNull(deleteActionResult);
            Assert.Equal(0, deleteActionResult.Length);
            
            _logger.LogInformation("Delete done");
            
            #endregion
        }
    }
}