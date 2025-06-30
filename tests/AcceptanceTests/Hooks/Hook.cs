using TechTalk.SpecFlow;

namespace Mc2.CrudTest.AcceptanceTests.Hooks
{
    [Binding]
    public class Hook
    {
        [BeforeScenario]
        public void BeforeScenario()
        {
            // Initialize resources before each scenario if needed
        }

        [AfterScenario]
        public void AfterScenario()
        {
            // Clean up resources after each scenario if needed
        }
    }
}
