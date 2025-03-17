using AutoMapper;
using LanguageExt;
using MyApiV8.Application.Interfaces.Base;
using MyApiV8.Domain.Interfaces.Models;
using MyApiV8.Domain.Interfaces.Notifier;
using MyApiV8.Domain.Models;

namespace MyApiV8.Application.Services.Base;

public abstract class BaseAppService : IBaseAppService
{
    protected readonly INotifier _notifier;
    protected readonly INotification _notification;
    protected readonly IMapper _mapper;
    protected readonly IApplicationResponse _applicationResponse;

    protected BaseAppService(
        INotifier notifier,
        INotification notification,
        IMapper mapper,
        IApplicationResponse applicationResponse)
    {
        _notifier = notifier;
        _notification = notification;
        _mapper = mapper;
        _applicationResponse = applicationResponse;
    }

    protected void Notify(string message) => _notifier.Handle(_notification.CreateNotification(message));

    protected virtual Task<IApplicationResponse> GetApplicationResponse<TModel, TDTO>(Option<TModel> model)
        where TModel : class
        where TDTO : class => 
        model.Match
        (
                Some: dto => _applicationResponse.Ok(_mapper.Map<TDTO>(dto)),
                None: _applicationResponse.BadRequest(_notifier.GetMessages())
        );
}
