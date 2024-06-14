using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace SR_Editor.Core
{
    public class EditorTransactionScope : IDisposable
    {
        private static List<EditorTransactionScope> scopes;

        private bool initialized = false;

        private bool disposed = false;

        private SqlConnection connection;

        private TransactionScope scope;

        private EditorTransactionScope parent;

        private System.Threading.Thread thread;

        private string connectionString = string.Empty;

        public SqlConnection Connection
        {
            get
            {
                SqlConnection connection;
                if (this.parent == null)
                {
                    if (this.connection == null)
                    {
                        this.connection = new SqlConnection()
                        {
                            ConnectionString = this.ConnectionString
                        };
                        this.connection.Open();
                    }
                    connection = this.connection;
                }
                else
                {
                    connection = this.parent.Connection;
                }
                return connection;
            }
        }

        public string ConnectionString
        {
            get
            {
                string str;
                str = (!(this.connectionString == string.Empty) ? this.connectionString : System.Configuration.ConfigurationManager.ConnectionStrings["SilkroadModel"].ConnectionString);
                return str;
            }
        }

        public EditorTransactionScope Parent
        {
            get
            {
                return this.parent;
            }
        }

        public System.Threading.Thread Thread
        {
            get
            {
                return this.thread;
            }
        }

        static EditorTransactionScope()
        {
            EditorTransactionScope.scopes = new List<EditorTransactionScope>();
        }

        public EditorTransactionScope()
        {
            this.Initialize(TransactionScopeOption.Required, IsolationLevel.ReadCommitted, new TimeSpan(0, 0, 3, 0, 0));
        }

        public EditorTransactionScope(string ConnectionString)
        {
            this.connectionString = ConnectionString;
            this.Initialize(TransactionScopeOption.Required, IsolationLevel.ReadCommitted, new TimeSpan(0, 0, 3, 0, 0));
        }

        public EditorTransactionScope(TransactionScopeOption scopeOption)
        {
            this.Initialize(scopeOption, IsolationLevel.ReadCommitted, new TimeSpan(0, 0, 3, 0, 0));
        }

        public EditorTransactionScope(TransactionScopeOption scopeOption, string ConnectionString)
        {
            this.connectionString = ConnectionString;
            this.Initialize(scopeOption, IsolationLevel.ReadCommitted, new TimeSpan(0, 0, 3, 0, 0));
        }

        public EditorTransactionScope(IsolationLevel isolationLevel)
        {
            this.Initialize(TransactionScopeOption.Required, isolationLevel, new TimeSpan(0, 0, 3, 0, 0));
        }

        public EditorTransactionScope(TimeSpan scopeTimeout)
        {
            this.Initialize(TransactionScopeOption.Required, IsolationLevel.ReadCommitted, scopeTimeout);
        }

        public EditorTransactionScope(TransactionScopeOption scopeOption, IsolationLevel isolationLevel)
        {
            this.Initialize(scopeOption, isolationLevel, new TimeSpan(0, 0, 3, 0, 0));
        }

        public EditorTransactionScope(TransactionScopeOption scopeOption, IsolationLevel isolationLevel, TimeSpan scopeTimeout)
        {
            this.Initialize(scopeOption, isolationLevel, scopeTimeout);
        }

        public void Complete()
        {
            if (this.scope != null)
            {
                this.scope.Complete();
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                try
                {
                    if (disposing)
                    {
                        if (this.connection != null)
                        {
                            try
                            {
                                try
                                {
                                    this.connection.Close();
                                }
                                catch
                                {
                                }
                            }
                            finally
                            {
                                this.connection.Dispose();
                            }
                        }
                        if (this.scope != null)
                        {
                            this.scope.Dispose();
                        }
                    }
                }
                finally
                {
                    lock (EditorTransactionScope.scopes)
                    {
                        EditorTransactionScope.scopes.Remove(this);
                    }
                }
            }
            this.disposed = true;
        }

        ~EditorTransactionScope()
        {
            this.Dispose(false);
        }

        private static EditorTransactionScope FindParentScope(EditorTransactionScope scope)
        {
            EditorTransactionScope item;
            lock (EditorTransactionScope.scopes)
            {
                int count = EditorTransactionScope.scopes.Count - 1;
                while (count >= 0)
                {
                    if ((EditorTransactionScope.scopes[count] == scope ? true : EditorTransactionScope.scopes[count].thread != scope.thread))
                    {
                        count--;
                    }
                    else
                    {
                        item = EditorTransactionScope.scopes[count];
                        return item;
                    }
                }
                item = null;
            }
            return item;
        }

        private void Initialize(TransactionScopeOption scopeOption, IsolationLevel isolationLevel, TimeSpan scopeTimeout)
        {
            if (!this.initialized)
            {
                TransactionOptions transactionOption = new TransactionOptions()
                {
                    IsolationLevel = isolationLevel,
                    Timeout = scopeTimeout
                };
                this.thread = System.Threading.Thread.CurrentThread;
                if (scopeOption == TransactionScopeOption.Required)
                {
                    this.parent = EditorTransactionScope.FindParentScope(this);
                }
                if (this.parent == null)
                {
                    if (scopeOption != TransactionScopeOption.Suppress)
                    {
                        scopeOption = TransactionScopeOption.RequiresNew;
                    }
                    this.scope = new TransactionScope(scopeOption, transactionOption);
                }
                lock (EditorTransactionScope.scopes)
                {
                    EditorTransactionScope.scopes.Add(this);
                }
            }
        }
    }
}
