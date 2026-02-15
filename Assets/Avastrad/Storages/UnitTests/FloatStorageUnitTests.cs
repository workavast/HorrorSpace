using NUnit.Framework;

namespace Avastrad.Storages
{
    public class FloatStorageUnitTests
    {
        [Test]
        public void LiteInitializationTest()
        {
            var intStorage0 = new FloatStorage(0);
            var intStorage1 = new FloatStorage(10);
            var intStorage2 = new FloatStorage(10, 5);
            var intStorage3 = new FloatStorage(10, 5, 1);

            CheckValues(intStorage0, 0, 0, 0);
            CheckValues(intStorage1, 10, 0, 0);
            CheckValues(intStorage2, 10, 5, 0);
            CheckValues(intStorage3, 10, 5, 1);
        }

        [Test]
        public void HardInitializationTest()
        {
            var intStorage0 = new FloatStorage(-10);
            var intStorage1 = new FloatStorage(10, -10);
            var intStorage2 = new FloatStorage(10, -10, -20);
            var intStorage3 = new FloatStorage(10, 5, 50);

            CheckValues(intStorage0, -10, -10, -10);
            CheckValues(intStorage1, 10, -10, -10);
            CheckValues(intStorage2, 10, -10, -20);
            CheckValues(intStorage3, 10, 5, 5);
        }

        [Test]
        public void UltraHardInitializationTest()
        {
            var intStorage0 = new FloatStorage(-10, 10);
            var intStorage1 = new FloatStorage(-10, 10, 20);
            var intStorage2 = new FloatStorage(-10, 10, -20);

            CheckValues(intStorage0, -10, -10, -10);
            CheckValues(intStorage1, -10, -10, -10);
            CheckValues(intStorage2, -10, -10, -20);
        }

        [Test]
        public void SetMaxValueTest()
        {
            var intStorage0 = new FloatStorage();
            var intStorage1 = new FloatStorage();
            var intStorage2 = new FloatStorage();

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
            var intStorage0 = new FloatStorage();
            var intStorage1 = new FloatStorage();
            var intStorage2 = new FloatStorage();

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
            var intStorage0 = new FloatStorage();
            var intStorage1 = new FloatStorage();
            var intStorage2 = new FloatStorage();

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
            var intStorage0 = new FloatStorage();
            intStorage0.ChangeCurrentValue(0);
            CheckValues(intStorage0, 0, 0, 0);
            intStorage0.ChangeCurrentValue(-1);
            CheckValues(intStorage0, 0, 0, 0);
            intStorage0.ChangeCurrentValue(1);
            CheckValues(intStorage0, 0, 0, 0);

            var intStorage1 = new FloatStorage(10);
            intStorage1.ChangeCurrentValue(0);
            CheckValues(intStorage1, 10, 0, 0);
            intStorage1.ChangeCurrentValue(-1);
            CheckValues(intStorage1, 10, 0, 0);
            intStorage1.ChangeCurrentValue(1);
            CheckValues(intStorage1, 10, 1, 0);

            var intStorage2 = new FloatStorage(-10, -50, -100);
            intStorage2.ChangeCurrentValue(0);
            CheckValues(intStorage2, -10, -50, -100);
            intStorage2.ChangeCurrentValue(-1);
            CheckValues(intStorage2, -10, -51, -100);
            intStorage2.ChangeCurrentValue(1);
            CheckValues(intStorage2, -10, -50, -100);
        }

        private static void CheckValues(FloatStorage intStorage, float max, float cur, float min)
        {
            var actual = (intStorage.MaxValue, intStorage.CurrentValue, intStorage.MinValue);
            var expected = (max, cur, min);

            Assert.AreEqual(expected, actual);
        }
    }
}
