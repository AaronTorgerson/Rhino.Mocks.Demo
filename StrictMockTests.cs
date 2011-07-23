using System.Collections.Generic;
using NUnit.Framework;
using Rhino.Mocks.Exceptions;

namespace Rhino.Mocks.Demo
{
	[TestFixture]
	public class StrictMockTests
	{
		[Test]
		public void CallingMethodWithoutExpectationOrStubFailsTest()
		{
			var mockList = MockRepository.GenerateStrictMock<IList<string>>();

			Assert.Throws<ExpectationViolationException>(() => mockList.Add("some text"));
		}

		[Test]
		public void NotCallingMethodExpectedMethodFailsTestOnVerify()
		{
			var mockList = MockRepository.GenerateStrictMock<IList<string>>();

			mockList.Expect(l => l.Add("some text"));

			Assert.Throws<ExpectationViolationException>(() => mockList.VerifyAllExpectations());
		}

		[Test]
		public void HappilyAcceptsCallForExpectedMethod()
		{
			var mockList = MockRepository.GenerateStrictMock<IList<string>>();

			mockList.Expect(l => l.Add("some text"));

			mockList.Add("some text");

			mockList.VerifyAllExpectations();
		}

		[Test]
		public void MultipleCallsToExpectedMethodFailsTestByDefault()
		{
			var mockList = MockRepository.GenerateStrictMock<IList<string>>();

			mockList.Expect(l => l.Add("some text"));

			mockList.Add("some text");

			Assert.Throws<ExpectationViolationException>(() => mockList.Add("other text"));
		}

		[Test]
		public void CallingExpectedMethodWithUnexpectedArumentsFailsTest()
		{
			var mockList = MockRepository.GenerateStrictMock<IList<string>>();

			mockList.Expect(l => l.Add("some text"));

			Assert.Throws<ExpectationViolationException>(() => mockList.Add("other text"));
		}

		[Test]
		public void ThrowsExceptionWhenExpectedMethodIsNotCalledEnoughTimes()
		{
			var mockList = MockRepository.GenerateStrictMock<IList<string>>();

			mockList.Expect(l => l.Add("some text")).Repeat.Twice();

			mockList.Add("some text");

			Assert.Throws<ExpectationViolationException>(() => mockList.VerifyAllExpectations());
		}

		[Test]
		public void HappilyAcceptsTwoCallsToExpectedMethodWithRepeat()
		{
			var mockList = MockRepository.GenerateStrictMock<IList<string>>();

			mockList.Expect(l => l.Add("some text")).Repeat.Twice();

			mockList.Add("some text");
			mockList.Add("some text");

			mockList.VerifyAllExpectations();
		}

		[Test]
		public void ExpectedMethodReturnsConfiguredReturnValue()
		{
			var mockList = MockRepository.GenerateStrictMock<IList<string>>();

			mockList.Expect(l => l.IndexOf("some text")).Return(1);

			Assert.That(mockList.IndexOf("some text"), Is.EqualTo(1));
			mockList.VerifyAllExpectations();
		}

		[Test]
		public void ArgumentConstraintsMakeTestLessStrict()
		{
			var mockList = MockRepository.GenerateStrictMock<IList<string>>();

			mockList.Expect(l => l.IndexOf(Arg.Text.EndsWith("text"))).Return(1);

			mockList.IndexOf("some text");

			mockList.VerifyAllExpectations();
		}

		[Test]
		public void ArgumentConstraintsApiIsFluentAndNice()
		{
			var mockList = MockRepository.GenerateStrictMock<IList<string>>();

			mockList.Expect(l => l.IndexOf(Arg.Text.EndsWith("text"))).Return(1);
			mockList.Expect(l => l.IndexOf(Arg<string>.Matches(s => s == "why would you do this?"))).Return(2);
			mockList.Expect(l => l.IndexOf(Arg.Is("soup"))).Return(3);

			Assert.That(mockList.IndexOf("some text"), Is.EqualTo(1));
			Assert.That(mockList.IndexOf("why would you do this?"), Is.EqualTo(2));
			Assert.That(mockList.IndexOf("soup"), Is.EqualTo(3));
		}

		[Test]
		public void VioatingArgumentConstraintFailsTest()
		{
			var mockList = MockRepository.GenerateStrictMock<IList<string>>();

			mockList.Expect(l => l.IndexOf(Arg.Text.EndsWith("text"))).Return(1);

			Assert.Throws<ExpectationViolationException>(() => mockList.IndexOf("some words"));
		}

		[Test]
		public void StubEnablesAMethodCallForSpecificArguments()
		{
			var mockList = MockRepository.GenerateStrictMock<IList<string>>();

			mockList.Stub(l => l.IndexOf("some text")).Return(1);

			Assert.That(mockList.IndexOf("some text"), Is.EqualTo(1));
			Assert.Throws<ExpectationViolationException>(() => mockList.IndexOf("other text"));
		}

		[Test]
		public void NotCallingStubbedMethodIsOkay()
		{
			var mockList = MockRepository.GenerateStrictMock<IList<string>>();

			mockList.Stub(l => l.IndexOf("some text")).Return(1);

			mockList.VerifyAllExpectations();
		}

		[Test]
		public void StubEnablesMultipleCallsToAMethodForSpecificArguments()
		{
			var mockList = MockRepository.GenerateStrictMock<IList<string>>();

			mockList.Stub(l => l.IndexOf("some text")).Return(1);

			Assert.That(mockList.IndexOf("some text"), Is.EqualTo(1));
			Assert.That(mockList.IndexOf("some text"), Is.EqualTo(1));
			Assert.That(mockList.IndexOf("some text"), Is.EqualTo(1));
		}

	}
}