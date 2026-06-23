namespace CarHierarchy.Patterns.Observer
{
    /// <summary>
    /// ПАТТЕРН: Наблюдатель (Observer)
    ///
    /// Observer — интерфейс подписчика.
    /// Реализуется любым объектом, который хочет получать
    /// уведомления об изменении списка автомобилей.
    ///
    /// Уместность: в MainForm обновление ListBox, счётчика и PropertyGrid
    /// вызывалось вручную из каждого обработчика (Add/Delete/SaveChange).
    /// Observer отделяет логику «что изменилось» от «кто должен отреагировать»,
    /// устраняя дублирование вызовов UpdateCarList() / UpdateCarCount().
    /// </summary>
    public interface ICarListObserver
    {
        void OnCarsChanged(IReadOnlyList<CarHierarchyLib.Models.Car> cars);
    }
}