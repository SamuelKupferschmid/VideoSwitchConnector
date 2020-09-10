using Microsoft.AspNetCore.SignalR;
using SimpleInjector;
using SimpleInjector.Lifestyles;

namespace VideoSwitchConnectorApi
{
    public sealed class SimpleInjectorHubActivator<T>
    : IHubActivator<T> where T : Hub
    {
        private readonly Container container;
        private Scope scope;

        public SimpleInjectorHubActivator(Container container) =>
            this.container = container;

        public T Create()
        {
            this.scope = AsyncScopedLifestyle.BeginScope(this.container);
            return this.container.GetInstance<T>();
        }

        public void Release(T hub) => this.scope.Dispose();
    }
}
