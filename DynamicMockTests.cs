using System.Collections.Generic;
using NUnit.Framework;

namespace Rhino.Mocks.Demo
{
	[TestFixture]
	public class DynamicMockTests
	{
		[Test]
		public void CallToUnexpectedMethodReturnsNullForReferenceType()
		{
			var mockList = MockRepository.GenerateMock<IList<string>>();

			Assert.That(mockList[0], Is.EqualTo(null));
		}
	}
}