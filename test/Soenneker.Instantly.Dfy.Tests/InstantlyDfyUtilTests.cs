using Soenneker.Instantly.Dfy.Abstract;
using Soenneker.Tests.HostedUnit;

namespace Soenneker.Instantly.Dfy.Tests;

[ClassDataSource<Host>(Shared = SharedType.PerTestSession)]
public sealed class InstantlyDfyUtilTests : HostedUnitTest
{
    private readonly IInstantlyDfyUtil _util;

    public InstantlyDfyUtilTests(Host host) : base(host)
    {
        _util = Resolve<IInstantlyDfyUtil>(true);
    }

    [Test]
    public void Default()
    {

    }
}
