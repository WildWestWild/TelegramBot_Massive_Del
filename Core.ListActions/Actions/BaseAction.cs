using Core.ListActions.ActionCommands;

namespace Core.ListActions.Actions;

public abstract class BaseAction
{
    protected event Action<ICommandIdentificator>? AfterActionEvent;

    public void OnAfterActionEvent(ICommandIdentificator obj) =>  AfterActionEvent?.Invoke(obj);
}