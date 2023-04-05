using GitlabMonitor.Model.Merge;
using GitlabMonitor.Model.Statistic;

namespace GitlabMonitor.Model;

public interface IContext
{
    /// <summary>
    ///     Создает список пользователей по userName
    /// </summary>
    /// <param name="userIds">Список Id</param>
    /// <returns>Список пользователей</returns>
    public Task CreateUsers(ICollection<int> userIds, CancellationToken token);

    /// <summary>
    /// Найти ид пользователя для бота
    /// </summary>
    /// <returns>user_id</returns>
    public Task<int> GetBotUserIdAsync(CancellationToken token);

    /// <summary>
    ///     Загрузить текущие открытые MergeRequests из проектов
    /// </summary>
    /// <param name="projectIds">Id проектов</param>
    /// <returns>Список МР'ов</returns>
    public Task<ICollection<MergeRequest>> GetMergeRequestsFromProjectsAsync(ICollection<int> projectIds,
        CancellationToken token);

    /// <summary>
    ///     Получить количество назначенных МР на человека
    /// </summary>
    /// <returns>Кортедж [ид_пользователя] = количество реквестов</returns>
    public Task<ICollection<(int UserId, int Count)>> GetAssignedMergeRequestsCountAsync(CancellationToken token);

    /// <summary>
    ///     Получить последние обработанные МР
    /// </summary>
    public Task<ICollection<AssignedMergeRequest>> GetLastAssignedMergeRequests(CancellationToken token);

    /// <summary>
    ///     Назначить пользователя ревьюить МР
    /// </summary>
    /// <param name="projectId">Ид проекта</param>
    /// <param name="mergeRequestId">Ид МР</param>
    /// <param name="title">Название МР'а</param>
    /// <param name="reference">Ссылки</param>
    /// <param name="userId">Ид назначаемого пользователя</param>
    public Task AssignToMergeRequestAsync(int projectId, int mergeRequestId, string title, string reference, int userId, CancellationToken token);
}