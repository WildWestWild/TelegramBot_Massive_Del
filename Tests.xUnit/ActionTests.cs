using Core.ListActions.ActionCommands;
using Core.ListActions.Actions;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Tests.xUnit
{
    public class ActionTests: IDisposable
    {
        private readonly ReadListAction _readListAction;
        private readonly AddListAction _addListAction;
        private readonly ILogger<ActionTests> _logger;
        private readonly AddListCommand _command = new()
        {
            ChatId = new Random().NextInt64(1, long.MaxValue),
            Name = "TestList"
        };

        public ActionTests(
            ILogger<ActionTests> logger, 
            ReadListAction readListAction,
            AddListAction addListAction)
        {
            _readListAction = readListAction;
            _addListAction = addListAction;
            _logger = logger;
        }

        [Fact]
        public void AddListTest()
        {
            _addListAction.AddList(_command);
            var list = _readListAction.GetList(new ReadListCommand
            {
                ChatId = _command.ChatId,
                Name = _command.Name
            });
            
            Assert.NotNull(list);
            Assert.Empty(list);
        }

        [Fact]
        public void Test1()
        {
            var x = 1;
        }

        public void Dispose()
        {
        }
    }
}