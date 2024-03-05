﻿using Core.ListActions.ActionCommands;

namespace Core.ListActions.Actions;

public abstract class BaseAction
{
    protected event Action<ICommandIdentificator>? AfterCommandAction;

    public void OnAfterActionEvent(ICommandIdentificator obj) =>  AfterCommandAction?.Invoke(obj);
}