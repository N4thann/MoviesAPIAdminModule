namespace Application.Interfaces.Mediator
{
    public interface ICommand { }

    public interface ICommand<out TResult> : ICommand { }
}
