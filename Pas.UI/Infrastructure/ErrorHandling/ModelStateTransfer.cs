using Microsoft.AspNetCore.Mvc.Filters;

namespace Pas.UI.Infrastructure.ErrorHandling
{
    public abstract class ModelStateTransfer : ActionFilterAttribute
    {
        protected const string Key = nameof(ModelStateTransfer);
    }
}