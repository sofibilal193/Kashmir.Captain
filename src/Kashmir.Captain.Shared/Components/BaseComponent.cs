using MediatR;
using Microsoft.AspNetCore.Components;

namespace Kashmir.Captain.Shared
{
    public abstract class BaseComponent : ComponentBase
    {
        [Inject]
        protected IMediator Mediator { get; set; } = default!;

        [Inject]
        protected NavigationManager Navigation { get; set; } = default!;
    }
}
