using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Infrastucture.Data.Core.Contracts
{
    /// <summary>
    /// /
    /// 
    /// </summary>
    public interface IUnitOfWork //: IDisposable
    {

        /// <summary>
        /// get DbContext
        /// </summary>
        DbContext DbContext { get; }

        /// <summary>
        /// Commit
        /// </summary>
        int Commit();

        /// <summary>
        /// commit repos changes
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        int Commit(long userId);

        ///// <summary>
        ///// Save , Audit , Event Changes 
        ///// </summary>
        ///// <param name="userId"></param>
        ///// <param name="eventObject"></param>
        ///// <returns></returns>
        //int Commit(long userId, List<EventLog> eventObjects);

        /// <summary>
        /// rollback 
        /// </summary>
        void Rollback();

    }
}
