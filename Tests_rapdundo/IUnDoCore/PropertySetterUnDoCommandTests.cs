using NUnit.Framework;
using RapdUnDo.IUndoCore;
using System;

namespace Tests_rapdundo.IUnDoCore
{
    [TestFixture]
    public class PropertySetterUnDoCommandTests
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



        [Test]
        public void Execute_PublicStringProperty()
        {
            const string NewName = "This is the new name of the guinea pig";
            // Arrange
            GuineaPig TestObject = new();
            var propertySetterUnDoCommand = new PropertySetterUnDoCommand<GuineaPig, string>(TestObject, nameof(GuineaPig.Name), NewName);

            // Act
            Assume.That(TestObject.Name, Is.EqualTo(DefaultName));
            propertySetterUnDoCommand.Execute();

            // Assert
            Assert.AreEqual(NewName, TestObject.Name);
        }



        [Test]
        public void Revert_PublicStringProperty()
        {
            const string NewName = "This is the new name of the guinea pig";
            // Arrange
            GuineaPig TestObject = new();
            var propertySetterUnDoCommand = new PropertySetterUnDoCommand<GuineaPig, string>(TestObject, nameof(GuineaPig.Name), NewName);

            // Act
            Assume.That(TestObject.Name, Is.EqualTo(DefaultName));
            propertySetterUnDoCommand.Execute();
            Assume.That(TestObject.Name, Is.EqualTo(NewName));
            propertySetterUnDoCommand.Revoke();

            // Assert
            Assert.AreEqual(DefaultName, TestObject.Name);
        }


        #region Testing Exceptions ================

        [Test]
        public void Instantiate_ProtectedProperty_Exception()
        {
            const string NewName = "The new nickname of the guinea pig is panama";
            // Arrange
            GuineaPig TestObject = new();

            // Act
            // Assert
            Assert.Throws<ArgumentException>(() => {
                var propertySetterUnDoCommand = new PropertySetterUnDoCommand<GuineaPig, string>(TestObject, "ProtectedName", NewName);
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
                var propertySetterUnDoCommand = new PropertySetterUnDoCommand<GuineaPig, string>(TestObject, "PrivateName", NewName);
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
                var propertySetterUnDoCommand = new PropertySetterUnDoCommand<GuineaPig, string>(TestObject, "Unknown Property", NewName);
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
                var propertySetterUnDoCommand = new PropertySetterUnDoCommand<GuineaPig, string>(null, nameof(GuineaPig.Name), NewName);
            });
        }

        #endregion
    }
}
