using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using CryptSharp.Utility;
using Sodium;

namespace SQRL.Client
{
    public sealed class Identity
    {
        private const int RandomEntropyBytes = 256;
        private const int ScryptIterations = 1024;
        private const int KeyWidth = 256/8;

        private readonly string _name;
        private readonly string _password;
        private byte[] _derivedMasterKey;
        private byte[] _masterKeySalt;

        private Identity(string name, string password)
        {
            _name = name;
            _password = password;
        }

        public static IIdentityStorageProvider StorageProvider { get; set; }

        public static Identity CreateNew(string name, string password, byte[] entropy)
        {
            if (name == null) throw new ArgumentNullException("name");
            if (password == null) throw new ArgumentNullException("password");
            if (entropy == null) throw new ArgumentNullException("entropy");
            
            var id= new Identity(name, password);

            id.GenerateMasterKey(entropy);

            return id;
        }

        public static Identity Open(string name, string password)
        {
            if (name == null) throw new ArgumentNullException("name");
            if (password == null) throw new ArgumentNullException("password");

            var id = new Identity(name, password);

            id.LoadIdentity();

            return id;
        }

        private void GenerateMasterKey(byte[] entropy)
        {
            byte[] generatedEntropy = Sodium.Random.GetBytes(RandomEntropyBytes);
            _masterKeySalt = Sodium.Random.GetBytes(RandomEntropyBytes);

            var master = SCrypt.ComputeDerivedKey(generatedEntropy, entropy, ScryptIterations, 1024, 1, null, KeyWidth);

            GenerateDerivedMasterKey(master);

            SaveIdentity(master);

            Array.Clear(master, 0, master.Length);
        }

        private void GenerateDerivedMasterKey(byte[] master)
        {
            var passwordHashBytes = GetPasswordHash();
            _derivedMasterKey = new byte[KeyWidth];

            for (int i = 0; i < master.Length; i++)
            {
                _derivedMasterKey[i] = (byte) (master[i] ^ passwordHashBytes[i]);
            }

            Array.Clear(passwordHashBytes, 0, passwordHashBytes.Length);
        }

        private void SaveIdentity(byte[] master)
        {
            if (StorageProvider == null)
            {
                return;
            }

            var verfier = CryptoHash.SHA256(_derivedMasterKey);
            Array.Resize(ref verfier, 16);
            var store = new IdentityStore
                {
                    MasterKey = master,
                    Salt = _masterKeySalt,
                    Verifier = verfier,
                    Iterations = ScryptIterations
                };

            var serializer = new BinaryFormatter();
            var stream = new MemoryStream();
            serializer.Serialize(stream, store);

            var protectedData = ProtectedData.Protect(stream.GetBuffer(), null, DataProtectionScope.CurrentUser);
            string encryptedString = Convert.ToBase64String(protectedData);
            StorageProvider.Save(_name, encryptedString);
        }

        private void LoadIdentity()
        {
            if (StorageProvider == null)
            {
                throw new InvalidOperationException("Unable to load identity.  No Storage Provider has been specified.");
            }

            string encryptedString = StorageProvider.Load(_name);
            var protectedData = Convert.FromBase64String(encryptedString);
            var data = ProtectedData.Unprotect(protectedData, null, DataProtectionScope.CurrentUser);
            
            var serializer = new BinaryFormatter();
            var store = (IdentityStore)serializer.Deserialize(new MemoryStream(data));

            _masterKeySalt = store.Salt;
            GenerateDerivedMasterKey(store.MasterKey);

            VerifyIdentity(store.Verifier);
        }

        private void VerifyIdentity(byte[] verifier)
        {
            byte[] computed = GetVerifierHash();
            if (!computed.SequenceEqual(verifier))
            {
                throw new Exception("Invalid password.");
            }
        }

        private byte[] GetVerifierHash()
        {
            var verfier = CryptoHash.SHA256(_derivedMasterKey);
            Array.Resize(ref verfier, 16);
            return verfier;
        }

        private byte[] GetPasswordHash()
        {
            var passwordHashBytes = Encoding.ASCII.GetBytes(_password);
            return SCrypt.ComputeDerivedKey(passwordHashBytes, _masterKeySalt, ScryptIterations, 1024, 1, null, KeyWidth);
        }

        public byte[] GetSitePrivateKey(string domain)
        {
            return CryptoAuth.AuthHmacSha512_256(domain, _derivedMasterKey);
        }

        [Serializable]
        private class IdentityStore
        {
            public byte[] MasterKey { get; set; }
            public byte[] Salt { get; set; }
            public byte[] Verifier { get; set; }
            public int Iterations { get; set; }
        }
    }
}