using BaseLibrary.DTOs;
using BaseLibrary.Responses;

namespace ServerLibrary.Repositories.Contracts
{
    public interface ICommonInterface<T>
    {
        Task<List<T>> GetAll();
        Task<T> Get(string id);

        Task<GeneralResponse> Insert(T Item);

        Task<GeneralResponse> Update(T Item);

        Task<GeneralResponse> Delete(string Item);

    }
}
