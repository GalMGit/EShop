namespace Modules.Identity.Features.Test;

public sealed class TestHandler
{
    public TestResponse Handle(
        TestQuery query)
    {
        
        return new TestResponse("1");
    }
}