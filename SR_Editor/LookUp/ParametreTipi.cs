using SR_Editor.LookUp.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SR_Editor.LookUp
{
    public class LookUpSqlParametreTipi : LookUpEntityBase<byte>
    {
        public LookUpSqlParametreTipi()
        {
        }

        public LookUpSqlParametreTipi(byte pKey)
          : base(pKey)
        {
        }

        public LookUpSqlParametreTipi(byte pKey, string pValue)
          : base(pKey, pValue)
        {
        }
    }
    public class SqlParametreTipi : LookUpEntityBaseList<byte, LookUpSqlParametreTipi>
    {
        private static SqlParametreTipi _liste;

        public static SqlParametreTipi Liste
        {
            get
            {
                return SqlParametreTipi._liste ?? (SqlParametreTipi._liste = new SqlParametreTipi());
            }
        }

        public SqlParametreTipi()
        {
            this.Load();
        }

        public override sealed void Load()
        {
            this.Add(new LookUpSqlParametreTipi((byte)1, "NVarChar"));
            this.Add(new LookUpSqlParametreTipi((byte)2, "DateTime"));
            this.Add(new LookUpSqlParametreTipi((byte)3, "Bit"));
            this.Add(new LookUpSqlParametreTipi((byte)4, "IPAddress"));
        }
    }
    public enum EnumSqlParametreTipi
    {
        NVarChar = 1,
        DateTime = 2,
        Bit = 3,
        IPAddress = 4,
    }

    public static class EnumSqlParametreTipiConverter
    {
        public static DbType ToDbType(EnumSqlParametreTipi tipi)
        {
            switch (tipi)
            {
                case EnumSqlParametreTipi.NVarChar:
                    return DbType.String;
                case EnumSqlParametreTipi.DateTime:
                    return DbType.DateTime;
                case EnumSqlParametreTipi.Bit:
                    return DbType.Boolean;
                case EnumSqlParametreTipi.IPAddress:
                    return DbType.Int32;
                default:
                    return DbType.String;
            }
        }
    }
}
