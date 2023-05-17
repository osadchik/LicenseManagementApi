using AutoFixture;
using System.Text;

namespace UnitTests.Utils
{
    internal class StreamCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Register((Func<string, Stream>)((string data) => new MemoryStream(Encoding.UTF8.GetBytes(data))));
        }
    }
}
