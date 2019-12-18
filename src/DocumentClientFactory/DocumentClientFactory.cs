using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Security;
using System.Text;

namespace finitelogic.Common.DocumentClientFactory
{
    public class DocumentClientFactory : IDocumentClientFactory
    {

        public IDocumentClient ReadOnlyDocumentClient
        { get { return _readOnlyDocumentClient.Value; } }
        public IDocumentClient ReadWriteDocumentClient
        {
            get { return _readWriteDocumentClient.Value; }
        }
        private static Lazy<IDocumentClient> _readOnlyDocumentClient = new Lazy<IDocumentClient>(() => GetClient(_endpoint, _authKeyRead, _connectionPolicy, _consistencyLevel.Value, isReadOnly: true));

        private static Lazy<IDocumentClient> _readWriteDocumentClient = new Lazy<IDocumentClient>(() => GetClient(_endpoint, _authKeyRead, _connectionPolicy, _consistencyLevel.Value, isReadOnly: false));

        private static ConnectionPolicy _connectionPolicy;
        private static ConsistencyLevel? _consistencyLevel;
        private static List<string> _preferredLocations;
        private static JsonSerializerSettings _serializerSettings;
        private static Uri _endpoint;
        private static SecureString _authKeyWrite;
        private static SecureString _authKeyRead;

        public DocumentClientFactory(Uri endpoint, SecureString authKeyWrite, SecureString authKeyRead, IList<string> preferredLocations, ConnectionPolicy connectionPolicy = null, ConsistencyLevel? consistencyLevel = null)
        {

            _endpoint = endpoint ?? throw new ArgumentNullException(nameof(endpoint));
            _authKeyWrite = authKeyWrite ?? throw new ArgumentNullException(nameof(authKeyWrite));
            _authKeyRead = authKeyRead ?? throw new ArgumentNullException(nameof(authKeyRead));
            _connectionPolicy = connectionPolicy ?? new ConnectionPolicy() { ConnectionProtocol = Protocol.Tcp, ConnectionMode = ConnectionMode.Direct };
            _consistencyLevel = consistencyLevel ?? ConsistencyLevel.Session;
            _serializerSettings = new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore };
            if (preferredLocations == null | preferredLocations.Count == 0)
                throw new InvalidOperationException($"{nameof(preferredLocations)} must have at least one location");
            _preferredLocations = (List<string>) preferredLocations;
            
        }

        private static IDocumentClient GetClient(Uri endpoint, SecureString authKey, ConnectionPolicy connectionPolicy, ConsistencyLevel consistencyLevel, bool isReadOnly)
        {
            if (isReadOnly)
            {
                for (int i =  _preferredLocations.Count-1; i >= 0; i--)
                    connectionPolicy.PreferredLocations.Add(_preferredLocations[i]);               
            }
            else
            {
                for (int i = 0; i < _preferredLocations.Count; i++)
                    connectionPolicy.PreferredLocations.Add(_preferredLocations[i]);
               
            }

            return new DocumentClient(endpoint, authKey, connectionPolicy, consistencyLevel);

        }

    }
}
