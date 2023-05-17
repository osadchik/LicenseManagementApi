using AutoFixture;
using System.Dynamic;

namespace UnitTests.Utils
{
    internal class ExpandoObjectCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customize<ExpandoObject>(composer => composer.FromFactory(() =>
            {
                ExpandoObject result = new();
                result.AddMany(fixture.Create<KeyValuePair<string, object?>>, fixture.RepeatCount);
                return result;
            }));
        }
    }
}