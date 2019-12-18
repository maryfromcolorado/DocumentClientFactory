using Microsoft.Azure.Documents;

namespace finitelogic.Common.DocumentClientFactory
{
    public interface IDocumentClientFactory
    {
        IDocumentClient ReadOnlyDocumentClient { get; }
        IDocumentClient ReadWriteDocumentClient { get; }
    }
}