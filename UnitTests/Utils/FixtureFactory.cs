using AutoFixture;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Kernel;
using System.Reflection;

namespace UnitTests.Utils
{
    internal static class FixtureFactory
    {
        public static IFixture GetAutoMockFixture(params Type[] customizationsOrBehaviors)
        {
            AutoNSubstituteCustomization customization = new()
            {
                ConfigureMembers = true,
                GenerateDelegates = true
            };

            IFixture fixture = new Fixture()
                .Customize(customization)
                .Customize(new StreamCustomization())
                .Customize(new ExpandoObjectCustomization());

            fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            foreach (Type type in customizationsOrBehaviors.Where(x => typeof(ICustomization).IsAssignableFrom(x)))
            {
                fixture.Customize(Create<ICustomization>(type));
            }

            foreach (Type type in customizationsOrBehaviors.Where(x => typeof(ISpecimenBuilderTransformation).IsAssignableFrom(x)))
            {
                fixture.Behaviors.Add(Create<ISpecimenBuilderTransformation>(type));
            }

            return fixture;
        }

        public static T Create<T>(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            ConstructorInfo? constructorInfo = typeof(T).IsAssignableFrom(type)
                ? type.GetConstructor(Type.EmptyTypes)
                : throw new ArgumentException($"{type} is not compatible with {typeof(T)}.");

            T result = constructorInfo != null
                ? (T)constructorInfo.Invoke(null)
                : throw new ArgumentException($"{type} has no default constructor.");

            return result;
        }
    }
}
