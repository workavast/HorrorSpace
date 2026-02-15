using NUnit.Framework;

namespace Avastrad.Storages
{
    public class IntStorageUnitTests
    {
        [Test]
        public void LiteInitializationTest()
        {
            var intStorage0 = new IntStorage(0);
            var intStorage1 = new IntStorage(10);
            var intStorage2 = new IntStorage(10, 5);
            var intStorage3 = new IntStorage(10, 5, 1);

            CheckValues(intStorage0, 0, 0, 0);
            CheckValues(intStorage1, 10, 0, 0);
            CheckValues(intStorage2, 10, 5, 0);
            CheckValues(intStorage3, 10, 5, 1);
        }

        [Test]
        public void HardInitializationTest()
        {
            var intStorage0 = new IntStorage(-10);
            var intStorage1 = new IntStorage(10, -10);
            var intStorage2 = new IntStorage(10, -10, -20);
            var intStorage3 = new IntStorage(10, 5, 50);

            CheckValues(intStorage0, -10, -10, -10);
            CheckValues(intStorage1, 10, -10, -10);
            CheckValues(intStorage2, 10, -10, -20);
            CheckValues(intStorage3, 10, 5, 5);
        }

        [Test]
        public void UltraHardInitializationTest()
        {
            var intStorage0 = new IntStorage(-10, 10);
            var intStorage1 = new IntStorage(-10, 10, 20);
            var intStorage2 = new IntStorage(-10, 10, -20);

            CheckValues(intStorage0, -10, -10, -10);
            CheckValues(intStorage1, -10, -10, -10);
            CheckValues(intStorage2, -10, -10, -20);
        }

        [Test]
        public void SetMaxValueTest()
        {
            var intStorage0 = new IntStorage();
            var intStorage1 = new IntStorage();
            var intStorage2 = new IntStorage();

            intStorage0.SetMaxValue(0);
            intStorage1.SetMaxValue(1);
            intStorage2.SetMaxValue(-1);

            CheckValues(intStorage0, 0, 0, 0);
            CheckValues(intStorage1, 1, 0, 0);
            CheckValues(intStorage2, -1, -1, -1);
        }

        [Test]
        public void SetCurrentValueTest()
        {
            var intStorage0 = new IntStorage();
            var intStorage1 = new IntStorage();
            var intStorage2 = new IntStorage();

            intStorage0.SetCurrentValue(0);
            intStorage1.SetCurrentValue(1);
            intStorage2.SetCurrentValue(-1);

            CheckValues(intStorage0, 0, 0, 0);
            CheckValues(intStorage1, 0, 0, 0);
            CheckValues(intStorage2, 0, 0, 0);
        }

        [Test]
        public void SetMinValueTest()
        {
            var intStorage0 = new IntStorage();
            var intStorage1 = new IntStorage();
            var intStorage2 = new IntStorage();

            intStorage0.SetMinValue(0);
            intStorage1.SetMinValue(1);
            intStorage2.SetMinValue(-1);

            CheckValues(intStorage0, 0, 0, 0);
            CheckValues(intStorage1, 1, 1, 1);
            CheckValues(intStorage2, 0, 0, -1);
        }

        [Test]
        public void ChangeCurrentValueTest()
        {
            var intStorage0 = new IntStorage();
            intStorage0.ChangeCurrentValue(0);
            CheckValues(intStorage0, 0, 0, 0);
            intStorage0.ChangeCurrentValue(-1);
            CheckValues(intStorage0, 0, 0, 0);
            intStorage0.ChangeCurrentValue(1);
            CheckValues(intStorage0, 0, 0, 0);

            var intStorage1 = new IntStorage(10);
            intStorage1.ChangeCurrentValue(0);
            CheckValues(intStorage1, 10, 0, 0);
            intStorage1.ChangeCurrentValue(-1);
            CheckValues(intStorage1, 10, 0, 0);
            intStorage1.ChangeCurrentValue(1);
            CheckValues(intStorage1, 10, 1, 0);

            var intStorage2 = new IntStorage(-10, -50, -100);
            intStorage2.ChangeCurrentValue(0);
            CheckValues(intStorage2, -10, -50, -100);
            intStorage2.ChangeCurrentValue(-1);
            CheckValues(intStorage2, -10, -51, -100);
            intStorage2.ChangeCurrentValue(1);
            CheckValues(intStorage2, -10, -50, -100);
        }

        private static void CheckValues(IntStorage intStorage, int max, int cur, int min)
        {
            var actual = (intStorage.MaxValue, intStorage.CurrentValue, intStorage.MinValue);
            var expected = (max, cur, min);

            Assert.AreEqual(expected, actual);
        }
    }
}