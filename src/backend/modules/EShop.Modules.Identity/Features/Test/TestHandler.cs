namespace EShop.Modules.Identity.Features.Test;

public sealed class TestHandler
{
    public TestResponse Handle(
        TestCommand command)
    {
        return new TestResponse("Это тестовый эндпоинт!");
    }
}