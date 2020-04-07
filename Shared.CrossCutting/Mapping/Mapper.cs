using System.Collections.Generic;
using System.Linq;

namespace Shared.CrossCutting.Mapping
{
    public abstract class Mapper<TEntity, TEditModel, TViewModel> where TEntity : class where TEditModel : class
        where TViewModel : class
    {

        public List<TViewModel> ToModels(List<TEntity> entities)
        {
            List<TViewModel> output = new List<TViewModel>();

            if (entities != null && entities.Any())
                foreach (var obj in entities)
                {
                    output.Add(ToModel(obj));
                }
            return output;
        }// ToModels


        public List<TEntity> ToEntities(List<TEditModel> models)
        {
            List<TEntity> output = new List<TEntity>();
            if (models != null && models.Any())
                foreach (var obj in models)
            {
                output.Add(ToEntity(obj));
            }
            return output;
        }// ToEntities

        public abstract TViewModel ToModel(TEntity entity);

        public abstract TEntity ToEntity(TEditModel model);

    }
}
