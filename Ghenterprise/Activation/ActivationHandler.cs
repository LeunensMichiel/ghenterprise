using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ghenterprise.Activation
{
    // Info: https://github.com/Microsoft/WindowsTemplateStudio/blob/master/docs/activation.md
    internal abstract class ActivationHandler
    {
        public abstract bool CanHandleActivation(object args);

        public abstract Task HandleActivationAsync(object args);
    }

    // Deze klasse inplementeren
    internal abstract class ActivationHandler<T> : ActivationHandler where T : class
    {
        public override bool CanHandleActivation(object args)
        {
            return args is T && CanHandleInternal(args as T);
        }

        public override async Task HandleActivationAsync(object args)
        {
            await HandleIntervalAsync(args as T);
        }

        // Deze methode overriden later om logica te implementeren
        protected abstract Task HandleIntervalAsync(T args);

        // Kunt ge overriden voor extra validatie volgens Microsoft
        protected virtual bool CanHandleInternal(T args)
        {
            return true;
        }
    }
}
