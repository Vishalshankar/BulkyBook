using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository.IRepository
{
    public interface ISP_Call:IDisposable
    {
        T Single<T>(string procedureName, DynamicParameters param = null);//It uses execute scalar which return first row first column

        void Execute<T>(string procedureName, DynamicParameters param = null);//It willonle execute on data and donot return any thing

        T OneRecord<T>(string procedureName, DynamicParameters param = null);//It return only one record
        IEnumerable<T> List<T>(string procedureName, DynamicParameters param = null);//it return lists from one table
        Tuple<IEnumerable<T1>,IEnumerable<T2>> List<T1,T2>(string procedureName, DynamicParameters param = null);//It return list from two tables

    }
}
