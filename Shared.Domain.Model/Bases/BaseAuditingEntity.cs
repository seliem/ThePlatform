
namespace Tamkeen.MCS.Shared.Domain.Model.Bases
{
    /// <summary>
    /// Created By  : 
    /// Date  :
    /// </summary>

    public interface IAuditable
    {
        /// <summary>
        /// return table name 
        /// </summary>
        /// <returns></returns>
        string GetTableName();

        /// <summary>
        /// return table primary  key 
        /// </summary>
        /// <returns></returns>
        string GetPrimaryKeyPropertyName();

        /// <summary>
        /// Return IsActive Property 
        /// </summary>
        /// <returns></returns>
        string GetIsActivePropertName();

        /// <summary>
        /// Is Deleted 
        /// </summary>
        /// <returns></returns>
        string GetIsDeletedPropertName();
    }
}
