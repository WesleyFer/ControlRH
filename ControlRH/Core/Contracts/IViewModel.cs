using ControlRH.Models;

namespace ControlRH.Core.Contracts
{
    public interface IViewModel<TEntity> where TEntity : class
    {
        /// <summary>
        /// Cria um novo modelo a partir do ViewModel
        /// </summary>
        /// <returns></returns>
        TEntity ToModel();
        //TEntity ToModel(TEntity? entity = null);

        /// <summary>
        /// Preenche o ViewModel a partir de um modelo da entidade
        /// </summary>
        /// <param name="entity"></param>
        void ToViewModel(TEntity entity);
    }
}
