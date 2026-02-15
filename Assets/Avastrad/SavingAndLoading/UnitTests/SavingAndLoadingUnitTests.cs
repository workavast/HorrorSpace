using System;
using NUnit.Framework;
using UnityEngine;

namespace Avastrad.SavingAndLoading
{
    public class SavingAndLoadingUnitTests
    {
        [Test]
        public void BinarySaveAndLoaderTest()
        {
            var save = new SomeSave("someString");
            
            ISaveAndLoader saveAndLoader = new BinarySaveAndLoader();
            
            saveAndLoader.DeleteSave();
            Assert.AreEqual(false, saveAndLoader.Exist());
            saveAndLoader.Save(save);
            Assert.AreEqual(true, saveAndLoader.Exist());
            var loadedSave = saveAndLoader.Load<SomeSave>();
            Assert.AreEqual(true, saveAndLoader.Exist());

            Assert.AreEqual(save._publicString, loadedSave._publicString);
            Assert.AreEqual(save.PrivateString, loadedSave.PrivateString);
            Assert.AreEqual(save.ProtectedString, loadedSave.ProtectedString);

            Assert.AreEqual(save._serPublicString, loadedSave._serPublicString);
            Assert.AreEqual(save.SerPrivateString, loadedSave.SerPrivateString);
            Assert.AreEqual(save.SerProtectedString, loadedSave.SerProtectedString);
            
            Assert.AreEqual(null, loadedSave._nonSerPublicString);
            Assert.AreEqual(null, loadedSave.NonSerPrivateString);
            Assert.AreEqual(null, loadedSave.NonSerProtectedString);
            
            Assert.AreEqual(save.PublicStringProperty, loadedSave.PublicStringProperty);
            Assert.AreEqual(save.GetPrivateStringProperty, loadedSave.GetPrivateStringProperty);
            Assert.AreEqual(save.GetProtectedStringProperty, loadedSave.GetProtectedStringProperty);
            
            Assert.AreEqual(save.SerPublicStringProperty, loadedSave.SerPublicStringProperty);
            Assert.AreEqual(save.GetSerPrivateStringProperty, loadedSave.GetSerPrivateStringProperty);
            Assert.AreEqual(save.GetSerProtectedStringProperty, loadedSave.GetSerProtectedStringProperty);

            Assert.AreEqual(null, loadedSave.NonSerPublicStringProperty);
            Assert.AreEqual(null, loadedSave.GetNonSerPrivateStringProperty);
            Assert.AreEqual(null, loadedSave.GetNonSerProtectedStringProperty);
            
            saveAndLoader.DeleteSave();
        }
        
        [Test]
        public void JsonSaveAndLoaderTest()
        {
            var save = new SomeSave("someString");
            
            ISaveAndLoader saveAndLoader = new JsonSaveAndLoader();
            
            saveAndLoader.DeleteSave();
            Assert.AreEqual(false, saveAndLoader.Exist());
            saveAndLoader.Save(save);
            Assert.AreEqual(true, saveAndLoader.Exist());
            var loadedSave = saveAndLoader.Load<SomeSave>();
            Assert.AreEqual(true, saveAndLoader.Exist());

            Assert.AreEqual(save._publicString, loadedSave._publicString);
            Assert.AreEqual(null, loadedSave.PrivateString);
            Assert.AreEqual(null, loadedSave.ProtectedString);

            Assert.AreEqual(save._serPublicString, loadedSave._serPublicString);
            Assert.AreEqual(save.SerPrivateString, loadedSave.SerPrivateString);
            Assert.AreEqual(save.SerProtectedString, loadedSave.SerProtectedString);
            
            Assert.AreEqual(null, loadedSave._nonSerPublicString);
            Assert.AreEqual(null, loadedSave.NonSerPrivateString);
            Assert.AreEqual(null, loadedSave.NonSerProtectedString);
            
            Assert.AreEqual(null, loadedSave.PublicStringProperty);
            Assert.AreEqual(null, loadedSave.GetPrivateStringProperty);
            Assert.AreEqual(null, loadedSave.GetProtectedStringProperty);
            
            Assert.AreEqual(save.SerPublicStringProperty, loadedSave.SerPublicStringProperty);
            Assert.AreEqual(save.GetSerPrivateStringProperty, loadedSave.GetSerPrivateStringProperty);
            Assert.AreEqual(save.GetSerProtectedStringProperty, loadedSave.GetSerProtectedStringProperty);

            Assert.AreEqual(null, loadedSave.NonSerPublicStringProperty);
            Assert.AreEqual(null, loadedSave.GetNonSerPrivateStringProperty);
            Assert.AreEqual(null, loadedSave.GetNonSerProtectedStringProperty);
            
            saveAndLoader.DeleteSave();
        }
        
        [Test]
        public void PlayerPrefsSaveAndLoaderTest()
        {
            var save = new SomeSave("someString");
            
            ISaveAndLoader saveAndLoader = new PlayerPrefsSaveAndLoader();
            
            saveAndLoader.DeleteSave();
            Assert.AreEqual(false, saveAndLoader.Exist());
            saveAndLoader.Save(save);
            Assert.AreEqual(true, saveAndLoader.Exist());
            var loadedSave = saveAndLoader.Load<SomeSave>();
            Assert.AreEqual(true, saveAndLoader.Exist());

            Assert.AreEqual(save._publicString, loadedSave._publicString);
            Assert.AreEqual(null, loadedSave.PrivateString);
            Assert.AreEqual(null, loadedSave.ProtectedString);

            Assert.AreEqual(save._serPublicString, loadedSave._serPublicString);
            Assert.AreEqual(save.SerPrivateString, loadedSave.SerPrivateString);
            Assert.AreEqual(save.SerProtectedString, loadedSave.SerProtectedString);
            
            Assert.AreEqual(null, loadedSave._nonSerPublicString);
            Assert.AreEqual(null, loadedSave.NonSerPrivateString);
            Assert.AreEqual(null, loadedSave.NonSerProtectedString);
            
            Assert.AreEqual(null, loadedSave.PublicStringProperty);
            Assert.AreEqual(null, loadedSave.GetPrivateStringProperty);
            Assert.AreEqual(null, loadedSave.GetProtectedStringProperty);
            
            Assert.AreEqual(save.SerPublicStringProperty, loadedSave.SerPublicStringProperty);
            Assert.AreEqual(save.GetSerPrivateStringProperty, loadedSave.GetSerPrivateStringProperty);
            Assert.AreEqual(save.GetSerProtectedStringProperty, loadedSave.GetSerProtectedStringProperty);

            Assert.AreEqual(null, loadedSave.NonSerPublicStringProperty);
            Assert.AreEqual(null, loadedSave.GetNonSerPrivateStringProperty);
            Assert.AreEqual(null, loadedSave.GetNonSerProtectedStringProperty);
            
            saveAndLoader.DeleteSave();
        }
        
        
        [Serializable]
        private class SomeSave
        {
            public string _publicString;
            private string _privateString;
            protected string _protectedString;
            
            [SerializeField] public string _serPublicString;
            [SerializeField] private string _serPrivateString;
            [SerializeField] protected string _serProtectedString;
            
            [NonSerialized] public string _nonSerPublicString;
            [NonSerialized] private string _nonSerPrivateString;
            [NonSerialized] protected string _nonSerProtectedString;
            
            public string PublicStringProperty { get; private set; }
            private string PrivateStringProperty { get; set; }
            protected string ProtectedStringProperty { get; private set; }

            [field: SerializeField] public string SerPublicStringProperty { get; private set; }
            [field: SerializeField] private string SerPrivateStringProperty { get; set; }
            [field: SerializeField] protected string SerProtectedStringProperty { get; private set; }
            
            [field: NonSerialized] public string NonSerPublicStringProperty { get; private set; }
            [field: NonSerialized] private string NonSerPrivateStringProperty { get; set; }
            [field: NonSerialized] protected string NonSerProtectedStringProperty { get; private set; }
            
            public string PrivateString => _privateString;
            public string ProtectedString => _protectedString;
            
            public string SerPrivateString => _serPrivateString;
            public string SerProtectedString => _serProtectedString;
            
            public string NonSerPrivateString => _nonSerPrivateString;
            public string NonSerProtectedString => _nonSerProtectedString;
            
            public string GetPrivateStringProperty => PrivateStringProperty;
            public string GetProtectedStringProperty => ProtectedStringProperty;
            
            public string GetSerPrivateStringProperty => SerPrivateStringProperty;
            public string GetSerProtectedStringProperty => SerProtectedStringProperty;
            
            public string GetNonSerPrivateStringProperty => NonSerPrivateStringProperty;
            public string GetNonSerProtectedStringProperty => NonSerProtectedStringProperty;
            
            public SomeSave()
            {
                
            }
            
            public SomeSave(string someString)
            {
                _publicString = _privateString = _protectedString
                    = _serPublicString = _serPrivateString = _serProtectedString
                        = _nonSerPublicString = _nonSerPrivateString = _nonSerProtectedString
                            = PublicStringProperty = PrivateStringProperty = ProtectedStringProperty
                                = SerPublicStringProperty = SerPrivateStringProperty = SerProtectedStringProperty
                                    = NonSerPublicStringProperty = NonSerPrivateStringProperty = NonSerProtectedStringProperty =
                                        someString;
            }
        }
    }
}
