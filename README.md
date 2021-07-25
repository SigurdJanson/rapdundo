# rapdundo

## Overview

![Basic layout of the implemented pattern]("documentation/img/ClassVisu.svg")

The classes and objects participating here include:

* The invoker asks the command to carry out the request. These are menu items, buttons, ... They can be based on [`ICommandSource`](https://docs.microsoft.com/en-us/dotnet/api/system.windows.input.icommandsource).
* The receiver is the object that performs the operations that are needed to carry out the command.
* `IUndoableCommand` declares an interface for executing an operation. It is derived from [`ICommand`](https://docs.microsoft.com/en-us/dotnet/api/system.windows.input.icommand.canexecutechanged).
* The **(Concrete) Command** links the invoker to the receiver
  * defines a binding between a receiver object and an action
  * implements `Execute()` by invoking the corresponding operation(s) on the receiver
* `IUndoableService`
  * `ISnackbar` is the MudBlazor service. With [Snackbars](https://mudblazor.com/components/snackbar) the toolkit has a UI for rapdUndo commands.
