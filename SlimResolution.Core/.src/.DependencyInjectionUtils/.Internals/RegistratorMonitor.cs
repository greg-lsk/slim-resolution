using System;

using SlimResolution.Core.IObservableUtils;
using SlimResolution.Core.MetadataTypeDiscovery.Internals;


namespace SlimResolution.Core.DependencyInjectionUtils.Internals;

internal sealed class RegistratorMonitor : Observable<RegistrationInfo>
{
    internal RegistratorMonitor(IObserverCollection<IObserver<RegistrationInfo>> observers) : base(observers) { }


    internal void Handle(RegistrationInfo registrationInfo) => Observers.OnEach(o => o.OnNext(registrationInfo));
}