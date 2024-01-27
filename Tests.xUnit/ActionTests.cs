using Core.ListActions.ActionCommands;
using Core.ListActions.Actions;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Tests.xUnit
{
    public class ActionTests: IDisposable
    {
        private static readonly AddListCommand Command = new()
        {
            ChatId = new Random().NextInt64(1, long.MaxValue),
            Name = "TestList"
        };
        
        private readonly ReadListAction _readListAction;
        private readonly AddListAction _addListAction;
        private readonly AddElementToListAction _addElementToListAction;
        private readonly ILogger<ActionTests> _logger;
        
        public ActionTests(
            ILogger<ActionTests> logger, 
            ReadListAction readListAction,
            AddListAction addListAction,
            AddElementToListAction addElementToListAction)
        {
            _readListAction = readListAction;
            _addListAction = addListAction;
            _addElementToListAction = addElementToListAction;
            _logger = logger;
        }

        [Fact]
        public void AddListTest()
        {
            _addListAction.AddList(Command);
            var list = _readListAction.GetList(Command);
            
            Assert.NotNull(list);
            Assert.Empty(list);
        }

        [Fact]
        public void AddElementTest()
        {
            foreach (ushort number in Enumerable.Range(1, 10))
            {
                _addElementToListAction.AddElement(new AddElementCommand
                {
                    ChatId = Command.ChatId,
                    Name = Command.Name,
                    Number = number,
                    Data = new Random().Next().ToString()
                });
            }

            var list = _readListAction.GetList(Command);
            Assert.NotNull(list);
            Assert.Equal(10, list.Length);
        }

        public void Dispose()
        {
        }
    }
}