using NUnit.Framework;

namespace GearPuzzle.Tests
{
    public sealed class TapClassifierTests
    {
        private readonly Cell cell = new Cell(3, 3);

        [Test]
        public void SecondTapInsideWindow_IsDouble()
        {
            var classifier = new TapClassifier(0.3f);

            Assert.AreEqual(TapKind.Single, classifier.Classify(cell, 0f));
            Assert.AreEqual(TapKind.Double, classifier.Classify(cell, 0.2f));
        }

        [Test]
        public void SecondTapAfterWindow_IsSingle()
        {
            var classifier = new TapClassifier(0.3f);

            classifier.Classify(cell, 0f);

            Assert.AreEqual(TapKind.Single, classifier.Classify(cell, 0.5f));
        }

        [Test]
        public void SecondTapOnOtherCell_IsSingle()
        {
            var classifier = new TapClassifier(0.3f);

            classifier.Classify(cell, 0f);

            Assert.AreEqual(TapKind.Single, classifier.Classify(new Cell(3, 4), 0.1f));
        }

        [Test]
        public void ThirdQuickTap_StartsOver()
        {
            var classifier = new TapClassifier(0.3f);

            classifier.Classify(cell, 0f);
            classifier.Classify(cell, 0.1f);

            Assert.AreEqual(TapKind.Single, classifier.Classify(cell, 0.2f));
        }
    }
}
