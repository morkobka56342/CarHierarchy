using CarHierarchyLib.Models;
using System.Collections.Generic;

namespace CarHierarchy.Patterns.Observer
{

    public class CarListSubject
    {
        private readonly List<Car> _cars = new();
        private readonly List<ICarListObserver> _observers = new();

        public void Subscribe(ICarListObserver observer)
        {
            if (!_observers.Contains(observer))
                _observers.Add(observer);
        }

        public void Unsubscribe(ICarListObserver observer)
        {
            _observers.Remove(observer);
        }

        public void AddCar(Car car)
        {
            _cars.Add(car);
            Notify();
        }

        public void RemoveCar(Car car)
        {
            _cars.Remove(car);
            Notify();
        }

        public void SetCars(List<Car> cars)
        {
            _cars.Clear();
            _cars.AddRange(cars);
            Notify();
        }

        public void SortByBrand()
        {
            var sorted = _cars.OrderBy(c => c.Brand).ThenBy(c => c.Model).ToList();
            _cars.Clear();
            _cars.AddRange(sorted);
            Notify();
        }

        public void NotifyChanged() => Notify();

        public IReadOnlyList<Car> Cars => _cars.AsReadOnly();

        public List<Car> GetRawList() => _cars;

        private void Notify()
        {
            foreach (var obs in _observers)
                obs.OnCarsChanged(_cars.AsReadOnly());
        }
    }
}