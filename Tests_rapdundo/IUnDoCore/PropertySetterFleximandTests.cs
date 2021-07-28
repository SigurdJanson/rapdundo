using NUnit.Framework;
using RapdUnDo.IUndoCore;
using System;

namespace Tests_rapdundo.IUnDoCore
{
    [TestFixture]
    public class PropertySetterFleximandTests
    {
        const string DefaultName = "Default public name";
        const string DefaultProtectedName = "Default protected name";
        const string DefaultPrivateName = "Zeus";

        class GuineaPig
        {
            public string Name { get; set; } = DefaultName;
            public double NumberofHairs { get; set; } = 9999999;
            protected string ProtectedName { get; set; } = DefaultProtectedName;
            private string PrivateName { get; set; }

            public GuineaPig() => PrivateName = DefaultPrivateName;
        }


        #region Execution / Revokation Tests ================
        [Test]
        public void Execute_PublicStringProperty()
        {
            const string NewName = "This is the new name of the guinea pig";
            // Arrange
            GuineaPig TestObject = new();
            var PropertySetterFmd = new PropertySetterFleximand<GuineaPig, string>(TestObject, nameof(GuineaPig.Name), NewName);

            // Act
            Assume.That(TestObject.Name, Is.EqualTo(DefaultName));
            PropertySetterFmd.Execute();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(NewName, TestObject.Name);
                Assert.AreEqual(1, PropertySetterFmd.ExecutionTimes);
            });
        }



        [Test]
        public void Revoke_PublicStringProperty()
        {
            const string NewName = "This is the new name of the guinea pig";
            // Arrange
            GuineaPig TestObject = new();
            var PropertySetterFmd = new PropertySetterFleximand<GuineaPig, string>(TestObject, nameof(GuineaPig.Name), NewName);

            // Act
            Assume.That(TestObject.Name, Is.EqualTo(DefaultName));
            PropertySetterFmd.Execute();
            Assume.That(TestObject.Name, Is.EqualTo(NewName));
            PropertySetterFmd.Revoke();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(DefaultName, TestObject.Name);
                Assert.AreEqual(0, PropertySetterFmd.ExecutionTimes);
            });
        }



        [Test]
        public void CanExecute_PublicStringProperty_True()
        {
            const string NewName = "This is the new name of the guinea pig";
            // Arrange
            GuineaPig TestObject = new();
            var PropertySetterFmd = new PropertySetterFleximand<GuineaPig, string>(TestObject, nameof(GuineaPig.Name), NewName);

            // Act
            Assume.That(TestObject.Name, Is.EqualTo(DefaultName));
            PropertySetterFmd.Execute();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.IsTrue(PropertySetterFmd.CanExecute());
                Assert.AreEqual(1, PropertySetterFmd.ExecutionTimes);
            });
        }


        [Test]
        public void CanRevoke_PublicStringProperty_True()
        {
            const string NewName = "This is the new name of the guinea pig";
            // Arrange
            GuineaPig TestObject = new();
            var PropertySetterFmd = new PropertySetterFleximand<GuineaPig, string>(TestObject, nameof(GuineaPig.Name), NewName);

            // Act
            Assume.That(TestObject.Name, Is.EqualTo(DefaultName));
            PropertySetterFmd.Execute();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.IsTrue(PropertySetterFmd.CanRevoke());
                Assert.AreEqual(1, PropertySetterFmd.ExecutionTimes);
            });
        }


        [Test]
        public void CanRevoke_PublicStringProperty_False ()
        {
            const string NewName = "This is the new name of the guinea pig";
            // Arrange
            GuineaPig TestObject = new();
            var PropertySetterFmd = new PropertySetterFleximand<GuineaPig, string>(TestObject, nameof(GuineaPig.Name), NewName);

            // Act
            Assume.That(TestObject.Name, Is.EqualTo(DefaultName));
            //PropertySetterFmd.Execute(); // no undo unless command was executed

            // Assert
            Assert.Multiple(() =>
            {
                Assert.IsFalse(PropertySetterFmd.CanRevoke());
                Assert.AreEqual(0, PropertySetterFmd.ExecutionTimes);
            });
        }
        #endregion



        #region Can[...]Changed Tests =================

        [Test]
        public void CanRevokeChanged_PublicStringProperty_Raised()
        {
            const string NewName = "This is the new name of the guinea pig";
            // Arrange
            bool HasCanRevokeChangedBeenCaught = false;
            void CatchCanRevoke(object sender, EventArgs e) => HasCanRevokeChangedBeenCaught = true;

            GuineaPig TestObject = new();
            var PropertySetterFmd = new PropertySetterFleximand<GuineaPig, string>(TestObject, nameof(GuineaPig.Name), NewName);
            PropertySetterFmd.CanRevokeChanged += CatchCanRevoke;

            // Act
            Assume.That(PropertySetterFmd.CanRevoke(), Is.EqualTo(false));
            PropertySetterFmd.Execute();

            // Assert
            Assert.IsTrue(HasCanRevokeChangedBeenCaught);
        }

        #endregion



        #region Testing Constructor Exceptions ================

        [Test]
        public void Instantiate_ProtectedProperty_Exception()
        {
            const string NewName = "The new nickname of the guinea pig is panama";
            // Arrange
            GuineaPig TestObject = new();

            // Act
            // Assert
            Assert.Throws<ArgumentException>(() => {
                var PropertySetterFmd = new PropertySetterFleximand<GuineaPig, string>(TestObject, "ProtectedName", NewName);
            });
        }


        [Test]
        public void Instantiate_PrivateProperty_Exception()
        {
            const string NewName = "The true name of the guinea pig is Dieter Lorenz";
            // Arrange
            GuineaPig TestObject = new();

            // Act
            // Assert
            Assert.Throws<ArgumentException>(() => {
                var PropertySetterFmd = new PropertySetterFleximand<GuineaPig, string>(TestObject, "PrivateName", NewName);
            });
        }


        [Test]
        public void Instantiate_MissingProperty_Exception()
        {
            const string NewName = "Guinea pig, the great";
            // Arrange
            GuineaPig TestObject = new();

            // Act
            // Assert
            Assert.Throws<ArgumentException>(() => {
                var PropertySetterFmd = new PropertySetterFleximand<GuineaPig, string>(TestObject, "Unknown Property", NewName);
            });
        }


        [Test]
        public void Instantiate_ObjectIsNull_Exception()
        {
            const string NewName = "Guinea pig, the great";
            // Arrange
            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => {
                var PropertySetterFmd = new PropertySetterFleximand<GuineaPig, string>(null, nameof(GuineaPig.Name), NewName);
            });
        }

        #endregion
    }
}
