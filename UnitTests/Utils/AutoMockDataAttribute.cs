using AutoFixture.Xunit2;

namespace UnitTests.Utils
{
    internal class AutoMockDataAttribute : AutoDataAttribute
    {
        public AutoMockDataAttribute(params Type[] customizationsOrBehaviors) : base(() => 
            FixtureFactory.GetAutoMockFixture(customizationsOrBehaviors))
        { }
    }
}
