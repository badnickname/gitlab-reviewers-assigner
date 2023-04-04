using GitlabMonitor.Model.Merge;
using GitlabMonitor.Model.Statistic;

namespace GitlabMonitor.Model;

public interface IContext
{
    /// <summary>
    ///     Найти пользователей по Username
    /// </summary>
    /// <param name="userNames">Список имен</param>
    /// <returns>Список пользователей</returns>
    public Task<ICollection<User>> GetUsersByUsernamesAsync(ICollection<string> userNames, CancellationToken token);

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
    /// <returns>Словарь [ид_пользователя] = количество реквестов</returns>
    public Task<IDictionary<int, int>> GetAssignedMergeRequestsCountAsync(CancellationToken token);

    /// <summary>
    ///     Получить последние обработанные МР
    /// </summary>
    public Task<ICollection<AssignedMergeRequest>> GetLastAssignedMergeRequests(CancellationToken token);

    /// <summary>
    ///     Назначить пользователя ревьюить МР
    /// </summary>
    /// <param name="projectId">Ид проекта</param>
    /// <param name="mergeRequestId">Ид МР</param>
    /// <param name="userId">Ид назначаемого пользователя</param>
    public Task AssignToMergeRequestAsync(int projectId, int mergeRequestId, int userId, CancellationToken token);
}