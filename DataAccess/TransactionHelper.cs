using System.Transactions;

namespace DataAccess
{
    public static class TransactionHelper
    {

        public static TransactionScope NewTransaction
        {
            get
            {
                return new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted });
            }
        }

    }
}
