using NUnit.Framework;

namespace Rhino.Mocks.Demo
{
	[TestFixture]
	public class StubTests
	{
		public class Widget
		{
			public virtual int Id { get; set; }
			public virtual string Name { get; set; }
		}

		[Test]
		public void StubProvidesDefaultValuesForAllProperties()
		{
			var widgetStub = MockRepository.GenerateStub<Widget>();

			Assert.That(widgetStub.Id, Is.EqualTo(0));
			Assert.That(widgetStub.Name, Is.Null);
		}

		[Test]
		public void StubAllowsYouToSetItsPropertiesAndMaintainsState()
		{
			var widgetStub = MockRepository.GenerateStub<Widget>();

			widgetStub.Name = "Mog";

			Assert.That(widgetStub.Name, Is.EqualTo("Mog"));
		}

	}
}