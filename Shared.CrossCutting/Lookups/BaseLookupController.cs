using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Shared.Domain.Model.Bases;
using Shared.Infrastucture.Data.Core.Contracts;
using Shared.CrossCutting.Search;
using Shared.CrossCutting.RestResult;
using Shared.CrossCutting.Mapping;
using System.Net;
using Shared.CrossCutting.Logging;
using Shared.CrossCutting.Logging.Resources;

namespace Shared.CrossCutting.Lookups
{
    [RoutePrefix("api/Lookup/[controller]")]
    public abstract class BaseLookupController<T> : ApiController where T : BaseLookup
    {

        protected readonly IRepository<T> _repository;
        protected readonly IFileLogger _logger;

        protected readonly string _parentKey;

        public BaseLookupController(IRepository<T> repository, string parentKey, IFileLogger logger)
        {
            _logger = logger;
            _repository = repository;
            _parentKey = parentKey;
        }

        public BaseLookupController(IRepository<T> repository, IFileLogger logger)
        {
            _logger = logger;

            _repository = repository;
        }

        /// <summary>
        /// Get all Lookup Items.
        /// </summary>
        /// <param name="lookupName"></param>
        /// <returns></returns>
        [HttpGet, Route()]
        [Route("ActiveLookupItems")]
        public virtual IHttpActionResult ActiveLookupItems()
        {
            try
            {
                var result = _repository.GetAll(x => x.IsVisible != false, false).OrderBy(x => x.Id);

                return ApiResult<ListModel<T>>.Success(new ListModel<T>(result.ToList(), result.Count()));
            }

            catch (Exception ex)
            {

                _logger.Log($"Error: {nameof(ActiveLookupItems)}", $"{nameof(ActiveLookupItems)} Throws Exception: {ex.Message} ", ex, true);

                _logger.LogToElmah(ex);

                return ApiResult<ListModel<T>>.ServerError(ex, new BaseApiResponse()
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    BusinessStatusCode = ErrorCodes.GET_LOOKUPSITEMS_ERROR.ToString(),
                    MessageAr = ArabicMessages.GET_LOOKUPSITEMS_ERROR,
                    MessageEn = EnglishMessages.GET_LOOKUPSITEMS_ERROR + Environment.NewLine + ex.Message,
                });
            }
        }

        /// <summary>
        /// Get Lookup Item by Id.
        /// </summary>
        /// <param name="lookupName"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{Id}")]
        public IHttpActionResult LookupItem(int id)
        {
            try
            {
                var result = _repository.GetByID(id);

                return ApiResult<T>.Success(result);

            }
            catch (Exception ex)
            {
                _logger.Log($"Error: {nameof(LookupItem)}", $"{nameof(LookupItem)} Throws Exception: {ex.Message} ", ex, true);
                _logger.LogToElmah(ex);

                return ApiResult<ListModel<T>>.ServerError(ex, new BaseApiResponse()
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    BusinessStatusCode = ErrorCodes.GET_LOOKUPSITEM_ERROR.ToString(),
                    MessageAr = ArabicMessages.GET_LOOKUPSITEM_ERROR,
                    MessageEn = EnglishMessages.GET_LOOKUPSITEM_ERROR + Environment.NewLine + ex.Message,
                });
            }
        }

        /// <summary>
        /// Get Lookup Items using IsDeleted Flag.
        /// </summary>
        /// <param name="lookupName"></param>
        /// <param name="includeDeleted"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("LookupItems/{IncludeSoftDeleted?}")]
        public virtual IHttpActionResult LookupItems(bool? includeSoftDeleted = false)
        {
            try
            {
                var result = _repository.GetAll(x => x.IsVisible != false, includeSoftDeleted.Value).OrderBy(x => x.Id);

                return ApiResult<List<T>>.Success(result.ToList());
            }

            catch (Exception ex)
            {
                _logger.Log($"Error: {nameof(LookupItems)}", $"{nameof(LookupItems)} Throws Exception: {ex.Message} ", ex, true);

                _logger.LogToElmah(ex);


                return ApiResult<ListModel<T>>.ServerError(ex, new BaseApiResponse()
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    BusinessStatusCode = ErrorCodes.GET_LOOKUPSITEMS_ERROR.ToString(),
                    MessageAr = ArabicMessages.GET_LOOKUPSITEMS_ERROR,
                    MessageEn = EnglishMessages.GET_LOOKUPSITEMS_ERROR + Environment.NewLine + ex.Message,
                });
            }
        }

        [HttpGet]
        [Route("LookupItemsByParentId/{ParentId}/{IncludeSoftDeleted?}")]
        public virtual IHttpActionResult LookupItemsByParentId(int parentId, bool? includeSoftDeleted = false)
        {
            try
            {

                var result = _repository.GetAll(x => x.IsVisible != false, includeSoftDeleted.Value).OrderBy(x => x.Id).AsQueryable();

                result = result.Filter<T>(_parentKey, typeof(int), parentId, true);

                return ApiResult<List<T>>.Success(result.ToList());
            }

            catch (Exception ex)
            {
                _logger.Log($"Error: {nameof(LookupItemsByParentId)}", $"{nameof(LookupItemsByParentId)} Throws Exception: {ex.Message} ", ex, true);
                _logger.LogToElmah(ex);

                return ApiResult<ListModel<T>>.ServerError(ex, new BaseApiResponse()
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    BusinessStatusCode = ErrorCodes.GET_LOOKUPSITEMS_ERROR.ToString(),
                    MessageAr = ArabicMessages.GET_LOOKUPSITEMS_ERROR,
                    MessageEn = EnglishMessages.GET_LOOKUPSITEMS_ERROR + Environment.NewLine + ex.Message,
                });
            }
        }
    }
}
